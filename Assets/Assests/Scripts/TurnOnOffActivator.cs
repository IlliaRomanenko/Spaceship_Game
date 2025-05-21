using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TurnOnOffActivator : MonoBehaviour
{
   [SerializeField] private GameObject _activator;
   [SerializeField] private Transform _fingerTip;
   [SerializeField] private Transform _ship;
   [SerializeField] private Transform _hand;
   [SerializeField] private GameObject _object;
   [SerializeField] private Transform _rig;
   [SerializeField] private float _rotationSensitivity = 10f;
   [SerializeField] private float _scaleVector = 20f;
   [SerializeField] private Transform _position;

   private float _maxDistance;
   private Vector3 _nintialHandPosition;
   private Vector3 _initialShipPosition;
   private Quaternion _initialShipRotation;
   private Quaternion _initialHandRotation;
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

  

   void OnTriggerEnter(Collider other)
   {
      if (other.transform.IsChildOf(_fingerTip))
      {
         if (_activator != null)
         {
            _activator.SetActive(true);
         }
      }
   }
   
   void OnTriggerExit(Collider other)
   {
      if (other.transform.IsChildOf(_fingerTip))
      {
         _isActivated = false;
         if (_activator != null)
         {
            _activator.SetActive(false);
         }
         _object.SetActive(true);
         _activator.SetActive(false);
         _ship.rotation = _initialShipRotation;
         _ship.position = _initialShipPosition;
      }
   }
   
   private void Update()
   {
      if (_isActivated)
      {
         Navigate();
      }
   }

   public void Activated()
   {
         _isActivated = true;
         _object.SetActive(false);
         _nintialHandPosition = transform.position;
         _initialHandRotation = _hand.rotation;

   }

   private void Navigate()
   {
      //Vector3 handOffset = _hand.position - _nintialHandPosition;
     //Vector3 newShipPosition = _initialShipPosition + handOffset*_scaleVector;
    //  if (handOffset.magnitude <= _maxDistance)
     // {
    //     _ship.position = newShipPosition;
    //  }
    
     Vector3 _rotation = _hand.rotation.eulerAngles;
     _ship.rotation = Quaternion.Euler(_rotation.x +_rotationSensitivity, _rotation.y +_rotationSensitivity, _rotation.z  +_rotationSensitivity);
     //_rig.position = _position.position;
     //_rig.rotation = _position.rotation;
     //Vector3 _camRotation = _hand.rotation.eulerAngles;
     // _camera.rotation = Quaternion.Euler(_camRotation.x +_rotationSensitivity, _camRotation.y +_rotationSensitivity, _camRotation.z +_rotationSensitivity);

     // Quaternion handDelta = _hand.rotation * Quaternion.Inverse(_initialHandRotation);

     //Vector3 _rigPosition = _rig.position;
     //_ship.rotation = handDelta * _initialShipRotation;
     //_rig.rotation = _ship.rotation;
     //_rig.position = _rigPosition;
   }
}
