using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ButtonStateTracker))]
public class ButtonScaleController : MonoBehaviour
{
    private ButtonScaleManager ScaleManager => ButtonScaleManager.Instance;

    private Button button;
    private ButtonStateTracker buttonStateTracker;

    private readonly float idleScale = 1f;
    [SerializeField] private float hoveredScale = 1.02f;
    [SerializeField] private float pressedScale = 0.98f;
    [SerializeField] private float disabledScale = 0.95f;

    [SerializeField] private float transitionTime = 0.2f;
    private float currentTransitionProgress = 0f;

    private Vector3 TargetScale {
        get {
            var state = buttonStateTracker != null ? buttonStateTracker.CurrentState : ButtonStateTracker.ButtonVisualState.Idle;
            float scale = idleScale;

            switch (state) {
                case ButtonStateTracker.ButtonVisualState.Idle:
                    scale = idleScale;
                    break;
                case ButtonStateTracker.ButtonVisualState.Hovered:
                    scale = hoveredScale;
                    break;
                case ButtonStateTracker.ButtonVisualState.Pressed:
                    scale = pressedScale;
                    break;
                case ButtonStateTracker.ButtonVisualState.Disabled:
                    scale = disabledScale;
                    break;
            }

            return Vector3.one * scale;
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonStateTracker = GetComponent<ButtonStateTracker>();

        if (button == null) {
            Debug.LogError($"Button is not valid at {this}!", this);
        }
        if (buttonStateTracker == null) {
            Debug.LogError($"ButtonStateTracker is not valid at {this}!", this);
        }
    }

    private void OnEnable()
    {
        if (ScaleManager != null) {
            ScaleManager.RegisterController(this);
        }

        if (buttonStateTracker != null) {
            buttonStateTracker.OnStateChanged += HandleButtonStateChanged;
        }
    }

    private void OnDisable()
    {
        if (ScaleManager != null) {
            ScaleManager.UnregisterController(this);
        }

        if (buttonStateTracker != null) {
            buttonStateTracker.OnStateChanged -= HandleButtonStateChanged;
        }
    }

    public void Tick()
    {
        if (transitionTime > 0f) {
            currentTransitionProgress = Mathf.MoveTowards(currentTransitionProgress, 1f, Time.deltaTime / transitionTime);
        }
        else {
            currentTransitionProgress = 1f;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, currentTransitionProgress);
    }

    private void HandleButtonStateChanged(ButtonStateTracker.ButtonVisualState state)
    {
        currentTransitionProgress = 0f;
    }
}