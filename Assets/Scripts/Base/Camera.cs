using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CharacterController))]
public class FreeFlyCam : MonoBehaviour
{
    [SerializeField] private bool _active = true;
    [SerializeField] private bool _enableRotation = true;
    [SerializeField] private float _mouseSense = 1.8f;
    [SerializeField] private bool _enableTranslation = true;
    [SerializeField] private float _translationSpeed = 55f;
    [SerializeField] private bool _enableMovement = true;
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _boostedSpeed = 50f;
    [SerializeField] private KeyCode _boostSpeed = KeyCode.LeftShift;
    [SerializeField] private KeyCode _moveUp = KeyCode.E;
    [SerializeField] private KeyCode _moveDown = KeyCode.Q;
    [SerializeField] private bool _enableSpeedAcceleration = true;
    [SerializeField] private float _speedAccelerationFactor = 1.5f;
    [SerializeField] private KeyCode _initPositonButton = KeyCode.R;

    private CharacterController _characterController;
    private CursorLockMode _wantedMode;
    private float _currentIncrease = 1;
    private float _currentIncreaseMem = 0;
    private Vector3 _initPosition;
    private Vector3 _initRotation;

    private void Start()
    {
        _initPosition = transform.position;
        _initRotation = transform.eulerAngles;
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (_active)
            _wantedMode = CursorLockMode.Locked;
    }

    private void SetCursorState()
    {
        if (Input.GetMouseButton(1))
        {
            _wantedMode = CursorLockMode.Locked;
        }
        else
        {
            _wantedMode = CursorLockMode.None;
        }

        Cursor.lockState = _wantedMode;
        Cursor.visible = (CursorLockMode.Locked != _wantedMode);
    }

    private void CalculateCurrentIncrease(bool moving)
    {
        _currentIncrease = Time.deltaTime;

        if (!_enableSpeedAcceleration || _enableSpeedAcceleration && !moving)
        {
            _currentIncreaseMem = 0;
            return;
        }

        _currentIncreaseMem += Time.deltaTime * (_speedAccelerationFactor - 1);
        _currentIncrease = Time.deltaTime + Mathf.Pow(_currentIncreaseMem, 3) * Time.deltaTime;
    }

    private void Update()
    {
        if (!_active)
            return;

        SetCursorState();

        if (_enableTranslation)
        {
            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * _translationSpeed);
        }

        if (_enableMovement)
        {
            Vector3 deltaPosition = Vector3.zero;
            float currentSpeed = _movementSpeed;

            if (Input.GetKey(_boostSpeed))
                currentSpeed = _boostedSpeed;

            if (Input.GetKey(KeyCode.W))
                deltaPosition += transform.forward;

            if (Input.GetKey(KeyCode.S))
                deltaPosition -= transform.forward;

            if (Input.GetKey(KeyCode.A))
                deltaPosition -= transform.right;

            if (Input.GetKey(KeyCode.D))
                deltaPosition += transform.right;

            if (Input.GetKey(_moveUp))
                deltaPosition += transform.up;

            if (Input.GetKey(_moveDown))
                deltaPosition -= transform.up;

            CalculateCurrentIncrease(deltaPosition != Vector3.zero);

            _characterController.Move(deltaPosition * currentSpeed * _currentIncrease);
        }

        if (Cursor.visible)
            return;

        if (_enableRotation)
        {
            transform.rotation *= Quaternion.AngleAxis(
                -Input.GetAxis("Mouse Y") * _mouseSense,
                Vector3.right
            );

            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
                transform.eulerAngles.z
            );
        }

        if (Input.GetKeyDown(_initPositonButton))
        {
            transform.position = _initPosition;
            transform.eulerAngles = _initRotation;
        }
    }
}