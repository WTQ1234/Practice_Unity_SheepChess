using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HRL;

public class PlayerPrefsManager : MonoSingleton<PlayerPrefsManager>
{
    public SectionInfo sectionInfo;

    [Button("重置存档")]
    public void ResetPlayerPref()
    {
        for (int i = 0; i < sectionInfo.ChapterInfos.Count; i++)
        {
            PlayerPrefs.SetInt($"ChapterState_{i}", (int)ChapterState.Locked);
        }
    }

    [Button("解锁所有")]
    public void UnlockPlayerPref()
    {
        for (int i = 0; i < sectionInfo.ChapterInfos.Count; i++)
        {
            print(i);
            PlayerPrefs.SetInt($"ChapterState_{i}", (int)ChapterState.Complete);
        }
    }
}
