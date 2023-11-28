using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using HRL;

[SerializeField]
public class SectionInfo : BasicInfo
{
    [Title("标题")]
    public string Title;
    [Title("关卡数")]
    public List<ChapterInfo> ChapterInfos;
}
