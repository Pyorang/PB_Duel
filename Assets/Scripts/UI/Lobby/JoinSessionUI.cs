using UnityEngine;
using UnityEngine.UI;

/*
 * JoinSessionUI 프리팹에 붙는 스크립트
 */

public class JoinSessionUI : BaseUI
{
    [SerializeField]
    private Button _backButton;

    private void Start()
    {
        _backButton.onClick.AddListener(OnClickCloseButton);
        _backButton.onClick.AddListener(MatchMaker.Instance.TryCloseLobby);
    }
}
