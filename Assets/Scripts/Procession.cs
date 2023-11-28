using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using HRL;

public class Procession : MonoSingleton<Procession>
{
    public List<Chess> animalsList = new List<Chess>();
    public List<Transform> gridsList = new List<Transform>();
    [SerializeField] float interval = 1.5f;//队伍间隔
    [SerializeField] int capacity = 7;//队伍容量
    [SerializeField] int[] records;//记录队伍中每个元素的位置
    [SerializeField] AudioData addSFX;
    [SerializeField] AudioData combineSFX;
    [SerializeField] AudioData successSFX;
    [SerializeField] AudioData defeatSFX;

    [Title("Temp")]
    [SerializeField] int curTweenNum;

    private void Start()
    {
        records = new int[AnimalSpawner.Instance.Sprites.Length];
    }
    public void AddToList(Chess obj)
    {
        AnimalSpawner.allCt--;
        AudioManager.Instance.PlaySFX(addSFX);
        animalsList.Add(obj);
        records[obj.GetComponent<Chess>().ID]++;
        obj.InProcession = true;
        UpdateQueueState();
    }
    public void UpdateQueueState()
    {
        if (animalsList.Count == 0)
        {
            Check();
            return;
        }
        Tween tween = null;
        for (int i = 0; i < animalsList.Count; i++)
        {
            if (i >= gridsList.Count)
            {
                break;
            }
            tween = animalsList[i].transform.DOMove(gridsList[i].position, 0.1f);
            InputManager.Instance.canClick = false;
        }
        if (tween != null)
        {
            curTweenNum += 1;
            tween.onComplete = () => {
                curTweenNum -= 1;
                if (curTweenNum == 0)
                {
                    ThreeDisappear();
                }
            };
        }
    }

    public void Check()
    {
        UIScreen_Hud hud = UIManager.Instance.GetScreen<UIScreen_Hud>();
        if (hud != null)
        {
            hud.OnChessDissolve((float)(AnimalSpawner.allCt + animalsList.Count)/ (float)AnimalSpawner.maxCt);
        }
        if (animalsList.Count >= capacity)
        {
            InputManager.Instance.canClick = false;
            AudioManager.Instance.PlaySFX(defeatSFX);
            LevelManager.Instance.OnGameEnd(false);
            PoolManager.Instance.Recycle<Chess>(animalsList[capacity]);
            animalsList[capacity].SetActive(false);
            animalsList.RemoveAt(capacity);
            return;
        }
        if (AnimalSpawner.allCt == 0)
        {
            InputManager.Instance.canClick = false;
            AudioManager.Instance.PlaySFX(successSFX);
            LevelManager.Instance.OnGameEnd(true);
            return;
        }
        InputManager.Instance.canClick = true;
    }

    public void OnGameEnd()
    {
        animalsList.Clear();
        records = new int[AnimalSpawner.Instance.Sprites.Length];
    }

    public void ThreeDisappear()//三消
    {
        List<Chess> waitToDisappearList = new List<Chess>(0);
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] == 3)
            {
                AudioManager.Instance.PlaySFX(combineSFX);
                for (int j = 0; j < animalsList.Count; j++)
                {
                    if (animalsList[j].ID == i)
                    {
                        waitToDisappearList.Add(animalsList[j]);
                    }
                }
                records[i] = 0;
                break;
            }
        }
        if (waitToDisappearList.Count == 3)
        {
            StartCoroutine(ThreeDisappearCoroutine(waitToDisappearList));
        }
        else
        {
            Check();
        }
    }

    IEnumerator ThreeDisappearCoroutine(List<Chess> ObjList)
    {
        InputManager.Instance.canClick = false;
        #region Animation
        float getBiggerSpeed = 0.01f;
        float getSmallerSpeed = 0.04f;
        float maxSize = 0.6f;
        while (true)
        {
            foreach(var item in ObjList)
            {
                item.transform.localScale += Vector3.one * getBiggerSpeed;
            }
            yield return null;
            if (ObjList[0].transform.localScale.x >= maxSize)
                break;
        }
        while (true)
        {
            foreach (var item in ObjList)
            {
                item.transform.localScale -= Vector3.one * getSmallerSpeed;
            }
            yield return null;
            if (ObjList[0].transform.localScale.x <= 0f)
                break;
        }
        #endregion
        foreach (var item in ObjList)
        {
            animalsList.Remove(item);
            item.InProcession = false;
            PoolManager.Instance.Recycle<Chess>(item);
        }
        UpdateQueueState();
        InputManager.Instance.canClick = true;
    }
}
