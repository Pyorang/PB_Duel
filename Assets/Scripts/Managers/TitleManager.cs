using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject _title;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _progressBarText;
    [SerializeField] private TextMeshProUGUI _loadingCompleteText;
    [SerializeField] private TextMeshProUGUI _pressAnyKeyText;
    [SerializeField] private Animator _pressAnyKeyAnimator;

    private Animator _titleAnimator;
    private bool _isLoadingComplete = false;
    private int _dotCount = 0;

    private void Awake()
    {
        // 초기 UI 비활성화
        _title.SetActive(false);
        _loadingCompleteText.gameObject.SetActive(false);
        _pressAnyKeyText.gameObject.SetActive(false);

        // 타이틀 오브젝트에서 Animator 가져오기
        var titleObj = transform.Find("TitleCanvas/Title");
        if (titleObj != null)
        {
            _titleAnimator = titleObj.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    // 씬 로딩과 UI 갱신 처리
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

        SetProgress(0.5f);  // 잠깐 기다렸다가 진행
        yield return new WaitForSeconds(0.5f);

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress);
            SetProgress(progress);

            // 로딩 완료 상태 처리
            if (progress >= 0.9f && !_isLoadingComplete)
            {
                _isLoadingComplete = true;

                _progressBarText.gameObject.SetActive(false);
                _loadingCompleteText.gameObject.SetActive(true);

                _pressAnyKeyText.gameObject.SetActive(true);
                _pressAnyKeyAnimator.SetTrigger("isLoadingComplete");

                StartCoroutine(WaitForAnyKey(loadingOperation));
            }

            yield return null;
        }
    }

    // 유저 키 입력 대기
    private IEnumerator WaitForAnyKey(AsyncOperation loadingOperation)
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        // 애니메이션 후 씬 전환
        if (_titleAnimator != null)
        {
            _titleAnimator.SetTrigger("isButtonPressed");
        }

        yield return new WaitForSeconds(1f);
        loadingOperation.allowSceneActivation = true;
    }

    // 로딩 진행도 + 점 애니메이션 처리
    private void SetProgress(float value)
    {
        _progressBar.value = value;

        _dotCount = (_dotCount + 1) % 4;
        string dots = new string('.', _dotCount);
        _progressBarText.text = $"LOADING{dots}";
    }
}
