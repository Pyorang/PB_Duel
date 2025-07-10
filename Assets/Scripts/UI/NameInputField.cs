using UnityEngine;
using TMPro;

/*
 * Mode Selection에서 이름 입력을 받는 InputField 관련 클래스
 */

public class NameInputField : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI name;

    // 이름 입력을 마치면 저장
    public void OnEndEditEvent(string str)
    {
        // TODO: UserDataManager에다가 이름 저장
        Debug.Log("NameInputField.OnEndEditEvent: " + name.text);
    }
}
