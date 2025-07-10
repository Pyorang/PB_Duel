using System;
using UnityEngine;

// UI에 넘길 콜백 함수 담는 클래스
public class BaseUIData
{
    public Action ActionOnShow;   // UI가 열릴 때 실행될 액션
    public Action ActionOnClose;  // UI가 닫힐 때 실행될 액션
}

// 모든 UI 클래스들이 상속받는 기본 UI 클래스
public class BaseUI : MonoBehaviour
{
    public Animation AnimOnOpen;  // UI가 열릴 때 재생할 애니메이션

    private Action _actionOnShow;
    private Action _actionOnClose;

    // UI를 Canvas에 붙이고 기본 상태로 초기화하는 함수
    public virtual void Init(Transform canvas)
    {
        Debug.Log($"{GetType()}::{nameof(Init)}");

        transform.SetParent(canvas, false);  // 위치 초기화 쉽게

        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        _actionOnShow = null;
        _actionOnClose = null;
    }

    // 외부에서 넘겨준 UI 데이터 세팅
    public virtual void SetData(BaseUIData data)
    {
        Debug.Log($"{GetType()}::{nameof(SetData)}");

        if (data == null)
        {
            Debug.LogWarning("BaseUIData가 null입니다. 데이터가 정상적으로 전달되지 않았습니다.");
            return;
        }

        _actionOnShow = data.ActionOnShow;
        _actionOnClose = data.ActionOnClose;
    }

    // UI를 보여주는 함수
    public virtual void Show()
    {
        if (AnimOnOpen != null && !AnimOnOpen.isPlaying)
        {
            AnimOnOpen.Play();
        }

        _actionOnShow?.Invoke();
        _actionOnShow = null;  // 중복 호출 방지
    }

    // UI를 닫는 함수
    public virtual void Close(bool isCloseAll = false)
    {
        if (isCloseAll == false)
        {
            _actionOnClose?.Invoke();
        }

        _actionOnClose = null;

        UIManager.Instance.CloseUI(this);
    }

    // 닫기 버튼이 눌렸을 때 실행되는 기본 동작
    public virtual void OnClickCloseButton()
    {
        Close();
    }
}
