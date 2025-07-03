using UnityEngine;

/*
 * 로비씬에서 사용되는 UI 스크린 관련 클래스
 */

public class UIScreen : MonoBehaviour
{
    // 다음 UIScreen 띄우기
    // 다음 로비 UI로 넘어가는 버튼에 연결되는 함수
    public void ShowScreen(UIScreen screen)
    {
        gameObject.SetActive(false);
        screen.gameObject.SetActive(true);
    }
}
