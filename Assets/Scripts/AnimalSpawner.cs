using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimalSpawner : MonoSingleton<AnimalSpawner>
{
    [SerializeField] Chess perfab;
    [SerializeField] List<Chess> Chesses;
    [SerializeField] Transform ChessPos;
    [SerializeField] Transform ChessParent;
    [SerializeField] Sprite[] sprites;
    float width = 20f;
    float height = 10f;
    [SerializeField] float border = 2f;
    [SerializeField] Vector2 borderOffset = new Vector2(1, 1);

    [Title("Temp")]
    [SerializeField] int groupCt = 30; //组数
    public static int allCt;
    public static int maxCt;
    public int cellPos = 200;
    int layers = 1;//层数从1开始
    public Sprite[] Sprites => sprites;

    public void StartGame(ChapterInfo _chapterInfo)
    {
        if (_chapterInfo == null)
            return;
        width = _chapterInfo.width;
        height = _chapterInfo.height;
        layers = 1;
        if (_chapterInfo.useChessBoard)
        {
            _StartGame_UseChessBoard(_chapterInfo);
        }
        else
        {
            _StartGame_RandomChessPos(_chapterInfo);
        }
    }

    private void _StartGame_RandomChessPos(ChapterInfo _chapterInfo)
    {
        groupCt = _chapterInfo.GroupNum;
        allCt = groupCt * 3;
        maxCt = allCt;
        int chessKind = Mathf.Min(_chapterInfo.ChessKindNum, sprites.Length);
        for (int i = 0; i < groupCt; i++)
        {
            int randID = Random.Range(0, chessKind);
            for (int j = 0; j < 3; j++)
            {
                var obj = ReleaseObj();
                obj.SetID(randID, true);
                Chesses.Add(obj);
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                SpriteRenderer childSpriteRender = obj.transform.GetChild(0).GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = layers++;
                childSpriteRender.sortingOrder = layers++;
                childSpriteRender.sprite = sprites[randID];
            }
        }
    }

    private void _StartGame_UseChessBoard(ChapterInfo _chapterInfo)
    {
        int count = _chapterInfo.chessBoards.Count;
        int chessKind = Mathf.Min(_chapterInfo.ChessKindNum, sprites.Length);
        List<int> randIdList = new List<int>();

        allCt = 0;
        for (int i = 0; i < count; i++)
        {
            bool[,] chessBoard = _chapterInfo.chessBoards[i];
            foreach(var b in chessBoard)
            {
                if (b)
                {
                    allCt++;
                }
            }
        }
        groupCt = allCt / 3;
        maxCt = allCt;
        int needCount = groupCt;
        for (int i = 0; (i < needCount) && (i < chessKind); i++)
        {
            randIdList.Add(i);
            randIdList.Add(i);
            randIdList.Add(i);
            needCount--;
            if (needCount <= 0)
            {
                break;
            }
        }
        for (int i = 0; i < needCount; i++)
        {
            int randId = Random.Range(0, chessKind);
            randIdList.Add(randId);
            randIdList.Add(randId);
            randIdList.Add(randId);
        }
        for (int i = 0; i < count; i++)
        {
            bool[,] chessBoard = _chapterInfo.chessBoards[i];
            int width = chessBoard.GetLength(0);
            int height = chessBoard.GetLength(1);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    bool spawn = chessBoard[w, h];
                    if (spawn)
                    {
                        int randIndex = Random.Range(0, randIdList.Count);
                        int randId = randIdList[randIndex];
                        randIdList.RemoveAt(randIndex);
                        float z = (groupCt - layers) * 0.01f;
                        Vector3 pos = new Vector3(borderOffset[0] + w - width / 2f, -(borderOffset[1] + h - height / 2f), z);
                        var obj = ReleaseObj(false);
                        obj.SetID(randId, true);
                        Chesses.Add(obj);
                        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                        SpriteRenderer childSpriteRender = obj.transform.GetChild(0).GetComponent<SpriteRenderer>();
                        spriteRenderer.sortingOrder = layers++;
                        childSpriteRender.sortingOrder = layers++;
                        childSpriteRender.sprite = sprites[randId];
                        obj.transform.localPosition = pos + ChessPos.position;
                    }
                }
            }
        }
    }

    public void OnGameEnd()
    {
        layers = 1;
    }

    public Chess SpawnAnimalByID(int id, Vector3 pos)
    {
        var obj = ReleaseObj(false);
        obj.GetComponent<Chess>().SetID(id, false);
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        SpriteRenderer childSpriteRender = obj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 9998;
        childSpriteRender.sortingOrder = 9999;
        childSpriteRender.sprite = sprites[id];
        obj.transform.localPosition = pos;
        return obj;
    }

    public Chess ReleaseObj(bool randomPos=true)
    {
        Vector3 pos = Vector3.zero;
        if (randomPos)
        {
            // 限制数值取整到10位
            float x = Mathf.Round(Random.Range(-width / 2, width / 2));
            float y = Mathf.Round(Random.Range(-height / 2, height / 2));
            float z = (groupCt - layers) * 0.01f;
            pos = new Vector3(x, y, z) + ChessPos.position;
        }
        return PoolManager.Instance.Release<Chess>(pos);
    }    

    public void RefreshObj(ChapterInfo chapterInfo)
    {
        OnGameEnd();
        PoolManager.Instance.HideAll<Chess>(true);
        _StartGame_UseChessBoard(chapterInfo);
    }
}
