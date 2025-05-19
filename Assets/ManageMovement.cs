using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ManageMovement : MonoBehaviour
{
    [SerializeField] private Transform _ship;
    [SerializeField] private Transform _hand;
    [SerializeField] private float _rotationSensitivity= 10f;
    [SerializeField] private float _scaleFactor = 20f;
    private float _maxDistance;
    private Vector3 _initialHandPosition;
    private Vector3 _initialShipPosition;
    private Quaternion _initialShipRotation;
    private bool _isHandInVolume = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable _button;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor _directInteractor;
    private bool _isButtonActivated = false;

    private void Awake()
    {
        _button = GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        _directInteractor = _hand.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();
        _maxDistance = this.gameObject.GetComponent<SphereCollider>().radius;
        _initialShipPosition = _ship.position;
        _initialShipRotation = _ship.rotation;
    }

    private void Start()
    {
        _button.selectEntered.AddListener(ButtonActivated);
    }

    private void Update()
    {
        if (_isButtonActivated)
        {
            NavigateShip();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _hand)
        {
            _isHandInVolume = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _hand)
        {
            _isHandInVolume = false;
            if (_isButtonActivated)
            {
                _isButtonActivated = false;
                _button.gameObject.SetActive(true);
            }
            _ship.position = _initialShipPosition;
            _ship.rotation = _initialShipRotation;
        }
    }

    public void ButtonActivated(SelectEnterEventArgs arg0)
    {
        if (_isHandInVolume && arg0.interactorObject == _directInteractor)
        {
            _isButtonActivated = true;
            _button.gameObject.SetActive(false);
            _initialHandPosition = _button.transform.position;
        }
    }

    private void NavigateShip()
    {
        Vector3 _handOffset = _hand.position - _initialHandPosition;
        Vector3 _newShipPosition =_initialShipPosition + _handOffset*_scaleFactor;
        if (_handOffset.magnitude <= _maxDistance)
        {
            _ship.position = _newShipPosition;
        }
       Vector3 _rotation = _hand.rotation.eulerAngles;
       _ship.rotation = Quaternion.Euler(_rotation.x+_rotationSensitivity,_rotation.y*_rotationSensitivity, (_rotation.z-90f)*_rotationSensitivity);
    }
}
