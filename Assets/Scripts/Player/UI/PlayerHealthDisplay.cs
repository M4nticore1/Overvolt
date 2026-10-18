using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    private PlayerCharacter Player => PlayerCharacter.Instance;

    [SerializeField] private HealthDisplay healthDisplay;

    private void Awake()
    {
        healthDisplay.SetHealthComponent(Player.HealthComponent);
    }
}