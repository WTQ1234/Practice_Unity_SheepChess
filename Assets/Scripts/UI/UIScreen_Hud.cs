using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
using System;

public class UIScreen_Hud : UIScreen
{
    [SerializeField] Button btn_Exit;
    [SerializeField] Image img_Person;

    protected override void Init()
    {
        base.Init();
        btn_Exit.onClick.AddListener(OnClick_Exit);
    }

    private void OnClick_Exit()
    {
        UIScreen_Pause pause = UIManager.Instance.PushScreen<UIScreen_Pause>();
    }

    public void StartGame(ChapterInfo chapterInfo)
    {
        AnimalSpawner.Instance.StartGame(chapterInfo);
        //img_Person.sprite = chapterInfo.Img_Person;
        OnChessDissolve(1);
    }

    public int OnChessDissolve(float percent)
    {
        //print(percent);
        //img_Person.gameObject.SetActive(false);
        //img_Person.gameObject.SetActive(true);
        //img_Person.SetMaterialDirty();
        return 0;
    }
}
