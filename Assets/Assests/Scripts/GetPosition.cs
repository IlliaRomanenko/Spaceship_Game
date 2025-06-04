using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class GetPosition : MonoBehaviour
{

    private Collider _collider;
    private GameObject _smallSphere;

    public static event Action OnPositionGot;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _smallSphere = GetComponentInChildren<GameObject>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        OnPositionGot?.Invoke();
        _smallSphere.SetActive(false);
        
        
    }
      private void OnTriggerExit(Collider other)
    {
       // transform.gameObject.SetActive(true);
    }

}
