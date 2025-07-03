using UnityEngine;
using TMPro;

public class NameInputField : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI name;

    // 이름 입력을 마치면 저장
    public void OnEndEditEvent(string str)
    {
        // UserDataManager에다가 이름 저장
        Debug.Log("NameInputField.OnEndEditEvent: " + name.text);
    }
}
