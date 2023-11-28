using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;

public class UIScreen_CG : UIScreen
{
    [SerializeField] Button btn_Exit;
    [SerializeField] Image img;
    protected override void Init()
    {
        base.Init();
        btn_Exit.onClick.AddListener(OnClick_Exit);
    }

    private void OnClick_Exit()
    {
        Remove();
    }

    public void SetCg(Sprite sprite)
    {
        img.sprite = sprite;
    }
}
