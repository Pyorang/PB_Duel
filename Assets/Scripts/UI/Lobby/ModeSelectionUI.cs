using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * ModeSelectionUI 프리팹에 붙는 스크립트
 * Mode Selection의 상호작용 UI 기능을 정의한 클래스
 */

public class ModeSelectionUI : BaseUI
{
    [SerializeField]
    private Button _hostButton, _joinButton, _backButton;
    [SerializeField]
    private TMP_InputField _inputField;

    private void Start()
    {
        _hostButton.onClick.AddListener(OnClickHostButton);
        _joinButton.onClick.AddListener(OnClickJoinButton);
        _backButton.onClick.AddListener(OnClickCloseButton);

    }

    public string GetHostName()
    {
        return _inputField.text;
    }

    public void OnClickHostButton()
    {
        UIManager.Instance.OpenUI<HostSessionUI>();
    }

    public void OnClickJoinButton()
    {
        UIManager.Instance.OpenUI<JoinSessionUI>();
        MatchMaker.Instance.TryJoinLobby();
    }
}
