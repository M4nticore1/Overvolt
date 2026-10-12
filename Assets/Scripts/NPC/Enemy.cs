using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerTarget
{
    // IPlayerTarget
    [SerializeField] private Transform playerTargetMarkerTransform;
    public Transform PlayerTargetMarkerTransform => playerTargetMarkerTransform;

    //public void Tick()
    //{

    //}
}