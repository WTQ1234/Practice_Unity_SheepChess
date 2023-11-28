#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
//using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using HRL;

public class ChapterEditorWindow : BasicConfigWindow
{
    private static string mFileName_Chapter = "Chapter[{0}]";
    private static string mFileName_Section = "Section[{0}]";
    private static string mAssetPath_Chapter = "Assets/Resources/ScriptableObject/ChapterInfo";
    private static string mAssetPath_Section = "Assets/Resources/ScriptableObject/SectionInfo";
    private static string mTitle_AllAssets_Chapter = "1.所有关卡";
    private static string mTitle_AllAssets_Section = "2.所有章节";

    //[Title("聊天对话数据")]
    //[ShowInInspector, ReadOnly]
    //public static DialogueDatabase selectedDatabase;

    [MenuItem("配置/主流程/关卡配置")]
    private static void Open()
    {
        var window = GetWindow<ChapterEditorWindow>();
        // 设置标题
        GUIContent titleContent = new GUIContent();
        titleContent.text = "属性配置";
        window.titleContent = titleContent;
        // 设置位置
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        // 添加基础配置
        if (!AssetPath.ContainsKey("属性路径"))
        {
            AssetPath.Add("默认数据名", mFileName_Chapter);
            AssetPath.Add("属性路径", mAssetPath_Chapter);
            AssetPath.Add("对话数据名", "HRLDialogueDatabase");
        }
        //selectedDatabase = Resources.Load<DialogueDatabase>("HRLDialogueDatabase");
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        base.BuildMenuTree();
        // 浏览当前所有属性
        ChapterOverview.Instance.UpdateOverview();
        // 将具体属性添加到列表
        if (ChapterOverview.Instance.AllInfos.Length > 0)
        {
            mCurTree.Add(mTitle_AllAssets_Chapter, new BasicInfoTable<ChapterInfo>(ChapterOverview.Instance.AllInfos));
            mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Chapter, mAssetPath_Chapter, typeof(ChapterInfo), true, true);
        }
        SectionOverview.Instance.UpdateOverview();
        // 将具体属性添加到列表
        if (SectionOverview.Instance.AllInfos.Length > 0)
        {
            mCurTree.Add(mTitle_AllAssets_Section, new BasicInfoTable<SectionInfo>(SectionOverview.Instance.AllInfos));
            mCurTree.AddAllAssetsAtPath(mTitle_AllAssets_Section, mAssetPath_Section, typeof(SectionInfo), true, true);
        }
        // 后续处理
        AfterCreateBuildMenuTree();
        return mCurTree;
    }

    protected override void OnBeginDrawEditors()
    {
        if (this.MenuTree == null)
        {
            return;
        }
        var selected = this.MenuTree?.Selection?.FirstOrDefault();
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
        // 绘制工具栏
        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        {
            if (selected != null)
            {
                GUILayout.Label(selected.Name);
            }
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("选中当前文件")))
            {
                SelectCurAssetFile();
            }
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建新章节")))
            {
                int assetNumber = FindAssetNumber(mAssetPath_Section, mFileName_Section);
                Debug.Log(assetNumber);
                string curFileName = string.Format(mFileName_Section, assetNumber);
                ScriptableObjectCreator.ShowDialog<SectionInfo>(mAssetPath_Section, curFileName, (obj, fileName) =>
                {
                    obj.Id = assetNumber;
                    obj.Name = obj.name;
                    obj.FileName = fileName;
                    obj.InitAfterCreateFile();
                    base.TrySelectMenuItemWithObject(obj);
                });
            }
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建新关卡")))
            {
                int assetNumber = FindAssetNumber(mAssetPath_Chapter, mFileName_Chapter);
                string curFileName = string.Format(mFileName_Chapter, assetNumber);
                ScriptableObjectCreator.ShowDialog<ChapterInfo>(mAssetPath_Chapter, curFileName, (obj, fileName) =>
                {
                    obj.Id = assetNumber;
                    obj.Name = obj.name;
                    obj.FileName = fileName;
                    obj.InitAfterCreateFile();
                    base.TrySelectMenuItemWithObject(obj);
                });
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
}
#endif
