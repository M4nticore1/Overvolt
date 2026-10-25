using System;
using UnityEngine;

public class HeadquaterEnergyController : MonoBehaviour
{
    private static HeadquaterEnergyController instance;
    public static HeadquaterEnergyController Instance => instance ??= FindFirstObjectByType<HeadquaterEnergyController>();

    [SerializeField] private float generateEnergyPerSecond = 1f;
    public float GenerateEnergyPerSecond => generateEnergyPerSecond;

    [SerializeField] private float maxEnergy = 100f;
    public float MaxEnergy => maxEnergy;

    public float CurrentEnergy { get; private set; }
    public float CurrentEnergyAlpha => maxEnergy > 0 ? CurrentEnergy / maxEnergy : 1f;

    public event Action<float> OnCurrentEnergyChanged;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (CurrentEnergy < maxEnergy) {
            SetCurrentEnergy(CurrentEnergy + generateEnergyPerSecond * Time.deltaTime);
        }
    }

    public void SetCurrentEnergy(float energy)
    {
        CurrentEnergy = Mathf.Clamp(energy, 0, maxEnergy);

        OnCurrentEnergyChanged?.Invoke(energy);
    }
}