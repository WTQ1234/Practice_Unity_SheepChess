using System.Linq;
using System.Data;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[GlobalConfig("Assets/Resources/ScriptableObject/EditorOverviews")]
public class ChapterOverview : GlobalConfig<ChapterOverview>
{
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    public ChapterInfo[] AllInfos;

    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void UpdateOverview()
    {
        // Finds and assigns all scriptable objects of type Character
        this.AllInfos = AssetDatabase.FindAssets("t:ChapterInfo")
            .Select(guid => AssetDatabase.LoadAssetAtPath<ChapterInfo>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
    }
}
