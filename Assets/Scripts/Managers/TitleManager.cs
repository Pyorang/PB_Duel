using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject _title;               // 타이틀 UI 오브젝트
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

        // 로딩 작업 실패 시 경고 출력 후 종료
        if (loadingOperation == null)
        {
            Debug.LogWarning($"{ESceneType.Lobby} 씬을 로드하는 데 실패했습니다.");
            yield break;
        }

        loadingOperation.allowSceneActivation = false; // 씬 자동 전환 비활성화

        _progressBar.value = 0.5f;                     // 초기 50%
        _progressBarText.text = "50%";                 // 텍스트도 50%로 초기화
        yield return new WaitForSeconds(0.5f);         // 0.5초 대기

        // 씬 로딩 완료 전까지 루프
        while (!loadingOperation.isDone)
        {

            _progressBar.value = loadingOperation.progress; // 로딩 진행도 반영
            _progressBarText.text = $"{(int)(_progressBar.value * 100.0f)}%"; // 텍스트 업데이트

            // 로딩이 90% 이상 되면 씬 전환 허용
            if (_progressBar.value >= 0.9f)
                loadingOperation.allowSceneActivation = true;

            yield return null; // 다음 프레임까지 대기
        }
    }
}
