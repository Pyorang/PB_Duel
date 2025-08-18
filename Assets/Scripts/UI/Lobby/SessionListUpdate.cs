using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * JoinSession의 방 리스트 업데이트하는 클래스
 * JoinSessionUI 프리팹에 붙는 스크립트
 */

public class SessionListUpdate : MonoBehaviour
{
    private SessionItemUI _sessionItemPrefab;
    [SerializeField]
    private Transform _sessionItemHolder;

    private readonly List<SessionItemUI> _sessionItems = new List<SessionItemUI>();

    private void Awake()
    {
        MatchMaker.Instance.joinSessionList = this;
        _sessionItemPrefab = Resources.Load<SessionItemUI>("UI/SessionItemUI");
    }


    private void ClearBrowser()
    {
        foreach (SessionItemUI item in _sessionItems)
            Destroy(item.gameObject);
        _sessionItems.Clear();
    }

    SessionItemUI GetSessionItem(int index)
    {
        return _sessionItems.ElementAtOrDefault(index) ?? CreateSessionItem();
    }

    SessionItemUI CreateSessionItem()
    {
        SessionItemUI item = Instantiate(_sessionItemPrefab, _sessionItemHolder);
        _sessionItems.Add(item);
        return item;
    }

    public void OnSessionListUpdated(List<SessionInfo> sessionList)
    {
        int i = 0;
        for (; i < sessionList.Count; i++)
        {
            SessionInfo sessionInfo = sessionList[i];

            bool isPrivate = false;
            if (sessionInfo.Properties.TryGetValue("isPrivate", out var isPrivateProp) && isPrivateProp.PropertyValue is bool isPrivateValue)
                isPrivate = isPrivateValue;
            string hostName = "";
            if(sessionInfo.Properties.TryGetValue("hostName", out var hostNameProp) && hostNameProp.PropertyValue is string hostNameValue)
                hostName = hostNameValue;

            GetSessionItem(i).Init(
                sessionInfo.Name,
                hostName,
                isPrivate,
                sessionInfo.IsOpen
            );
        }

        for(;i<_sessionItems.Count;i++)
        {
            _sessionItems[i].Disable();
        }
    }
}
