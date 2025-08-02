using UnityEngine;
using System.Collections;

public class Pin_Rotating : MonoBehaviour
{
    [SerializeField]
    public bool IsClockwise = true;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _rotationSpeed = 90f;

    private void Start()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            if (_target != null)
            {
                float direction = IsClockwise ? -1f : 1f;
                float angle = direction * _rotationSpeed * Time.deltaTime;

                _target.Rotate(0f, 0f, angle);
            }

            yield return null;
        }
    }
}
