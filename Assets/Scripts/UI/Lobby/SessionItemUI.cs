using UnityEngine;
using TMPro;
using NanoSockets;
using UnityEngine.UI;

/*
 * SessionItemUI 프리팹에 붙는 스크립트
 */

public class SessionItemUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _sessionName;
    [SerializeField]
    private TextMeshProUGUI _hostName;
    [SerializeField]
    private Toggle _isPrivate;
    [SerializeField]
    private Button _isOpen;

    // MatchMaker.cs에서 방 리스트 초기화할 때 호출되는 함수
    public void Init(string p_sessionName, string p_hostName, bool p_isPrivate, bool p_isOpen)
    {
        gameObject.SetActive(true);

        _sessionName.text = p_sessionName;
        _hostName.text = p_hostName;
        _isPrivate.isOn = p_isPrivate;
        _isOpen.interactable = p_isOpen;
    }

    public void Disable()
    {
        _sessionName = null;
        _hostName = null;
        _isPrivate.isOn = false;
        _isOpen.interactable = false;

        gameObject.SetActive(false);
    }

    // TODO: InSession UI 머지되면 작성하기
    public void OnJoinButton()
    {

    }
}
