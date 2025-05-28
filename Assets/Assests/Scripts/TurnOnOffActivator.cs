using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TurnOnOffActivator : MonoBehaviour
{
   [SerializeField] private GameObject _activator;
   [SerializeField] private Transform _fingerTip;
   [SerializeField] private Transform _ship;
   [SerializeField] private Transform _hand;
   //[SerializeField] private GameObject _object;
   [SerializeField] private float _rotationSensitivity = 10f;
   [SerializeField] private float _scaleFactor = 20f;
   //[SerializeField] private Transform _position;

   public static event Action OnHandInSphere;
   private float _maxDistance;
   private Vector3 _initialHandPosition;
   private Vector3 _initialShipPosition;
   private Quaternion _initialShipRotation;
   private bool _isHandInVolume = false;
   private XRGrabInteractable _simpleInteractable;
   //private IXRInteractor _baseInteractor;
   private bool _isActivated = false;
   
   private void Awake()
   {
      _simpleInteractable = GetComponentInChildren<XRGrabInteractable>();
      //_baseInteractor = _hand.GetComponent<IXRInteractor>();
      _maxDistance = GetComponent<SphereCollider>().radius;
      _initialShipPosition = _ship.position;
      _initialShipRotation = _ship.rotation;
   }

    private void Start()
    {
      GetPosition.OnPositionGot += Activated;
    }

   private void Update()
   {
      if (_isActivated)
      {
         Navigate();
      }
   }
    
    void OnTriggerEnter(Collider other)
   {
      if (other.transform.IsChildOf(_fingerTip))
      {
         if (_activator != null)
         {
            _isHandInVolume = true;
            _activator.SetActive(true);
           
            //
         }
      }
   }
   
   void OnTriggerExit(Collider other)
   {
      if (other.transform.IsChildOf(_fingerTip))
      {
         _isHandInVolume = false;
        
         if (_activator != null)
         {
            _isActivated = false;
            _activator.SetActive(false);
         }
         _ship.rotation = _initialShipRotation;
         _ship.position = _initialShipPosition;
      }
   }
   

   public void Activated()
   {
      if (_isHandInVolume)
      {
         _activator.SetActive(true);
          _isActivated = true;
           OnHandInSphere?.Invoke();
         //_object.SetActive(false);
         _initialHandPosition = _activator.transform.position;
      }
        

   }

   private void Navigate()
   {
      Vector3 _handOffset = _hand.position - _initialHandPosition;
      Vector3 _newShipPosition = _initialShipPosition + _handOffset * _scaleFactor;
      if (_handOffset.magnitude <= _maxDistance)
      {
         _ship.position = _newShipPosition;
      }
      Quaternion handRotation = _hand.rotation;
      Vector3 handEuler = handRotation.eulerAngles;
      float zRotation = handEuler.z;
      zRotation *= _rotationSensitivity;
       Vector3 currentShipEuler = _ship.rotation.eulerAngles;
        _ship.rotation = Quaternion.Euler(currentShipEuler.x, currentShipEuler.y, zRotation);


       //_ship.rotation = Quaternion.Euler(_rotation.x+_rotationSensitivity,_rotation.y*_rotationSensitivity, _rotation.z*_rotationSensitivity);
   }
}
