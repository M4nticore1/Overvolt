using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStateTracker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;

    public enum ButtonVisualState { Idle, Hovered, Pressed, Disabled }
    public ButtonVisualState CurrentState { get; private set; } = ButtonVisualState.Idle;

    public event Action<ButtonVisualState> OnStateChanged;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button == null) {
            Debug.LogError($"Button is not valid at {this}!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable && CurrentState != ButtonVisualState.Pressed) {
            CurrentState = ButtonVisualState.Hovered;
            OnStateChanged?.Invoke(CurrentState);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable && CurrentState != ButtonVisualState.Pressed) {
            CurrentState = ButtonVisualState.Idle;
            OnStateChanged?.Invoke(CurrentState);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable) {
            CurrentState = ButtonVisualState.Pressed;
            OnStateChanged?.Invoke(CurrentState);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable) {
            CurrentState = RectTransformUtility.RectangleContainsScreenPoint(button.transform as RectTransform, eventData.position, eventData.pressEventCamera) ? ButtonVisualState.Hovered : ButtonVisualState.Idle;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}