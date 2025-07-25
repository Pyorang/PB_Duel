using UnityEngine;

public class Pin_RotatingBar : MonoBehaviour
{
    // 회전할 대상 오브젝트
    [SerializeField] private Transform _target;

    // 회전 속도 (도/초)
    [SerializeField] private float _rotationSpeed = 90f;

    // 회전 방향 (시계 방향이면 true)
    [SerializeField] private bool _isClockwise = true;


    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        float direction = _isClockwise ? -1f : 1f;
        float angle = direction * _rotationSpeed * Time.deltaTime;

        _target.Rotate(0f, 0f, angle);
    }
}
