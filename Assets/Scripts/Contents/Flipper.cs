using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Unity.VisualScripting;

public class Flipper : NetworkBehaviour
{
    [Header("플리퍼 오브젝트들")]
    [SerializeField] private GameObject _lLFlipper;
    [SerializeField] private GameObject _lRFlipper;
    [SerializeField] private GameObject _rLFlipper;
    [SerializeField] private GameObject _rRFlipper;

    [Header("플리퍼 회전 설정")]
    [SerializeField] private float _targetAngle = 60f;
    [SerializeField] private float _rotationSpeed = 90f;

    private static readonly float _defaultFlipperSize = 1f;
    private bool _playerControlEnabled = true;
    private bool _playerControlReversed = false;

    private readonly bool[] _flipperPressed = new bool[4];
    private static readonly int[] _reversedMap = { 1, 0, 3, 2 };

    private InGame_Control _playerInput;

    private void OnEnable() => EnableInputActions();
    private void OnDisable() => DisableInputActions();

    private void Update()
    {
        if (!_playerControlEnabled) return;

        GameObject[] flippers = { _lLFlipper, _lRFlipper, _rLFlipper, _rRFlipper };

        for (int i = 0; i < 4; i++)
        {
            int index = GetFlipperIndex(i);
            float target = _flipperPressed[i] ? _targetAngle * ((i % 2 == 0) ? -1 : 1) : 0f;
            if (_playerControlReversed) target *= -1;

            Quaternion rotation = Quaternion.Euler(flippers[index].transform.localEulerAngles.x, target, flippers[index].transform.localEulerAngles.z);
            flippers[index].transform.localRotation = Quaternion.RotateTowards(flippers[index].transform.localRotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private int GetFlipperIndex(int i) => _playerControlReversed ? _reversedMap[i] : i;

    public void ReverseFlipperControl() { _playerControlReversed = !_playerControlReversed; }

    // NOTE : 나중에 플리퍼 사이즈 바뀌는 이펙트 넣을꺼면 여기서 수정해야 함.
    public IEnumerator RPC_ChangeFlipperSize(float time, float multiplier)
    {
        GameObject[] flippers = { _lLFlipper, _lRFlipper, _rLFlipper, _rRFlipper };
        Vector3 enlarged = new Vector3(multiplier, _defaultFlipperSize, _defaultFlipperSize);
        Vector3 normal = Vector3.one * _defaultFlipperSize;

        foreach (var flipper in flippers)
            flipper.transform.localScale = enlarged;

        yield return new WaitForSeconds(time);

        foreach (var flipper in flippers)
            flipper.transform.localScale = normal;
    }

    #region Input System 사용 관련 함수
    private void EnableInputActions()
    {
        _playerInput = new InGame_Control();
        _playerInput.Player.Enable();

        _playerInput.Player.LLeftFlipper.performed += ctx => _flipperPressed[0] = true;
        _playerInput.Player.LRightFlipper.performed += ctx => _flipperPressed[1] = true;
        _playerInput.Player.RLeftFlipper.performed += ctx => _flipperPressed[2] = true;
        _playerInput.Player.RRightFlipper.performed += ctx => _flipperPressed[3] = true;

        _playerInput.Player.LLeftFlipper.canceled += ctx => _flipperPressed[0] = false;
        _playerInput.Player.LRightFlipper.canceled += ctx => _flipperPressed[1] = false;
        _playerInput.Player.RLeftFlipper.canceled += ctx => _flipperPressed[2] = false;
        _playerInput.Player.RRightFlipper.canceled += ctx => _flipperPressed[3] = false;
    }

    private void DisableInputActions()
    {
        _playerInput.Player.Disable();

        _playerInput.Player.LLeftFlipper.performed -= ctx => _flipperPressed[0] = true;
        _playerInput.Player.LRightFlipper.performed -= ctx => _flipperPressed[1] = true;
        _playerInput.Player.RLeftFlipper.performed -= ctx => _flipperPressed[2] = true;
        _playerInput.Player.RRightFlipper.performed -= ctx => _flipperPressed[3] = true;

        _playerInput.Player.LLeftFlipper.canceled -= ctx => _flipperPressed[0] = false;
        _playerInput.Player.LRightFlipper.canceled -= ctx => _flipperPressed[1] = false;
        _playerInput.Player.RLeftFlipper.canceled -= ctx => _flipperPressed[2] = false;
        _playerInput.Player.RRightFlipper.canceled -= ctx => _flipperPressed[3] = false;
    }

    #endregion
}
