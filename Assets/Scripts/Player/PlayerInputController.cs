using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;

    private InputActionMap playerInputActionMap;

    private InputAction moveIA;
    public InputAction MoveIA => moveIA ??= GetInputAction("Move");

    private InputAction sprintIA;
    public InputAction SprintIA => sprintIA ??= GetInputAction("Sprint");

    private InputAction interactIA;
    public InputAction InteractIA => interactIA ??= GetInputAction("Interact");

    private InputAction GetInputAction(string inputActionName)
    {
        if (inputActionAsset != null) {
            playerInputActionMap ??= inputActionAsset.FindActionMap("Player");

            if (playerInputActionMap != null) {
                var inputAction = playerInputActionMap.FindAction(inputActionName);

                if (inputAction == null) {
                    Debug.LogError($"Action '{inputActionName}' not found in {playerInputActionMap}!");
                }

                return inputAction;
            }
            else {
                Debug.LogError("PlayerInputActionMap is not valid!");
            }
        }
        else {
            Debug.LogError("InputActionAsset is not valid!");
        }

        return null;
    }
}