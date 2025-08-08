using UnityEngine;
using System.Collections;

public class Pin_Kicker : MonoBehaviour
{
    [SerializeField] private bool _isCoverActive = false;
    public bool IsCoverActive => _isCoverActive;

    [SerializeField] private float _kickForce = 10f;
    public float KickForce
    {
        get => _kickForce;
        set => _kickForce = value;
    }

    [SerializeField] private GameObject _coverObject;

    private bool _hasKicked = false;

    private void Start()
    {
        if (_coverObject != null)
        {
            _coverObject.SetActive(false);
        }

        _isCoverActive = false;
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (_hasKicked)
        {
            return;
        }

        if (p_other.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = p_other.attachedRigidbody;

            if (ballRigidbody != null)
            {
                StartCoroutine(KickSequence(ballRigidbody.gameObject));
            }

            _hasKicked = true;
        }
    }

    private IEnumerator KickSequence(GameObject p_ball)
    {
        yield return null;

        p_ball.SetActive(false);

        yield return new WaitForSeconds(1f);

        p_ball.transform.position = transform.position;
        p_ball.SetActive(true);

        Rigidbody ballRigidbody = p_ball.GetComponent<Rigidbody>();

        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.AddForce(Vector3.up * _kickForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.5f);

        if (_coverObject != null)
        {
            _coverObject.SetActive(true);
        }

        _isCoverActive = true;
    }
}
