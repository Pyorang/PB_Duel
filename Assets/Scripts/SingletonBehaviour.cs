using System;
using UnityEngine;

// MonoBehaviour를 상속받아 싱글톤 패턴을 구현하는 제네릭 베이스 클래스
public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // 씬 전환 시에도 파괴되지 않을지 여부를 지정하는 속성 (기본값은 false)
    protected bool IsDestroyOnLoad { get; set; } = false;

    // 싱글톤 인스턴스를 저장하는 정적 변수
    private static T s_instance;

    // 싱글톤 인스턴스에 접근할 수 있는 프로퍼티
    public static T Instance => s_instance;

    // Unity의 Awake 콜백 메서드, 오브젝트가 생성될 때 호출됨
    private void Awake()
    {
        Init();
    }

    // 싱글톤 인스턴스 초기화 메서드
    protected virtual void Init()
    {
        // 이미 인스턴스가 존재하면 중복 방지를 위해 현재 게임 오브젝트를 파괴함
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없으면 현재 오브젝트를 인스턴스로 할당
        s_instance = this as T;

        // IsDestroyOnLoad가 false일 경우 씬 전환 시 파괴되지 않도록 설정
        if (IsDestroyOnLoad == false)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // 오브젝트가 파괴될 때 호출되는 콜백 메서드
    private void OnDestroy()
    {
        Dispose();
    }

    // 싱글톤 인스턴스를 해제하는 가상 메서드
    protected virtual void Dispose()
    {
        s_instance = null;
    }
}
