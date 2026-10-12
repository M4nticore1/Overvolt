using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float speed = 3f;

    private ScaleAnimationManager Manager => ScaleAnimationManager.Instance;

    private void OnEnable()
    {
        if (Manager != null) {
            Manager.RegisterEffect(this);
        }
    }

    private void OnDisable()
    {
        if (Manager != null) {
            Manager.UnregisterEffect(this);
        }
    }

    public void Tick()
    {
        var t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        var scale = Mathf.Lerp(minScale, maxScale, t);
        var scaleVector = Vector3.one * scale;

        transform.localScale = scaleVector;
    }
}