using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject _title;                // 타이틀 UI 오브젝트
    [SerializeField] private Slider _progressBar;              // 로딩 프로그레스 바 슬라이더
    [SerializeField] private TextMeshProUGUI _progressBarText; // 프로그레스 바 옆에 표시할 % 텍스트

    private void Awake()
    {
        _title.SetActive(false);  // 처음엔 타이틀 UI 비활성화
    }

    private void Start()
    {
        StartCoroutine(LoadingSequence());  // 로딩 코루틴 시작
    }

    // 씬 로딩과 프로그레스 바 업데이트를 처리하는 코루틴
    private IEnumerator LoadingSequence()
    {
        Debug.Log($"{GetType()}::{nameof(LoadingSequence)}");

        _title.SetActive(true);  // 타이틀 UI 활성화

        // 씬 로딩 비동기 작업 요청 (로비 씬)
        var loadingOperation = SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);

        if (loadingOperation == null)
        {
            Debug.LogWarning($"{ESceneType.Lobby} 씬을 로드하는 데 실패했습니다.");
            yield break;
        }

        loadingOperation.allowSceneActivation = false;

        // 초기값 세팅
        SetProgress(0.5f);
        yield return new WaitForSeconds(0.5f);

        // 로딩 완료까지 루프
        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress);
            SetProgress(progress);

            if (progress >= 0.9f)
                loadingOperation.allowSceneActivation = true;

            yield return null;
        }
    }

    // 프로그레스 바 UI를 업데이트하는 함수
    private void SetProgress(float value)
    {
        _progressBar.value = value;
        _progressBarText.text = $"{(int)(value * 100)}%";
    }
}
