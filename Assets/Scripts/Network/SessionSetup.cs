using System;
using UnityEngine;

/*
 * 방 설정 세팅을 위한 클래스
 */

public class SessionSetup : MonoBehaviour
{
    // 방 생성 관련 변수들
    private string sessionName;
    public string SessionName { get => sessionName; set => sessionName = value; }
    private bool isPrivate;
    public bool IsPrivate
    {
        get => isPrivate; set
        {
            isPrivate = value;
            //TODO: ApplyConfig() 완성하고 호출하여 방 설정 적용
        }
    }

    private Action onTryCreateSession; //TODO: 플레이어 접속 기능 넣기
    private Action onSessionCreated; //TODO: 세션 생성 UI 기능 넣기


    // 새로운 방 생성
    // Host Session에서 Confirm 버튼과 연결
    public void TryCreateSession()
    {
        if (MatchMaker.Instance.Runner == null) // 새로운 방 생성
        {
            onTryCreateSession?.Invoke();
            MatchMaker.Instance.SetRoomCode(SessionName);
            MatchMaker.Instance.SetPrivate(IsPrivate);
            MatchMaker.Instance.TryHostSession(() => onSessionCreated?.Invoke());
        }
        else
        {
            Debug.LogError("이미 방이 생성되어 있습니다. 방을 나가고 다시 시도해주세요.");
        }
    }

    // 기존 방 설정 변경
    public void ApplyConfig()
    {

    }
}
