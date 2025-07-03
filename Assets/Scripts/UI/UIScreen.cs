using UnityEngine;

/*
 * 로비씬에서 사용되는 UI 스크린
 */

public class UIScreen : MonoBehaviour
{
    // 다음 UIScreen 띄우기
    public void ShowScreen(UIScreen screen)
    {
        gameObject.SetActive(false);
        screen.gameObject.SetActive(true);
    }
}
