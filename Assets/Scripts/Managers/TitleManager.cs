using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject _title;                // 타이틀 UI 오브젝트
    [SerializeField] private Slider _progressBar;              // 로딩 프로그레스 바 슬라이더
    [SerializeField] private TextMeshProUGUI _progressBarText; // 로딩 텍스트
    [SerializeField] private TextMeshProUGUI _loadingCompleteText; // 로딩 완료 텍스트

    private int dotCount = 0;

    [SerializeField] private TextMeshProUGUI _pressAnyKeyText; // 아무 키 입력 텍스트
    [SerializeField] private Animator _pressAnyKeyAnimator;// 애니메이션 오브젝트
    private bool _isLoadingComplete = false;
    private Animator _titleAnimator;


    private void Awake()
    {
        // 게임오브젝트 숨기기
        _title.SetActive(false);
        _loadingCompleteText.gameObject.SetActive(false);
        _pressAnyKeyText.gameObject.SetActive(false);
   

        var titleObj = transform.Find("TitleCanvas/Title");
        if (titleObj != null)
        {
            _titleAnimator = titleObj.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadingSequence());  // 로딩 코루틴 시작
    }

    // 씬 로딩과 프로그레스 바 업데이트를 처리하는 코루틴
    private IEnumerator LoadingSequence()
    {
        Debug.Log($"{GetType()}::{nameof(LoadingSequence)}");

        _title.SetActive(true);
        var loadingOperation = SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);

        if (loadingOperation == null)
        {
            Debug.LogWarning($"{ESceneType.Lobby} 씬을 로드하는 데 실패했습니다.");
            yield break;
        }

        loadingOperation.allowSceneActivation = false;

        SetProgress(0.5f);
        yield return new WaitForSeconds(0.5f);

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress);
            SetProgress(progress);

            // 로딩 완료 직전까지 진행
            if (progress >= 0.9f && !_isLoadingComplete)
            {
                _isLoadingComplete = true;
                // 로딩중 텍스트 끄기
                _progressBarText.gameObject.SetActive(false);
                // 로딩 완료 텍스트 켜기
                _loadingCompleteText.gameObject.SetActive(true); 

                // 메시지 및 애니메이션 표시
                _pressAnyKeyText.gameObject.SetActive(true);
                _pressAnyKeyAnimator.SetBool("isLoadingComplete",true);
                // 유저 입력 대기
                StartCoroutine(WaitForAnyKey(loadingOperation));
            }

            yield return null;
        }
    }
    private IEnumerator WaitForAnyKey(AsyncOperation loadingOperation)
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        // 트리거 실행
        if (_titleAnimator != null)
        {
            _titleAnimator.SetTrigger("isButtonPressed");
        }

        // 잠깐 대기 후 씬 전환
        yield return new WaitForSeconds(1f);
        loadingOperation.allowSceneActivation = true;
    }


    // 프로그레스 바 UI를 업데이트하는 함수
    private void SetProgress(float value)
    {
        _progressBar.value = value;
        dotCount = (dotCount + 1) % 4; // 0~3 반복
        string dots = new string('.', dotCount);
        _progressBarText.text = $"LOADING{dots}";
    }
}
