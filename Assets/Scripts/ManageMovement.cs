using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ManageMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _ship;
    [SerializeField] private Transform _hand;
    [SerializeField] private InputActionReference _gripAction;

    [Header("Tuning")]
    [SerializeField] private float _rotationSensitivity = 10f;
    [SerializeField] private float _scaleFactor = 20f;

    private float _maxDistance;
    private Vector3 _initialHandPosition;
    private Vector3 _initialShipPosition;

    private bool _isGripHeld = false;
    private bool _isHandInsideZone = false;

    private void Awake()
    {
        _maxDistance = GetComponent<SphereCollider>().radius;

        _gripAction.action.performed += OnGripPressed;
        _gripAction.action.canceled += OnGripReleased;

        Debug.Log("ManageMovement initialized. Max distance: " + _maxDistance);
    }

    private void Update()
    {
        RotateShip(); 
    }

    private void OnDestroy()
    {
        _gripAction.action.performed -= OnGripPressed;
        _gripAction.action.canceled -= OnGripReleased;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(_hand))
        {
            _isHandInsideZone = true;
            Debug.Log("Hand entered the control zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.IsChildOf(_hand))
        {
            _isHandInsideZone = false;
            Debug.Log("Hand exited the control zone.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isGripHeld) return;

        Debug.Log("Grip held and hand confirmed in zone — controlling ship.");
        RotateShip();
        MoveShip();
    }

    private void OnGripPressed(InputAction.CallbackContext context)
    {
        _isGripHeld = true;
        _initialHandPosition = _hand.position;
        _initialShipPosition = _ship.position;
        Debug.Log("Grip pressed. Tracking starts at: " + _initialHandPosition);
    }

    private void OnGripReleased(InputAction.CallbackContext context)
    {
        _isGripHeld = false;
        Debug.Log("Grip released. Stopping control.");
    }

    private void MoveShip()
    {
        Vector3 handOffset = _hand.position - _initialHandPosition;

        if (handOffset.magnitude <= _maxDistance)
        {
            Vector3 newPos = _initialShipPosition + handOffset * _scaleFactor;
            _ship.position = newPos;
            Debug.Log("Ship moved to: " + newPos);
        }
        else
        {
            Debug.LogWarning("Movement blocked — hand moved too far (" + handOffset.magnitude + " > " + _maxDistance + ")");
        }
    }

    private void RotateShip()
    {
        Vector3 rot = _hand.rotation.eulerAngles;
        Quaternion newRot = Quaternion.Euler(
            rot.x + _rotationSensitivity,
            rot.y * _rotationSensitivity,
            (rot.z - 90f) * _rotationSensitivity
        );

        _ship.rotation = newRot;
        Debug.Log("Ship rotated to: " + newRot.eulerAngles);
    }
}
