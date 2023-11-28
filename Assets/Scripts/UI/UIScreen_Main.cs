using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;

public class UIScreen_Main : UIScreen
{
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Exit;
    [SerializeField] Button btn_Book;
    [SerializeField] Button btn_Language;
    [SerializeField] Button btn_Setting;

    protected override void Init()
    {
        base.Init();
        btn_Start.onClick.AddListener(OnClick_Start);
        btn_Book.onClick.AddListener(OnClick_Book);
        btn_Exit.onClick.AddListener(OnClick_Exit);
        btn_Language.onClick.AddListener(OnClick_Language);
        btn_Setting.onClick.AddListener(() => UIManager.Instance.PushScreen<UIScreen_Setting>());
    }

    private void OnClick_Start()
    {
        UIManager.Instance.PushScreen<UIScreen_Chapter>();
    }

    private void OnClick_Book()
    {
        UIManager.Instance.PushScreen<UIScreen_Book>();
    }

    private void OnClick_Language()
    {
        UIManager.Instance.PushScreen<UIScreen_Language>();
    }

    private void OnClick_Exit()
    {
        Application.Quit();
    }
}
