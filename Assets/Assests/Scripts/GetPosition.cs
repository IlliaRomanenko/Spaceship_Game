using System;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    private Collider _collider;
    private GameObject _smallSphere;

    public static event Action OnPositionGot;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _smallSphere = GetComponentInChildren<MeshRenderer>(true)?.gameObject; // безопасное получение
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_smallSphere != null)
            _smallSphere.SetActive(false);

        OnPositionGot?.Invoke();
    }

    private void OnDestroy()
    {
        // Сбросим событие на null при удалении
        OnPositionGot = null;
    }
}