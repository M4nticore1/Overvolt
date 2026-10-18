using UnityEngine;
using UnityEngine.UI;

public class HeadquaterEnergyDisplay : MonoBehaviour
{
    private HeadquaterEnergyController HeadquaterEnergy => HeadquaterEnergyController.Instance;

    [SerializeField] private Image energyBar;

    private void OnEnable()
    {
        HeadquaterEnergy.OnCurrentEnergyChanged += HandleEnergyChanged;
    }

    private void OnDisable()
    {
        HeadquaterEnergy.OnCurrentEnergyChanged -= HandleEnergyChanged;
    }

    private void UpdateEnergyBarAlpha()
    {
        energyBar.fillAmount = HeadquaterEnergy.CurrentEnergyAlpha;
    }

    private void HandleEnergyChanged(float energy)
    {
        UpdateEnergyBarAlpha();
    }
}