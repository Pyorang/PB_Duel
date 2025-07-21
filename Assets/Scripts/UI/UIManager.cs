using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// UI 전반을 관리하는 싱글톤 UI 매니저 클래스
public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform CanvasTransform;          // 메인 UI 캔버스 트랜스폼
    public Transform ClosedUITrasnform;        // 비활성화된 UI를 보관하는 트랜스폼

    private BaseUI _frontUI;                   // 현재 화면 최상단에 위치한 UI
    private Dictionary<Type, BaseUI> _openUIPool = new();    // 활성화된 UI 저장용 딕셔너리
    private Dictionary<Type, BaseUI> _closeUIPool = new();   // 비활성화된 UI 재사용 딕셔너리

    // 싱글톤 초기화 시 자동 호출되는 함수
    protected override void Init()
    {
        base.Init();
    }

    // 현재 활성화된 UI 중 특정 타입의 UI를 반환 (없으면 null)
    public BaseUI GetActiveUI<T>() where T : BaseUI
    {
        _openUIPool.TryGetValue(typeof(T), out var ui);
        return ui;
    }

    // 가장 위에 떠 있는 UI 반환
    public BaseUI GetFrontUI() => _frontUI;

    // 현재 열린 UI가 하나라도 있는지 여부 반환
    public bool ExistAnyOpenUI() => _frontUI != null;

    // 가장 위에 있는 UI 닫기
    public void CloseFrontUI()
    {
        _frontUI?.Close();
    }

    // 열려있는 모든 UI를 순차적으로 닫기
    public void CloseAllUI()
    {
        while (_frontUI != null)
        {
            _frontUI.Close();
        }
    }

    // UI 열기: 존재하면 재사용, 아니면 새로 생성 후 활성화
    public void OpenUI<T>(BaseUIData data = null) where T : BaseUI
    {
        var uiType = typeof(T);
        Debug.Log($"{GetType()}::{nameof(OpenUI)}({uiType})");

        var ui = GetUI<T>(out bool isAlreadyOpen);

        if (ui == null)
        {
            Debug.LogWarning($"{uiType} 프리팹을 찾을 수 없습니다.");  // 한글 경고문
            return;
        }

        if (isAlreadyOpen)
        {
            Debug.Log($"{uiType} UI가 이미 열려 있습니다.");
            return;
        }

        ui.Init(CanvasTransform);
        ui.transform.SetSiblingIndex(CanvasTransform.childCount - 1); // 최상단 배치
        ui.gameObject.SetActive(true);
        if(!(data is null)) ui.SetData(data);
        ui.Show();

        _frontUI = ui;
        _openUIPool[uiType] = ui;
    }

    // UI 닫기: UI 비활성화 및 재사용 풀로 이동
    public void CloseUI(BaseUI ui)
    {
        var uiType = ui.GetType();
        Debug.Log($"{GetType()}::{nameof(CloseUI)}({uiType})");

        ui.gameObject.SetActive(false);
        _openUIPool.Remove(uiType);
        _closeUIPool[uiType] = ui;
        ui.transform.SetParent(ClosedUITrasnform);

        _frontUI = null;

        // 가장 위에 있는 UI를 다시 찾기
        if (CanvasTransform.childCount > 0)
        {
            var lastChild = CanvasTransform.GetChild(CanvasTransform.childCount - 1);
            _frontUI = lastChild.GetComponent<BaseUI>();
        }
    }

    // UI를 풀에서 꺼내거나 새로 생성해 반환
    private BaseUI GetUI<T>(out bool isAlreadyOpen) where T : BaseUI
    {
        var uiType = typeof(T);

        if (_openUIPool.TryGetValue(uiType, out var activeUI))
        {
            isAlreadyOpen = true;
            return activeUI;
        }

        if (_closeUIPool.TryGetValue(uiType, out var closedUI))
        {
            isAlreadyOpen = false;
            return closedUI;
        }

        var prefab = Resources.Load<BaseUI>($"UI/{uiType}");
        if (prefab == null)
        {
            isAlreadyOpen = false;
            Debug.LogWarning($"{uiType} 프리팹을 찾을 수 없습니다.");
            return null;
        }

        isAlreadyOpen = false;
        return Instantiate(prefab);
    }
}
