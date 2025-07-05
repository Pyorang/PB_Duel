using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 종류를 정의하는 열거형
public enum ESceneType
{
    Title,
    Lobby,
    InGame
}

// 씬 로딩을 담당하는 싱글톤 매니저 클래스
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    // 초기화 함수 (싱글톤 생성 시 자동 호출)
    protected override void Init()
    {
        base.Init();
        Debug.Log("씬 로더 초기화 완료");
    }

    // 씬을 동기적으로 즉시 로드
    public void LoadScene(ESceneType sceneType)
    {
        Debug.Log($"{sceneType} 씬 로드됨 (동기적)");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    // 현재 씬을 다시 로드
    public void ReloadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"{currentScene} 씬 다시 로드됨");
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene);
    }

    // 씬을 비동기적으로 로드 (로딩 중 UI 등 처리 가능)
    public AsyncOperation LoadSceneAsync(ESceneType sceneType)
    {
        Debug.Log($"{sceneType} 씬 로드됨 (비동기적)");
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}
