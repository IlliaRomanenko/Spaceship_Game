
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ManageMove : MonoBehaviour
{
    [SerializeField] private Transform _ship;
    [SerializeField] private Transform _hand;
    [SerializeField] private GameObject _object;
    [SerializeField] private float _rotationSensitivity = 10f;
    [SerializeField] private float _scaleVector = 20f;

    private float _maxDistance;
    private Vector3 _nintialHandPosition;
    private Vector3 _initialShipPosition;
    private Quaternion _initialShipRotation;
    private IXRInteractable _simpleInteractable;
    private IXRInteractor _baseInteractor;
    private bool _isActivated = false;
   
    private void Awake()
    {
        //_simpleInteractable = GetComponentInChildren<IXRInteractable>();
        //_baseInteractor = _hand.GetComponent<IXRInteractor>();
        _maxDistance = this.GetComponentInParent<SphereCollider>().radius;
        _initialShipPosition = _ship.position;
        _initialShipRotation = _ship.rotation;
    }

    private void Start()
    {
       
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
    }

    private void Navigate()
    {
        Vector3 handOffset = _hand.position - _nintialHandPosition;
        Vector3 _newShipPosition = _initialShipPosition + handOffset*_scaleVector;
        if (handOffset.magnitude <= _maxDistance)
        {
            _ship.position = _newShipPosition;
        }

        Vector3 _rotation = _hand.rotation.eulerAngles;
        _ship.rotation = Quaternion.Euler(_rotation.x +_rotationSensitivity, _rotation.y +_rotationSensitivity, (_rotation.z -90f) +_rotationSensitivity);
    }
}
