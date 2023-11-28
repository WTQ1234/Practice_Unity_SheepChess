using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace HRL
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] Canvas canvas;
        [SerializeField] Canvas canvas_top;
        public List<UIScreen> UIScreens = new List<UIScreen>();
        public Dictionary<string, UIScreen> Dict_UIScreens = new Dictionary<string, UIScreen>();

        #region Push
        public T PushScreen<T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            T screen = null;
            if (Dict_UIScreens.ContainsKey(uiName))
            {
                _PushScreen(Dict_UIScreens[uiName]);
                screen = (T)Dict_UIScreens[uiName];
            }
            else
            {
                UIScreen prefab = Resources.Load<UIScreen>($"Prefabs/UI/{uiName}");
                if (prefab != null)
                {
                    UIScreen obj = Instantiate(prefab, prefab.GetScreenInfo().mTopDialog ? canvas_top.transform : canvas.transform);
                    _PushScreen(obj);
                    screen = (T)obj;
                }
            }
            Debug.Log($"Push Screen {screen}");
            return screen;
        }

        public void PushScreen(UIScreen uiScreen)
        {
            _PushScreen(uiScreen);
        }

        private void _PushScreen(UIScreen uiScreen)
        {
            string uiName = uiScreen.UIName;
            if (!Dict_UIScreens.ContainsKey(uiName))
            {
                Dict_UIScreens.Add(uiName, uiScreen);
            }
            else
            {
                Dict_UIScreens[uiName] = uiScreen;
            }

            // 根据 screeninfo 判定是否要隐藏上一个界面
            if (uiScreen.GetScreenInfo().mHideLastDialog)
            {
                if (UIScreens.Count > 0)
                {
                    UIScreens[UIScreens.Count - 1].OnHide();
                }
            }
            UIScreens.Add(uiScreen);
            uiScreen.OnShown();
        }
        #endregion

        #region Pop
        // 暂时不做销毁，UI界面不多，直接隐藏
        public void PopScreen<T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            if (Dict_UIScreens.TryGetValue(uiName, out UIScreen uiScreen))
            {
                if (!UIScreens.Contains(uiScreen))
                {
                    return;
                }
                UIScreen screen = Dict_UIScreens[uiName];
                Debug.Log($"Pop Screen {screen}");
                screen.OnHide();
                UIScreens.Remove(screen);
                if (UIScreens.Count > 0)
                {
                    UIScreens[UIScreens.Count - 1].OnShown();
                }
            }
        }

        public void PopScreen(UIScreen uiScreen)
        {
            if (!UIScreens.Contains(uiScreen))
            {
                return;
            }
            uiScreen.OnHide();
            UIScreens.Remove(uiScreen);
            if (UIScreens.Count > 0)
            {
                UIScreens[UIScreens.Count - 1].OnShown();
            }
        }

        // 清除所有，用于返回主界面
        public void PopAllScreen()
        {
            foreach (var screen in UIScreens)
            {
                Debug.Log($"Pop Screen {screen}");
                screen.OnHide();
            }
            UIScreens.Clear();
        }
        #endregion

        public UIScreen GetTopScreen()
        {
            if (UIScreens.Count > 0)
            {
                return UIScreens[UIScreens.Count - 1];
            }
            return null;
        }

        public T GetScreen
            <T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            Debug.Log(uiName);
            foreach (UIScreen screen in UIScreens)
            {
                Debug.Log(screen.UIName);
                if (screen.UIName == uiName)
                {
                    return (T)screen;
                }
            }
            return null;
        }
    }
}