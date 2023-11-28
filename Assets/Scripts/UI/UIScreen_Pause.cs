using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;

public class UIScreen_Pause : UIScreen
{
    [SerializeField] Button btn_Continue;
    [SerializeField] Button btn_Exit;
    [SerializeField] Button btn_Restart;

    protected override void Init()
    {
        base.Init();
        btn_Continue.onClick.AddListener(OnClick_Continue);
        btn_Exit.onClick.AddListener(OnClick_Exit);
        btn_Restart.onClick.AddListener(OnClick_ReStart);
    }

    public override void OnShown()
    {
        base.OnShown();
        InputManager.Instance.canClick = false;
    }

    public override void OnHide()
    {
        base.OnHide();
        InputManager.Instance.canClick = true;
    }

    private void OnClick_Continue()
    {
        Remove();
    }

    private void OnClick_Exit()
    {
        Remove();
        LevelManager.Instance.OnGameExit();
    }

    private void OnClick_ReStart()
    {
        Remove();
        LevelManager.Instance.OnGameRestart();
    }
}
