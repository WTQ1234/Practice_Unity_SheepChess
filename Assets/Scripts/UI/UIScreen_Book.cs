using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;

public class UIScreen_Book : UIScreen
{
    [SerializeField] Button btn_Exit;
    [SerializeField] Material mat_book;
    [SerializeField] Material mat_clear;
    [SerializeField] int mCurSection;
    [SerializeField] SectionInfo mSection;
    [SerializeField] Transform layout_cg;
    [SerializeField] Transform prefab_cg;
    [SerializeField] ScrollRect scrollRect;

    protected override void Init()
    {
        base.Init();
        btn_Exit.onClick.AddListener(OnClick_Exit);
        mCurSection = 0;
        mSection = ConfigMgr.Instance.GetBasicInfoById<SectionInfo>(mCurSection);
        for (int i = 0; i < mSection.ChapterInfos.Count; i++)
        {
            var chapterInfo = mSection.ChapterInfos[i];
            ChapterState chapterState = (ChapterState)PlayerPrefs.GetInt($"ChapterState_{i}");
            Transform cg = Instantiate(prefab_cg, layout_cg);
            cg.gameObject.SetActive(true);
            Image img = cg.transform.Find("Mask/Img").GetComponent<Image>();
            img.sprite = chapterInfo.Img_Person;
            switch (chapterState)
            {
                case ChapterState.Locked:
                    img.material = mat_book;
                    cg.transform.Find("Mask_Lock").gameObject.SetActive(true); 
                    break;
                case ChapterState.Unlocked:
                    img.material = mat_book;
                    cg.transform.Find("Mask_Lock").gameObject.SetActive(true);
                    break;
                case ChapterState.Complete:
                    img.material = mat_clear;
                    cg.transform.Find("Mask_Lock").gameObject.SetActive(false);
                    var btn = cg.transform.Find("Btn").GetComponent<Button>();
                    int index = i;
                    btn.onClick.AddListener(() =>
                    {
                        OnClick_CG(chapterInfo.Img_Person);
                    });
                    break;
            }
            
        }
        //scrollRect.horizontalNormalizedPosition = 0;
    }

    private void OnClick_Exit()
    {
        Remove();
    }

    private void OnClick_CG(Sprite sprite)
    {
        var cg = UIManager.Instance.PushScreen<UIScreen_CG>();
        cg.SetCg(sprite);
    }
}
