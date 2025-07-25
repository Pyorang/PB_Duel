using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}

public class Pin_Rotating : MonoBehaviour
{
    // 회전할 대상 오브젝트
    [SerializeField] private Transform _target;

    // 회전 속도 (도/초)
    [SerializeField] private float _rotationSpeed = 90f;

    // 회전 방향 (시계 방향이면 true)
    [SerializeField] private bool _isClockwise = true;

    // 회전할 축
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.Z;

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        float direction = _isClockwise ? -1f : 1f;
        float angle = direction * _rotationSpeed * Time.deltaTime;

        RotateTarget(angle);
    }

    private void RotateTarget(float angle)
    {
        switch (_rotationAxis)
        {
            case RotationAxis.X:
                _target.Rotate(angle, 0f, 0f);
                break;

            case RotationAxis.Y:
                _target.Rotate(0f, angle, 0f);
                break;

            case RotationAxis.Z:
                _target.Rotate(0f, 0f, angle);
                break;
        }
    }
}
