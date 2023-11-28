using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HRL;
using System.Linq;

public class UIScreen_Language : UIScreen
{
    [SerializeField] Button btn_language;
    [SerializeField] Transform layout_languages;

    protected override void Init()
    {
        base.Init();
        var languageList = ConfigMgr.Instance.GetAllInfo<LanguageInfo>().Values.ToList();
        for(int i = 0; i < languageList.Count; i++)
        {
            var language = languageList[i];
            Button btn = Instantiate<Button>(btn_language, layout_languages);
            btn.onClick.AddListener(() => OnClick(language.languageType));
            btn.transform.Find("Image").GetComponent<Image>().sprite = language.languageImg;
            btn.transform.Find("Text").GetComponent<Text>().text = language.languageName;
        }
    }

    private void OnClick(LanguageType _languageType)
    {
        LanguageManager.Instance.ChangeLanguage(_languageType);
        Remove();
    }
}
