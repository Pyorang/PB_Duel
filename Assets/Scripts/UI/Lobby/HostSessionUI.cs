using UnityEngine;
using UnityEngine.UI;

/*
 * HostSessionUI 프리팹에 붙는 스크립트
 */

public class HostSessionUI : BaseUI
{
    [SerializeField]
    private Button _backButton, _createButton;

    private void Start()
    {
        _backButton.onClick.AddListener(OnClickCloseButton);
        _createButton.onClick.AddListener(gameObject.GetComponent<SessionSetup>().TryCreateSession);
        //createButton.onClick.AddListener(OnClickCreateButton);
    }

    // InSession UI 머지되면 작성하기
    public void OnClickCreateButton()
    {
        //UIManager.Instance.OpenUI<>();
    }
}
