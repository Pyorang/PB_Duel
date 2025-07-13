using UnityEngine;

public class ModeSelectionUI : BaseUI
{
    public void OnClickHostButton()
    {
        UIManager.Instance.OpenUI<HostSessionUI>();
    }

    public void OnClickJoinButton()
    {
        UIManager.Instance.OpenUI<JoinSessionUI>();
    }
}
