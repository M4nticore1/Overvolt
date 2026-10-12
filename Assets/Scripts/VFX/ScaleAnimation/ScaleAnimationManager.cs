using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimationManager : MonoBehaviour
{
    private static ScaleAnimationManager instance;
    public static ScaleAnimationManager Instance => instance ??= FindFirstObjectByType<ScaleAnimationManager>();

    private List<ScaleAnimation> animations = new();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = animations.Count - 1; i >= 0; i--) {
            var effect = animations[i];
            if (effect == null) {
                animations.RemoveAt(i);
                continue;
            }

            effect.Tick();
        }
    }

    public void RegisterEffect(ScaleAnimation animation)
    {
        if (animation == null) return;
        if (animations.Contains(animation)) return;

        animations.Add(animation);
    }

    public void UnregisterEffect(ScaleAnimation animation)
    {
        animations.Remove(animation);
    }
}