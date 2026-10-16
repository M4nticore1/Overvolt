using UnityEngine;

public interface IPlayerTarget
{
    public Transform transform { get; }
    public Transform PlayerTargetShootPoint { get; }
    public Transform PlayerTargetMarkerPoint { get; }
    public HealthComponent HealthComponent { get; }
}