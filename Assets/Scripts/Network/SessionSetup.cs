using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * 방 설정 세팅을 위한 클래스
 * HostSessionUI 프리팹에 붙는 스크립트
 */

public class SessionSetup : MonoBehaviour
{
    // UI 요소 인스펙터에 연결
    [SerializeField]
    private TMP_InputField _sessionNameInputField;
    [SerializeField]
    private Toggle _isPrivateToggle;
    [SerializeField]
    private TMP_InputField _passwordInputField;

    // 방 생성 관련 변수들
    private string _sessionName;
    private bool _isPrivate;
    private string _password;
    private string _hostName;

    public string SessionName { get => _sessionName; set => _sessionName = value; }
    public bool IsPrivate
    {
        get => _isPrivate; 
        set
        {
            _isPrivate = value;
            //TODO: ApplyConfig() 호출하여 방 설정 적용
        }
    }
    public string Password { get => _password; set => _password = value; }


    private Action _onTryCreateSession; //TODO: 플레이어 접속 기능 넣기
    private Action _onSessionCreated; //TODO: 세션 생성 UI 기능 넣기



    private void Awake()
    {
        _sessionNameInputField.onValueChanged.AddListener((value) => SessionName = value);
        _isPrivateToggle.onValueChanged.AddListener((value) => IsPrivate = value);
        _passwordInputField.onValueChanged.AddListener((value) => Password = value);
    }

    private void Start()
    {
        _hostName = GameObject.FindFirstObjectByType<ModeSelectionUI>().GetHostName();
    }

    // 새로운 방 생성
    // Host Session에서 Create 버튼과 연결
    public void TryCreateSession()
    {
        if (MatchMaker.Instance.Runner == null) // 새로운 방 생성
        {
            _onTryCreateSession?.Invoke();
            MatchMaker.Instance.SetRoomCode(SessionName);
            MatchMaker.Instance.SetPrivate(IsPrivate);
            MatchMaker.Instance.SetHostName(_hostName);
            MatchMaker.Instance.TryHostSession(() => _onSessionCreated?.Invoke());
        }
        else // 이미 방이 생성되었다면 설정 변경
        {
            ApplyConfig();
            Debug.LogError("이미 방이 생성되어 있습니다. 방을 나가고 다시 시도해주세요.");
        }
    }

    //TODO: ApplyConfig 완성하기
    // 기존 방 설정 변경
    public void ApplyConfig()
    {

    }
}
