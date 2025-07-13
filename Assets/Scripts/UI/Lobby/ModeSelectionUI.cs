using UnityEngine;
using UnityEngine.UI;

/*
 * Mode Selection의 상호작용 UI 기능을 정의한 클래스
 */

public class ModeSelectionUI : BaseUI
{
    [SerializeField]
    private Button hostButton, joinButton, backButton;

    private void Start()
    {
        hostButton.onClick.AddListener(OnClickHostButton);
        joinButton.onClick.AddListener(OnClickJoinButton);
        backButton.onClick.AddListener(OnClickCloseButton);
    }

    public void OnClickHostButton()
    {
        UIManager.Instance.OpenUI<HostSessionUI>();
    }

    public void OnClickJoinButton()
    {
        UIManager.Instance.OpenUI<JoinSessionUI>();
        //MatchMaker.Instance.TryJoinLobby();
    }
}
