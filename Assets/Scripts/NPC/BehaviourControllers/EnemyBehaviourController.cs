using UnityEngine;

public abstract class EnemyBehaviourController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    protected Enemy Enemy => enemy;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    protected virtual void Subscribe()
    {

    }

    protected virtual void Unsubscribe()
    {

    }

    public abstract void Tick();
}