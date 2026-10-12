using UnityEngine;

public interface IPlayerTarget
{
    public Transform transform { get; }
    public Transform PlayerTargetMarkerTransform { get; }
}