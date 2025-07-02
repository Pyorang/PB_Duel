using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 종류 열거형 정의
public enum ESceneType
{
    Title,
    Lobby,
    InGame
}

// SingletonBehaviour 상속 → 자동 싱글턴

public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    
    protected override void Init()
    {
        base.Init(); // SingletonBehaviour의 Init() 호출

        // 싱글턴 초기화 이후 추가 작업이 필요하면 여기에 작성
       
        Debug.Log("씬 로더 초기화 완료");
    }

    // 씬을 동기적으로 로드
    public void LoadScene(ESceneType sceneType)
    {
        Debug.Log($"{sceneType} 씬 로드됨 (동기적)");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    // 현재 씬을 다시 로드
    public void ReloadScene()
    {
        Debug.Log($"{SceneManager.GetActiveScene().name} 씬 다시 로드됨");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 씬을 비동기적으로 로드
    public AsyncOperation LoadSceneAsync(ESceneType sceneType)
    {
        Debug.Log($"{sceneType} 씬 로드됨 (비동기적)");
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}
