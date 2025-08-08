using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 매치메이킹(로비 접속, 방 만들기/입장/종료)을 처리하는 클래스
 * 로비씬의 Lobby > LobbyNetwork 오브젝트에 붙는 스크립트
 */

public class MatchMaker : SingletonBehaviour<MatchMaker>, INetworkRunnerCallbacks
{
    public NetworkRunner Runner;
    private NetworkRunner _runnerPrefab; // TODO: 프리팹에 addons 추가하기
    private NetworkObject _networkManagerPrefab; // TODO: gameManager.cs 만들어서 컴포넌트로 추가
    public SessionListUpdate joinSessionList; // SessionListUpdate.cs에서 초기화함

    // 방 생성 관련 변수
    private bool _isPrivate = false; // 방 비공개 여부
    private string _roomcode = null; // 방 이름
    private string _hostName = null;

    protected override void Init()
    {
        IsDestroyOnLoad = false;
        base.Init();

        _runnerPrefab = Resources.Load<NetworkRunner>("Network/Runner");
        _networkManagerPrefab = Resources.Load<NetworkObject>("Network/NetworkManager");
    }

    #region Mode Selection에서 Join 버튼 클릭 시 로비 접속 및 나가기 관련 기능

    // Mode Selection에서 Join 버튼과 연결
    // Join 버튼 클릭 시 JoinSession에 네트워크 접속하는 함수
    public void TryJoinLobby()
    {
        StartCoroutine(JoinLobby());
    }

    IEnumerator JoinLobby()
    {
        //TODO: 방 리스트에 로딩 UI 띄우는 코드 추가
        Runner = Instantiate(_runnerPrefab);
        Runner.AddCallbacks(this); // Runner가 네트워크 이벤트를 감지 시 처리할 수 있도록 등록
        Task<StartGameResult> task = Runner.JoinSessionLobby(SessionLobby.ClientServer); // 비동기 로비 접속
        while (!task.IsCompleted)
        {
            yield return null;
        }
        StartGameResult result = task.Result;

        if (result.Ok)
        {
            Debug.Log("로비 접속");
            //TODO: 로비 UI 세팅 코드 추가
        }
        else
        {
            Debug.LogError("로비 접속 실패: " + result.ToString());
            //TODO: 로비 접속 실패 UI 세팅 코드 추가
        }
    }


    // JoinSession에서 Back 버튼 클릭 시 네트워크 끊기 기능
    public void TryCloseLobby()
    {
        StartCoroutine(CloseLobby());
    }

    IEnumerator CloseLobby()
    {
        Task task = Runner.Shutdown(); // 네트워크 연결 종료
        while(!task.IsCompleted)
        {
            yield return null;
        }
        Runner = null;
    }

    #endregion

    #region Host Session에서 Confirm 버튼 클릭 시 방 생성 관련 기능

    public void TryHostSession(Action successCallback)
    {
        StartCoroutine(HostSession(successCallback));
    }

    IEnumerator HostSession(Action successCallback)
    {
        // Runner가 없으면 새로 생성
        if (!Runner)
        {
            Runner = Instantiate(_runnerPrefab);
            Runner.GetComponent<NetworkEvents>().PlayerJoined.AddListener((runner, player) =>
            {
                if (runner.IsServer && runner.LocalPlayer == player)
                {
                    runner.Spawn(_networkManagerPrefab);
                }
            });
            Runner.AddCallbacks(this);
        }

        // 방 생성
        Task<StartGameResult> task = Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = _roomcode,
            SceneManager = Runner.GetComponent<INetworkSceneManager>(),
            SessionProperties = new Dictionary<string, SessionProperty>
            {
                { "isPrivate", _isPrivate },
                { "hostName", _hostName }
            }
        });
        while (!task.IsCompleted)
        {
            yield return null;
        }
        StartGameResult result = task.Result;

        if (result.Ok) // 방 생성 성공
        {
            Debug.Log("session player count : " + Runner.SessionInfo.PlayerCount);
            Debug.Log($"Private bool is {_isPrivate}. Session is {(Runner.SessionInfo.IsVisible ? "public" : "private")}");

            if (successCallback != null)
                successCallback.Invoke();
        }
        else // 방 생성 실패
        {
            Debug.LogError($"방 생성 실패: {result.ToString()}");
            // TODO: 연결 실패 UI 세팅 코드 추가
        }
    }

    public void SetPrivate(bool value)
    {
        _isPrivate = value;
    }

    public void SetRoomCode(string code)
    {
        _roomcode = code;
    }

    public void SetHostName(string name)
    {
        _hostName = name;
    }
    #endregion

    #region 재정의한 INetworkRunnerCallbacks

    // Join버튼 눌러 JoinSession 입장 시 방 리스트를 업데이트할 때 호출됨
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("방 생성!!!!");
        if(sessionList == null || sessionList.Count == 0)
        {
            Debug.Log("방이 없습니다.");
            return;
        }
        joinSessionList.OnSessionListUpdated(sessionList);
    }

    // NetworkRunner가 종료될 때 호출됨(메인메뉴로 돌아갈 때)
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Runner = null;
        if(shutdownReason != ShutdownReason.Ok)
        {
            //TODO: 네트워크 종료 실패 UI 세팅 코드 추가
            Debug.LogError($"네트워크 종료 실패: {shutdownReason}");
        }
    }
    #endregion

    #region INetworkRunnerCallbacks
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}
