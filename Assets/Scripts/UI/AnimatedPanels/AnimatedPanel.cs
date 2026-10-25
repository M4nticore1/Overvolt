using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AnimatedPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    public RectTransform RectTransform => rectTransform != null ? rectTransform : GetComponent<RectTransform>();

    [Header("Animation")]
    [SerializeField] private Button backgroundCloseButton;

    [SerializeField] private float animationSpeed = 10f;
    public float AnimationSpeed => animationSpeed;

    [SerializeField] private RectTransform contentRoot;
    public RectTransform ContentRoot => contentRoot;

    [SerializeField] private bool hideWhenClosed = false;
    public bool HideWhenClosed => hideWhenClosed;

    [SerializeField] private bool isShown = false;
    public bool IsShown => isShown;

    public bool IsUnderAnimation => IsShown ? animationProgress < 1f : animationProgress > 0f;

    private float animationProgress = 0f;

    public event Action OnShown;
    public event Action OnHidden;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        if (backgroundCloseButton != null) {
            backgroundCloseButton.onClick.AddListener(HandleCloseButtonClicked);
        }
    }

    protected virtual void OnDisable()
    {
        if (backgroundCloseButton != null) {
            backgroundCloseButton.onClick.RemoveListener(HandleCloseButtonClicked);
        }
    }

    protected virtual void Start()
    {
        StartCoroutine(UpdateContentRootEnabledEndOfFrame());
    }

    private void Update()
    {
        var targetProgress = isShown ? 1f : 0f;

        if (!Mathf.Approximately(animationProgress, targetProgress)) {
            animationProgress = Mathf.Lerp(animationProgress, targetProgress, animationSpeed * Time.deltaTime);

            if (Mathf.Abs(animationProgress - targetProgress) < 0.001f) {
                animationProgress = targetProgress;
            }

            SetAnimationProgress(animationProgress);
            UpdateContentRootEnabled();
        }
    }

    public abstract void SetAnimationProgress(float progress);

    protected abstract void HandleShown();

    protected abstract void HandleHidden();

    public void Show()
    {
        isShown = true;
        UpdateContentRootEnabled();
        HandleShown();

        OnShown?.Invoke();
    }

    public void Hide()
    {
        isShown = false;
        HandleHidden();

        OnHidden?.Invoke();
    }

    private void UpdateContentRootEnabled()
    {
        SetContentRootEnabled(ShouldSetContentActive());
    }

    private void SetContentRootEnabled(bool value)
    {
        contentRoot.gameObject.SetActive(value);
    }

    private void HandleCloseButtonClicked()
    {
        Hide();
    }

    private bool ShouldSetContentActive()
    {
        if (isShown) return true;
        if (IsUnderAnimation) return true;

        return false;
    }

    private IEnumerator UpdateContentRootEnabledEndOfFrame()
    {
        yield return null;

        UpdateContentRootEnabled();
    }
}