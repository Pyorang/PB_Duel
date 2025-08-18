using UnityEngine;

public class Pin_BoosterZone : MonoBehaviour
{
    public int speedMultiplier = 2;
    public Pin_Ball pinBall; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) 
        {
            if (pinBall != null)
            {
                Debug.Log("부스터존 진입");
                pinBall.MultiplyVelocity(speedMultiplier);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball")) 
        {
            if (pinBall != null)
            {
                Debug.Log("부스터존 ");
                pinBall.MultiplyVelocity(1f / speedMultiplier);
            }
        }
    }
}
