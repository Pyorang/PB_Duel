using UnityEngine;
using UnityEngine.UI;

/*
 * 로비씬의 Lobby > LobbyUIController 오브젝트에 붙는 스크립트
 */

public class LobbyUIController : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(OnClickPlayButton);
    }

    public void OnClickPlayButton()
    {
        UIManager.Instance.OpenUI<ModeSelectionUI>();
    }
}
