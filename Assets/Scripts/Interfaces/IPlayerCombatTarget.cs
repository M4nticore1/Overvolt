using UnityEngine;

public interface IPlayerCombatTarget
{
    public Transform transform { get; }
    public Transform PlayerTargetShootPoint { get; }
    public Transform PlayerTargetMarkerPoint { get; }
    public HealthComponent HealthComponent { get; }
}