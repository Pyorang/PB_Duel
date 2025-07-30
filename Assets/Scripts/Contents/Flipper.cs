using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flipper : MonoBehaviour
{
    [Header("왼쪽 플립퍼 세트 오브젝트들")]
    [Space]
    [SerializeField] private GameObject _lLFlipper;
    [SerializeField] private GameObject _lRFlipper;

    [Header("오른쪽 플립퍼 세트 오브젝트들")]
    [SerializeField] private GameObject _rLFlipper;
    [SerializeField] private GameObject _rRFlipper;

    [Header("플리퍼 회전 관련 설정들")]
    [Space]
    [SerializeField] private float _targetAngle = 60f;
    [SerializeField] private float _rotationSpeed = 90f;

    private static readonly float _defaultFlipperSize = 1f;
    private bool _playerControlEnabled = true;
    public bool _playerControlReversed = false;

    private bool _isLLeftPressed = false;
    private bool _isLRightPressed = false;
    private bool _isRLeftPressed = false;
    private bool _isRRightPressed = false;

    private InGame_Control _playerInputActions;

    private void OnEnable()
    {
        EnableInputActions();
    }

    private void OnDisable()
    {
        DisableInputActions();
    }

    private void Update()
    {
        if (_playerControlEnabled)
        {
            float lLeftTarget = _isLLeftPressed ? -_targetAngle : 0;
            if (_playerControlReversed) lLeftTarget = -lLeftTarget;
            Quaternion lLeftRotation = Quaternion.Euler(_lLFlipper.transform.localEulerAngles.x, lLeftTarget, _lLFlipper.transform.localEulerAngles.z);
            float lRightTarget = _isLRightPressed ? _targetAngle : 0;
            if (_playerControlReversed) lRightTarget = -lRightTarget;
            Quaternion lRightRotation = Quaternion.Euler(_lRFlipper.transform.localEulerAngles.x, lRightTarget, _lRFlipper.transform.localEulerAngles.z);
            float rLeftTarget = _isRLeftPressed ? -_targetAngle : 0;
            if (_playerControlReversed) rLeftTarget = -rLeftTarget;
            Quaternion rLefttRotation = Quaternion.Euler(_rLFlipper.transform.localEulerAngles.x, rLeftTarget, _rLFlipper.transform.localEulerAngles.z);
            float rRightTarget = _isRRightPressed ? _targetAngle : 0;
            if (_playerControlReversed) rRightTarget = -rRightTarget;
            Quaternion rRightRotation = Quaternion.Euler(_rRFlipper.transform.localEulerAngles.x, rRightTarget, _rRFlipper.transform.localEulerAngles.z);

            if (!_playerControlReversed) _lLFlipper.transform.localRotation = Quaternion.RotateTowards(_lLFlipper.transform.localRotation, lLeftRotation, _rotationSpeed * Time.deltaTime);
            else _lRFlipper.transform.localRotation = Quaternion.RotateTowards(_lRFlipper.transform.localRotation, lLeftRotation, _rotationSpeed * Time.deltaTime);

            if (!_playerControlReversed) _lRFlipper.transform.localRotation = Quaternion.RotateTowards(_lRFlipper.transform.localRotation, lRightRotation, _rotationSpeed * Time.deltaTime);
            else _lLFlipper.transform.localRotation = Quaternion.RotateTowards(_lLFlipper.transform.localRotation, lRightRotation, _rotationSpeed * Time.deltaTime);

            if (!_playerControlReversed) _rLFlipper.transform.localRotation = Quaternion.RotateTowards(_rLFlipper.transform.localRotation, rLefttRotation, _rotationSpeed * Time.deltaTime);
            else _rRFlipper.transform.localRotation = Quaternion.RotateTowards(_rRFlipper.transform.localRotation, rLefttRotation, _rotationSpeed * Time.deltaTime);

            if (!_playerControlReversed) _rRFlipper.transform.localRotation = Quaternion.RotateTowards(_rRFlipper.transform.localRotation, rRightRotation, _rotationSpeed * Time.deltaTime);
            else _rLFlipper.transform.localRotation = Quaternion.RotateTowards(_rLFlipper.transform.localRotation, rRightRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    public IEnumerator ChangeFlipperSize(float time, float multiplier)
    {
        _lLFlipper.transform.localScale = new Vector3(multiplier, _defaultFlipperSize, _defaultFlipperSize);
        _lRFlipper.transform.localScale = new Vector3(multiplier, _defaultFlipperSize, _defaultFlipperSize);
        _rLFlipper.transform.localScale = new Vector3(multiplier, _defaultFlipperSize, _defaultFlipperSize);
        _rRFlipper.transform.localScale = new Vector3(multiplier, _defaultFlipperSize, _defaultFlipperSize);

        yield return new WaitForSeconds(time);

        _lLFlipper.transform.localScale = new Vector3(_defaultFlipperSize, _defaultFlipperSize, _defaultFlipperSize);
        _lRFlipper.transform.localScale = new Vector3(_defaultFlipperSize, _defaultFlipperSize, _defaultFlipperSize);
        _rLFlipper.transform.localScale = new Vector3(_defaultFlipperSize, _defaultFlipperSize, _defaultFlipperSize);
        _rRFlipper.transform.localScale = new Vector3(_defaultFlipperSize, _defaultFlipperSize, _defaultFlipperSize);
    }

    private void EnableInputActions()
    {
        _playerInputActions = new InGame_Control();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.LLeftFlipper.performed += ctx => _isLLeftPressed = true;
        _playerInputActions.Player.LRightFlipper.performed += ctx => _isLRightPressed = true;
        _playerInputActions.Player.RLeftFlipper.performed += ctx => _isRLeftPressed = true;
        _playerInputActions.Player.RRightFlipper.performed += ctx => _isRRightPressed = true;
        _playerInputActions.Player.LLeftFlipper.canceled += ctx => _isLLeftPressed = false;
        _playerInputActions.Player.LRightFlipper.canceled += ctx => _isLRightPressed = false;
        _playerInputActions.Player.RLeftFlipper.canceled += ctx => _isRLeftPressed = false;
        _playerInputActions.Player.RRightFlipper.canceled += ctx => _isRRightPressed = false;
    }

    private void DisableInputActions()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.Player.LLeftFlipper.performed -= ctx => _isLLeftPressed = true;
        _playerInputActions.Player.LRightFlipper.performed -= ctx => _isLRightPressed = true;
        _playerInputActions.Player.RLeftFlipper.performed -= ctx => _isRLeftPressed = true;
        _playerInputActions.Player.RRightFlipper.performed -= ctx => _isRRightPressed = true;
        _playerInputActions.Player.LLeftFlipper.canceled -= ctx => _isLLeftPressed = false;
        _playerInputActions.Player.LRightFlipper.canceled -= ctx => _isLRightPressed = false;
        _playerInputActions.Player.RLeftFlipper.canceled -= ctx => _isRLeftPressed = false;
        _playerInputActions.Player.RRightFlipper.canceled -= ctx => _isRRightPressed = false;
    }
}
