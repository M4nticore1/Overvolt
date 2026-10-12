using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerTarget
{
    [Header("IPlayerTarget")]
    [SerializeField] private Transform playerTargetMarkerTransform;
    public Transform PlayerTargetMarkerTransform => playerTargetMarkerTransform;

    //public void Tick()
    //{

    //}
}