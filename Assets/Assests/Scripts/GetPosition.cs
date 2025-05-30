using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class GetPosition : MonoBehaviour
{

    private Collider _collider;

    public static event Action OnPositionGot;
    private void Awake()
    {
        _collider = GetComponent<Collider>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        OnPositionGot?.Invoke();
        
    }
      private void OnTriggerExit(Collider other)
    {
        transform.gameObject.SetActive(true);
    }

}
