using UnityEngine;
using Sirenix.OdinInspector;

public class LogHelper : MonoBehaviour
{
    private static LogHelper instance;
    public static LogHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LogHelper>();
                if (instance == null)
                {
                    instance = new GameObject("_" + typeof(LogHelper).Name).AddComponent<LogHelper>();
                    DontDestroyOnLoad(instance);
                }
            }
            return instance;
        }
        set { }
    }

    [Title("预定义打印")] 
    public Color TypeColor_Default = Color.gray;
    public Color TypeColor_Warning = Color.yellow;
    public Color TypeColor_Error = Color.red;
    private string ColorString_Default;
    private string ColorString_Warning;
    private string ColorString_Error;

    [Title("自定义打印")]
    public bool Print = true;
    public Color TypeColor_Test = Color.blue;
    private string ColorString_Test;

    void Awake()
    {
        ColorString_Test = ColorUtility.ToHtmlStringRGB(TypeColor_Test);
        // 预定义
        ColorString_Default = ColorUtility.ToHtmlStringRGB(TypeColor_Default);
        ColorString_Warning = ColorUtility.ToHtmlStringRGB(TypeColor_Warning);
        ColorString_Error = ColorUtility.ToHtmlStringRGB(TypeColor_Error);
    }

    static string GetColor(string type)
    {
        switch(type)
        {
            case "Test":
                return Instance.ColorString_Test;
            // 预定义
            case "Log":
                return Instance.ColorString_Default;
            case "Warning":
                return Instance.ColorString_Warning;
            case "Error":
                return Instance.ColorString_Error;
            default:
                return Instance.ColorString_Default;
        }
    }

    public static void Info(string type="Log", params object[] args)
    {
        if (Instance.Print)
            Debug.Log($"<color=#{GetColor(type)}>[{type}]</color> {string.Join(",", args)}");
    }
    
    public static void Warning(string type="Warning", params object[] args)
    {
        if (Instance.Print)
            Debug.LogWarning($"<color=#{GetColor(type)}>[{type}]</color> {string.Join(",", args)}");
    }

    public static void Error(string type="Error", params object[] args)
    {
        if (Instance.Print)
            Debug.LogError($"<color=#{GetColor(type)}>[{type}]</color> {string.Join(",", args)}");
    }
}