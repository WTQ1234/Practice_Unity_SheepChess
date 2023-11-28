using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using HRL;

public class UIScreen_Chapter : UIScreen
{
    [SerializeField] Button btn_Exit;
    [SerializeField] Button btn_Book;
    [SerializeField] Button btn_Left;
    [SerializeField] Button btn_Right;
    [SerializeField] Transform layout_Chapters;
    [SerializeField] UIComp_Btn_Chapter prefab_Chapter;
    [SerializeField] Text text_Progerss;
    [Title("Temp")]
    [SerializeField] int mCurSection;
    [SerializeField] int mCueIndex;
    [SerializeField] SectionInfo mSection;
    [SerializeField] int perChapterNum = 5;

    protected override void Init()
    {
        base.Init();
        prefab_Chapter.gameObject.SetActive(false);
        // °´Å¥
        btn_Exit.onClick.AddListener(OnClick_Exit);
        btn_Book.onClick.AddListener(OnClick_Book);
        btn_Left.onClick.AddListener(OnClick_Left);
        btn_Right.onClick.AddListener(OnClick_Right);
    }

    public override void OnShown()
    {
        base.OnShown();
        mCurSection = 0;
        mCueIndex = 0;
        mSection = ConfigMgr.Instance.GetBasicInfoById<SectionInfo>(mCurSection);
        foreach (var child in layout_Chapters.GetComponentsInChildren<Button>())
        {
            Destroy(child.gameObject);
        }
        _SetSection();
    }

    private void _SetSection()
    {
        for (int i = 0; i < layout_Chapters.childCount; i++)
        {
            var transform = layout_Chapters.GetChild(i);
            GameObject.Destroy(transform.gameObject);
        }
        for(int i = mCueIndex; i < Mathf.Min(mCueIndex + 5, mSection.ChapterInfos.Count); i++)
        {
            var chapterInfo = mSection.ChapterInfos[i];
            UIComp_Btn_Chapter curBtn = Instantiate<UIComp_Btn_Chapter>(prefab_Chapter, layout_Chapters);
            curBtn.gameObject.SetActive(true);
            ChapterState chapterState = (ChapterState)PlayerPrefs.GetInt($"ChapterState_{i}");
            if (i == 0 && (chapterState == ChapterState.Locked))
            {
                chapterState = ChapterState.Unlocked;
                PlayerPrefs.SetInt($"ChapterState_{i}", (int)ChapterState.Unlocked);
            }
            curBtn.text_chapter.text = (i + 1).ToString();
            switch (chapterState)
            {
                case ChapterState.Locked:
                    curBtn.img_unlock.gameObject.SetActive(false);
                    curBtn.img_lock.gameObject.SetActive(true);
                    curBtn.img_bottom_green.gameObject.SetActive(false);
                    curBtn.img_bottom_gray.gameObject.SetActive(true);
                    break;
                case ChapterState.Unlocked:
                    curBtn.img_unlock.gameObject.SetActive(true);
                    curBtn.img_lock.gameObject.SetActive(false);
                    curBtn.img_bottom_green.gameObject.SetActive(false);
                    curBtn.img_bottom_gray.gameObject.SetActive(false);
                    //
                    curBtn.btn.onClick.AddListener(() => OnClick(chapterInfo));
                    break;
                case ChapterState.Complete:
                    curBtn.img_unlock.gameObject.SetActive(true);
                    curBtn.img_lock.gameObject.SetActive(false);
                    curBtn.img_bottom_green.gameObject.SetActive(true);
                    curBtn.img_bottom_gray.gameObject.SetActive(false);
                    if ((ChapterState)PlayerPrefs.GetInt($"ChapterState_{i + 1}") == ChapterState.Locked &&
                        (ChapterState)PlayerPrefs.GetInt($"ChapterState_{i}") == ChapterState.Locked)
                    {
                        PlayerPrefs.SetInt($"ChapterState_{i}", (int)ChapterState.Unlocked);
                    }
                    //
                    curBtn.btn.onClick.AddListener(() => OnClick(chapterInfo));
                    break;
            }
        }
        int completeNum = 0;
        for (int i = 0; i < mSection.ChapterInfos.Count; i++)
        {
            var chapterInfo = mSection.ChapterInfos[i];
            ChapterState chapterState = (ChapterState)PlayerPrefs.GetInt($"ChapterState_{i}");
            if (i == 0 && (chapterState == ChapterState.Locked))
            {
                chapterState = ChapterState.Unlocked;
                PlayerPrefs.SetInt($"ChapterState_{i}", (int)ChapterState.Unlocked);
            }
            switch (chapterState)
            {
                case ChapterState.Complete:
                    completeNum++;
                    break;
            }
        }
        text_Progerss.text = $"{completeNum}/{mSection.ChapterInfos.Count}";
    }

    private void OnClick(ChapterInfo chapterInfo)
    {
        LevelManager.Instance.OnGameReady(chapterInfo);
    }

    private void OnClick_Book()
    {
        UIManager.Instance.PushScreen<UIScreen_Book>();
    }

    private void OnClick_Exit()
    {
        Remove();
    }

    private void OnClick_Left()
    {
        mCueIndex -= perChapterNum;
        if (mCueIndex < 0)
        {
            mCueIndex = 0;
        }
        _SetSection();
    }

    private void OnClick_Right()
    {
        mCueIndex += perChapterNum;
        if (mCueIndex >= mSection.ChapterInfos.Count)
        {
            mCueIndex = 0;
        }
        _SetSection();
    }
}

public enum ChapterState
{
    Locked,
    Unlocked,
    Complete,
}
