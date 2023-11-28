using System.Linq;
using System.Data;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using HRL;
//using PixelCrushers.DialogueSystem;

[SerializeField]
public class ChapterInfo : BasicInfo
{
    [HideIf("useChessBoard")]
    [Title("拼图组数 总数为组数*3 如果使用棋盘配置，这里无效")]
    public int GroupNum = 10;
    [Title("拼图种类 即有多少种棋子 超过最大图片数量也没事")]
    public int ChessKindNum = 5;

    [Title("关卡图片")]
    public Sprite Img_Person;

    //[Title("战斗前对话")]
    //[ValueDropdown("GetConversations")]
    //public string conversation_before;
    //[Title("战斗后对话")]
    //[ValueDropdown("GetConversations")]
    //public string conversation_after;
    //private static IEnumerable GetConversations()
    //{
    //    var list = new List<string>();
    //    var selectedDatabase = Resources.Load<DialogueDatabase>("HRLDialogueDatabase");
    //    if (selectedDatabase != null)
    //    {
    //        foreach (var entry in selectedDatabase.conversations)
    //        {
    //            var title = entry.Title;
    //            if (!list.Contains(title))
    //            {
    //                list.Add(title);
    //            }
    //        }
    //    }
    //    return list.ToArray();
    //}

    [Title("棋盘的长宽 随机棋子会限制在这里")]
    [InfoBox("注意，改变这里会使配置的棋盘清空！")]
    [OnValueChanged("OnResetBoard")]
    public int width = 12;
    [InfoBox("注意，改变这里会使配置的棋盘清空！")]
    [OnValueChanged("OnResetBoard")]
    public int height = 6;

    [Title("是否要使用预配置的棋盘而非随机位置")]
    [OnValueChanged("OnChangeUse")]
    public bool useChessBoard = false;

    [ShowIf("useChessBoard")]
    [Button("新增一层")]
    public void AddOneBoard()
    {
        chessBoards.Add(new bool[width, height]);
    }

    [ShowIf("useChessBoard")]
    [InfoBox("$curCountInChessBoard")]
    [OnValueChanged("OnChangeBoard", IncludeChildren = true)]
    public List<bool[,]> chessBoards = new List<bool[,]>();

    //[TableMatrix(HorizontalTitle = "棋盘", SquareCells = true)]
    //public bool[,] curChessBoard;

    [HideInInspector]
    public string curCountInChessBoard = "///";

    // 在切换是否选择预配置棋盘时调用
    private void OnChangeUse()
    {
        if (useChessBoard)
        {
            if (chessBoards.Count == 0)
            {
                chessBoards.Add(new bool[width, height]);
            }
        }
        else
        {
        }
    } 
    // 在编辑时调用
    private void OnChangeBoard()
    {
        int count = 0;
        foreach (var item in chessBoards)
        {
            foreach(var it in item)
            {
                if (it)
                {
                    count++;
                }
            }
        }
        if (count % 3 != 0)
        {
            curCountInChessBoard = $"<color=#FF0000>当前棋盘总数为 {count} ,不为3的倍数，将会卡关！不会被记录！</color>";
        }
        else
        {
            curCountInChessBoard = $"当前棋盘总数为 {count} ,为3的倍数，没问题";
            //GroupNum = count / 3;
        }
        if (Application.isPlaying)
        {
            AnimalSpawner.Instance.RefreshObj(this);
        }
    }
    private void OnResetBoard()
    {
        if (chessBoards.Count == 0)
        {
            chessBoards.Add(new bool[width, height]);
        }
        else
        {
            var newChessBoards = new List<bool[,]>();
            for (int i = 0; i < chessBoards.Count; i++)
            {
                var board = new bool[width, height];
                int curWidth = chessBoards[i].GetLength(0);
                int curHeight = chessBoards[i].GetLength(1);
                for (int w = 0; w < Mathf.Min(width, curWidth); w++)
                {
                    for(int h = 0; h < Mathf.Min(height, curHeight); h++)
                    {
                        board[w, h] = chessBoards[i][w, h];
                    }
                }
                newChessBoards.Add(board);
            }
            chessBoards = newChessBoards;
        }
        if (Application.isPlaying)
        {
            AnimalSpawner.Instance.RefreshObj(this);
        }
    }
}
