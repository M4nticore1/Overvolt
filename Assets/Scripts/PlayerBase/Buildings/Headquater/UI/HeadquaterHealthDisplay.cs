using UnityEngine;

public class HeadquaterHealthDisplay : MonoBehaviour
{
    private Headquater Headquater => Headquater.Instance;

    [SerializeField] private HealthDisplay healthDisplay;

    private void Awake()
    {
        healthDisplay.SetHealthComponent(Headquater.HealthComponent);
    }
}