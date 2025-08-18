using UnityEngine;
using System.Collections.Generic;

public class Pin_RotationRegulator : MonoBehaviour
{
    [SerializeField]
    private List<Pin_Rotating> _rotatingObjects;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ToggleRotationDirection();
        }
    }

    private void ToggleRotationDirection()
    {
        foreach (Pin_Rotating rotatingObject in _rotatingObjects)
        {
            rotatingObject.IsClockwise = !rotatingObject.IsClockwise;
        }
    }
}
