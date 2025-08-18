using UnityEngine;

/*
 * 임시로 화면 해상도를 설정하는 스크립트
 */

public class TempScreenSetting : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920/2, 1080/2, false);
    }
}
