using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnScreenJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private PlayerMoveController MoveController => PlayerMoveController.Instance;

    [SerializeField] private RectTransform area;
    [SerializeField] private RectTransform handle;

    private Canvas canvas;

    private Camera UICamera {
        get {
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
                return canvas.worldCamera;
            }

            return Camera.main;
        }
    }

    private float AreaRadius => Mathf.Min(area.rect.width, area.rect.height) / 2f;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (area == null)
            Debug.LogError("Area is not valid!");
        if (handle == null)
            Debug.LogError("Handle is not valid!");
        if (canvas == null)
            Debug.LogError("Canvas is not valid!");
    }

    private void OnEnable()
    {
        MoveController.OnMoveInputChanged += HandleMoveInputChanged;
    }

    private void OnDisable()
    {
        MoveController.OnMoveInputChanged -= HandleMoveInputChanged;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ApplyJoystickInput(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MoveController.SetMoveInput(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ApplyJoystickInput(eventData);
    }

    private void ApplyJoystickInput(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, eventData.position, null, out Vector2 localPoint)) {
            var clampedPosition = Vector2.ClampMagnitude(localPoint, AreaRadius);
            var input = AreaRadius > 0f ? clampedPosition / AreaRadius : Vector2.zero;

            MoveController.SetMoveInput(input);
        }
    }

    private void UpdateHandlePosition()
    {
        handle.localPosition = MoveController.MoveInput * AreaRadius;
    }

    private void HandleMoveInputChanged(Vector2 moveInput)
    {
        UpdateHandlePosition();
    }
}