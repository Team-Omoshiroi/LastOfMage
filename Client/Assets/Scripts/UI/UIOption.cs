using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
    [SerializeField] private List<Toggle> _Perf;
    
    [SerializeField] private List<Toggle> _FPS;
    
    [SerializeField] private List<Toggle> _Language;

    private RectTransform _self;
    private void Awake()
    {
        _self = GetComponentInChildren<RectTransform>();
    }

    public void LowPerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckPerfAllOff())
                _Perf[0].SetIsOnWithoutNotify(true);
        }
    }
    public void MiddlePerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckPerfAllOff())
                _Perf[1].SetIsOnWithoutNotify(true);
        }
    }
    public void HighPerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if(CheckPerfAllOff())
                _Perf[2].SetIsOnWithoutNotify(true);
        }
    }
    private bool CheckPerfAllOff()
    {
        foreach (Toggle toggle in _Perf)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }

    public void FPS30(bool set)
    {
        if (set)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            if (CheckFPSAllOff())
                _FPS[0].SetIsOnWithoutNotify(true);
        }
    }
    public void FPS60(bool set)
    {
        if (set)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            if(CheckFPSAllOff())
                _FPS[1].SetIsOnWithoutNotify(true);
        }
    }
    private bool CheckFPSAllOff()
    {
        foreach (Toggle toggle in _FPS)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }

    public void EffectSound(Single set)
    {
        SoundManager.Instance?.VolumeSetting(eSoundType.PlayerEffect, set / 5.0f);
    }
    public void BGMSound(Single set)
    {
        SoundManager.Instance?.VolumeSetting(eSoundType.Bgm, set / 5.0f);
    }
    public void UISize(Single set)
    {
        UIManager.Instance.UISize = set;
    }
    public void Korean(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckLanguageAllOff())
                _Language[0].SetIsOnWithoutNotify(true);
        }
    }
    public void English(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckLanguageAllOff())
                _Language[1].SetIsOnWithoutNotify(true);
        }
    }
    private bool CheckLanguageAllOff()
    {
        foreach (Toggle toggle in _Language)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }

    public void Quit()
    {
        var ui = UIManager.Instance.ShowUI<UIStorePopup>("Lobby/PopupConfirm", _self);
        ui.SetUP("정말 종료하시겠습니까?", () => { Application.Quit(); });
    }
}
