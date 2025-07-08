//using Fusion;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;

///*
// * 매치메이킹(로비 접속, 방 만들기/입장/종료)을 처리하는 클래스
// */

////TODO: 퓨전2 깔고 - 1. Runner 프리팹 만들기 2. 코드 테스트하기

//public class MatchMaker : MonoBehaviour, INetworkRunnerCallbacks
//{
//    public static MatchMaker Instance { get; private set; }

//    public NetworkRunner Runner;
//    public NetworkRunner runnerPrefab;
//    public NetworkObject managerPrefab; // TODO: 매니저 프리팹 만들기

//    // 방 생성 관련 변수들
//    private bool isPrivate = false; // 비공개 여부
//    private string roomcode = null; // 방 이름


//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//            Destroy(gameObject);
//    }


//    #region Mode Selection에서 Join 버튼 클릭 시 로비 접속 관련 기능

//    // Mode Selection에서 Join 버튼과 연결
//    // Join 버튼 클릭 시 JoinSession에 접속하는 함수
//    public void TryJoinLobby()
//    {
//        StartCoroutine(JoinLobby());
//    }

//    IEnumerator JoinLobby()
//    {
//        //TODO: 방 리스트에 로딩 UI 띄우는 코드 추가
//        Runner = Instantiate(runnerPrefab);
//        Runner.AddCallbacks(this); // Runner가 네트워크 이벤트를 감지 시 처리할 수 있도록 등록
//        Task<StartGameResult> task = Runner.JoinSessionLobby(SessionLobby.ClientServer); // 비동기 로비 접속
//        while (!task.IsCompleted)
//        {
//            yield return null;
//        }
//        StartGameResult result = task.Result;

//        if (result.Ok)
//        {
//            Debug.Log("로비 접속");
//            //TODO: 로비 UI 세팅 코드 추가
//        }
//        else
//        {
//            Debug.LogError("로비 접속 실패: " + result.Reason);
//            //TODO: 로비 접속 실패 UI 세팅 코드 추가
//        }
//    }
//    #endregion

//    #region Host Session에서 Confirm 버튼 클릭 시 방 생성 관련 기능

//    public void TryHostSession(Action successCallback)
//    {
//        StartCoroutine(HostSession(successCallback));
//    }

//    IEnumerator HostSession(Action successCallback)
//    {
//        // Runner가 없으면 새로 생성
//        if (!Runner)
//        {
//            Runner = Instantiate(runnerPrefab);
//            Runner.GetComponent<NetworkEvents>().PlayerJoined.AddListener((runner, player) =>
//            {
//                if (runner.IsServer && runner.LocalPlayer == player)
//                {
//                    runner.Spawn(managerPrefab);
//                }
//            });
//            Runner.AddCallbacks(this);
//        }

//        // 방 생성
//        Task<StartGameResult> task = Runner.StartGame(new StartGameArgs()
//        {
//            GameMode = GameMode.Host,
//            SessionName = roomcode,
//            SceneManager = Runner.GetComponent<INetworkSceneManager>()
//        });
//        while (!task.IsCompleted)
//        {
//            yield return null;
//        }
//        StartGameResult result = task.Result;

//        if (result.Ok) // 방 생성 성공
//        {
//            if (successCallback != null)
//                successCallback.Invoke();
//        }
//        else // 방 생성 실패
//        {
//            // TODO: 연결 실패 UI 세팅 코드 추가
//        }
//    }

//    public void SetPrivate(bool value)
//    {
//        isPrivate = value;
//    }

//    public void SetRoomCode(string code)
//    {
//        roomcode = code;
//    }
//    #endregion

//    #region INetworkRunnerCallbacks
//    public void OnConnectedToServer(NetworkRunner runner) { }
//    public void OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
//    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
//    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
//    public void OnDisconnectedFromServer(NetworkRunner runner) { }
//    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
//    public void OnInput(NetworkRunner runner, NetworkInput input) { }
//    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
//    public void OnSceneLoadStart(NetworkRunner runner) { }
//    public void OnSceneLoadDone(NetworkRunner runner) { }
//    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
//    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
//    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
//    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
//    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
//    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
//    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

//    #endregion
//}
