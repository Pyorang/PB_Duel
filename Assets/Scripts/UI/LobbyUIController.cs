using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
    }

    public void OnClickPlayButton()
    {
        UIManager.Instance.OpenUI<ModeSelectionUI>();
    }
}
