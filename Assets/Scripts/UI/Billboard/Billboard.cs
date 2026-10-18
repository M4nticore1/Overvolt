using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool invertRotation = false;
    [SerializeField] private bool useParentRotation = false;

    private Camera Cam => Camera.main;
    private BillboardsManager billboardsManager => BillboardsManager.Instance;

    private void OnEnable()
    {
        billboardsManager.Register(this);
    }

    private void OnDisable()
    {
        billboardsManager.Unregister(this);
    }

    public void Tick()
    {
        transform.LookAt(Cam.transform.position, Cam.transform.up);

        if (invertRotation) {
            transform.Rotate(0f, 180f, 0f);
        }

        if (useParentRotation && transform.parent != null) {
            transform.rotation *= transform.parent.rotation;
        }
    }
}