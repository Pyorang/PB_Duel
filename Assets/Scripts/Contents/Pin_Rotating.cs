using UnityEngine;

public class Pin_Rotating : MonoBehaviour
{
    // 회전할 대상 오브젝트
    [SerializeField]
    private Transform _target;

    // 회전 속도 (도/초)
    [SerializeField]
    private float _rotationSpeed = 90f;

    // 회전 방향 (시계 방향이면 true)
    [SerializeField]
    private bool _isClockwise = true;

    // 회전 방향 조절기 활성화 상태 여부
    [SerializeField]
    private bool _isDirectionRegulatorActive = false;

    private void Start()
    {
        StartCoroutine(RotateCoroutine());
    }

    private System.Collections.IEnumerator RotateCoroutine()
    {
        while (true)
        {
            if (_target != null)
            {
                bool isClockwiseNow;

                if (_isDirectionRegulatorActive)
                {
                    isClockwiseNow = !_isClockwise;
                }
                else
                {
                    isClockwiseNow = _isClockwise;
                }

                float direction = isClockwiseNow ? -1f : 1f;
                float angle = direction * _rotationSpeed * Time.deltaTime;

                _target.Rotate(0f, 0f, angle);
            }

            yield return null;
        }
    }
}
