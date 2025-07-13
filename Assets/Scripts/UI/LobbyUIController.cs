using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        UIManager.Instance.OpenUI<ModeSelectionUI>();
    }
}
