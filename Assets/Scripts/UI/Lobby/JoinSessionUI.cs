using UnityEngine;
using UnityEngine.UI;

public class JoinSessionUI : BaseUI
{
    [SerializeField]
    private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnClickCloseButton);
    }
}
