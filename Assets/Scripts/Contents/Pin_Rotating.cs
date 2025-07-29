using UnityEngine;

public class Pin_Rotating : MonoBehaviour
{
    [SerializeField]
    public bool IsClockwise = true;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _rotationSpeed = 90f;

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
                    isClockwiseNow = !IsClockwise;
                }
                else
                {
                    isClockwiseNow = IsClockwise;
                }

                float direction = isClockwiseNow ? -1f : 1f;
                float angle = direction * _rotationSpeed * Time.deltaTime;

                _target.Rotate(0f, 0f, angle);
            }

            yield return null;
        }
    }
}
