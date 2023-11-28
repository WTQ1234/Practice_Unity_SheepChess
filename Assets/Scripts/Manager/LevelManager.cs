using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using HRL;

public class LevelManager : MonoSingleton<LevelManager>
{
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    [Title("Temp")]
    public ChapterInfo chapterInfo;
    public bool isGameStart = false;

    private void Start()
    {
        UIManager.Instance.PopAllScreen();
        UIManager.Instance.PushScreen<UIScreen_Main>();
    }

    public void OnGameReady(ChapterInfo _chapterInfo)
    {
        chapterInfo = _chapterInfo;
        OnGameStart();
    }

    public void OnGameStart()
    {
        if (isGameStart) return;
        isGameStart = true;
        PoolManager.Instance.HideAll<Chess>(false);
        UIScreen_Hud hud = UIManager.Instance.PushScreen<UIScreen_Hud>();
        hud.StartGame(chapterInfo);
        InputManager.Instance.canClick = true;
        AudioManager.Instance.PlayBGM(audioClip1);
    }   
    
    public void OnGameEnd(bool isWin)
    {
        if (!isWin)
        {
            OnGameExit();
            return;
        }
        else
        {
            PlayerPrefs.SetInt($"ChapterState_{chapterInfo.Id}", (int)ChapterState.Complete);
            if (PlayerPrefs.GetInt($"ChapterState_{chapterInfo.Id + 1}") != (int)ChapterState.Complete)
            {
                PlayerPrefs.SetInt($"ChapterState_{chapterInfo.Id + 1}", (int)ChapterState.Unlocked);
            }
        }
        if (!isGameStart) return;
        isGameStart = false;
        _PopCG();
        _OnGameExit();
        chapterInfo = null;
        AudioManager.Instance.PlayBGM(audioClip2);
    }

    private void _PopCG()
    {
        UIScreen_Hud hud = UIManager.Instance.GetScreen<UIScreen_Hud>();
        if (hud != null)
        {
            UIManager.Instance.PopScreen(hud);
        }
        //var cg = UIManager.Instance.PushScreen<UIScreen_CG>();
        //cg.SetCg(chapterInfo.Img_Person);
    }

    public void OnGameExit()
    {
        if (!isGameStart) return;
        isGameStart = false;
        _OnGameExit();
        UIManager.Instance.PopScreen<UIScreen_Hud>();
        chapterInfo = null;
        AudioManager.Instance.PlayBGM(audioClip2);
    }

    private void _OnGameExit()
    {
        Procession.Instance.OnGameEnd();
        AnimalSpawner.Instance.OnGameEnd();
        PoolManager.Instance.HideAll<Chess>(true);
        InputManager.Instance.canClick = false;
    }

    public void OnGameRestart()
    {
        if (!isGameStart) return;
        _OnGameExit();
        UIScreen_Hud hud = UIManager.Instance.GetScreen<UIScreen_Hud>();
        hud.StartGame(chapterInfo);
        InputManager.Instance.canClick = true;
    }
}
