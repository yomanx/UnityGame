using Consts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    Camera uiCamera;
    Canvas uiCanvas;
    EventSystem uiEventSystem;
    List<UIBase> stackUI = new List<UIBase>();
    UIBase topUI;


    public UIBase GetTopUI() => topUI;

    public override void Init()
    {
        base.Init();

        GameObject _obj;

        _obj = new GameObject("UICamera");
        uiCamera = _obj.AddComponent<Camera>();
        uiCamera.transform.SetParent(transform);
        uiCamera.cullingMask = LayerMask.GetMask("UI");
        uiCamera.clearFlags = CameraClearFlags.Depth;
        uiCamera.orthographic = true;

        _obj = new GameObject("UICanvas");
        uiCanvas = _obj.AddComponent<Canvas>();
        _obj.AddComponent<CanvasScaler>();
        _obj.AddComponent<GraphicRaycaster>();
        uiCanvas.transform.SetParent(transform);
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //uiCanvas.worldCamera = uiCamera;

        _obj = new GameObject("UIEventSystem");
        uiEventSystem = _obj.AddComponent<EventSystem>();
        _obj.AddComponent<StandaloneInputModule>();
        uiEventSystem.transform.SetParent(transform);

    }
    

    public UIBase GetUI(string name)
    {
        foreach(UIBase ui in stackUI)
        {
            if (ui.name == name)
                return ui;
        }

        return null;
    }

    public UIBase GetUI(System.Type type)
    {
        foreach (UIBase ui in stackUI)
        {
            if (ui.GetType() == type)
                return ui;
        }

        return null;
    }

    public T Show<T>() where T : UIBase
    {
        T ui = stackUI.Find(x => x.GetType() == typeof(T))?.GetComponent<T>();
        if (ui != null)
            return ui;
        ui = CreateNewUI<T>(typeof(T).ToString());

        return ui;
    }

    private T CreateNewUI<T>(string name) where T : UIBase
    {
        var prefab = AssetDatabase.LoadAssetAtPath(string.Format("Assets/Resources/UI/{0}.prefab", name), typeof(T));
        if (prefab == null)
            return null;
        GameObject _obj = Instantiate(prefab, uiCanvas.transform) as GameObject;
        if (_obj == null)
            return null;

        T uiObject = _obj.GetComponent<T>();

        stackUI.Add(uiObject);

        return uiObject;
    }
}
