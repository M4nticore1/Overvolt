using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private HealthComponent healthComponent;

    [Header("Display")]
    [SerializeField] private GameObject content;
    [SerializeField] private Image bar;
    [SerializeField] private Gradient barGradient;

    [Header("Stats")]
    [SerializeField, Min(0f)] private float minHealthVisibilityThreshold = 0.5f;
    [SerializeField, Min(0f)] private float visibilityTime = 0f;

    private Coroutine hideCoroutine;
    private bool isDisplayed => content != null ? content.activeSelf && content.activeInHierarchy : true;
    private bool isSubscribed;

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        TryUnsubscribe();
        StopHideCoroutine();
    }

    private void Start()
    {
        UpdateDisplayed();
    }

    public void SetHealthComponent(HealthComponent health)
    {
        if (this.healthComponent == health)
            return;

        TryUnsubscribe();

        this.healthComponent = health;

        TrySubscribe();
        UpdateHealth();
    }

    public void RemoveHealthComponent()
    {
        TryUnsubscribe();

        healthComponent = null;
        Hide();
    }

    private void TrySubscribe()
    {
        if (isSubscribed)
            return;

        if (healthComponent == null)
            return;

        healthComponent.OnHealthChanged += OnHealthChanged;
        healthComponent.OnDied += OnDied;

        isSubscribed = true;
    }

    private void TryUnsubscribe()
    {
        if (!isSubscribed)
            return;

        if (healthComponent == null) {
            isSubscribed = false;
            return;
        }

        healthComponent.OnHealthChanged -= OnHealthChanged;
        healthComponent.OnDied -= OnDied;

        isSubscribed = false;
    }

    private void OnHealthChanged()
    {
        if (healthComponent == null)
            return;

        TryDisplay();
        UpdateHealth();
        UpdateHideCoroutine();
    }

    private void OnDied()
    {
        Hide();
    }

    private bool TryDisplay()
    {
        if (healthComponent == null)
            return false;

        if (isDisplayed)
            return false;

        if (GetHealthPercent() > minHealthVisibilityThreshold)
            return false;

        Display();

        return true;
    }

    public void Display()
    {
        if (content != null) {
            content.SetActive(true);
        }

        UpdateHealth();
    }

    public void Hide()
    {
        StopHideCoroutine();

        if (content != null) {
            content.SetActive(false);
        }
    }

    public void UpdateHealth()
    {
        if (!isDisplayed)
            return;

        UpdateHealthBar();
    }

    private void UpdateDisplayed()
    {
        if (ShouldDisplay()) {
            Display();
        }
        else {
            Hide();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthComponent == null)
            return;

        if (bar == null)
            return;

        var healthPercent = GetHealthPercent();
        bar.fillAmount = healthPercent;

        if (barGradient != null) {
            bar.color = barGradient.Evaluate(healthPercent);
        }
    }

    private void UpdateHideCoroutine()
    {
        StopHideCoroutine();

        if (visibilityTime <= 0f)
            return;

        hideCoroutine = StartCoroutine(HideDelayed());
    }

    private void StopHideCoroutine()
    {
        if (hideCoroutine == null)
            return;

        StopCoroutine(hideCoroutine);
        hideCoroutine = null;
    }

    private bool ShouldDisplay()
    {
        return GetHealthPercent() < minHealthVisibilityThreshold;
    }

    private float GetHealthPercent()
    {
        if (healthComponent == null)
            return 0f;

        if (healthComponent.MaxHealth <= 0f)
            return 0f;

        return Mathf.Clamp01(healthComponent.CurrentHealth / healthComponent.MaxHealth);
    }

    private IEnumerator HideDelayed()
    {
        yield return new WaitForSeconds(visibilityTime);

        hideCoroutine = null;
        Hide();
    }
}