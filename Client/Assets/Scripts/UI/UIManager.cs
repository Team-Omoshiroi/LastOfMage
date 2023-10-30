using System;
using System.Collections.Generic;
using UnityEngine;
using static Google.Protobuf.Reflection.FeatureSet.Types;


public class UIManager
{
    private static UIManager _instance;
    /// <summary>
    /// null체크를 하고 없으면 생성하여 리턴함
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    private Dictionary<Type, GameObject> _prefabs;
    private LinkedList<UIBase> OpenList;
    private LinkedList<UIBase> HideList;

    public float UISize;// { get => GameManager.Data.UISize; }
    public float FontSize;// { get => GameManager.Data.FontSizeMultiplier; }
    public float UIRemainTime;// { get => GameManager.Data.UIRemainTime; }

    private string _prefabPath = "Prefabs/UI/";

    private Canvas _canvas;
    public Canvas UIRoot
    {
        get
        {
            if (_canvas == null)
                if (GameObject.Find("_UI").TryGetComponent(out _canvas))
                    return _canvas;
                else
                    return null;
            return _canvas;
        }
        set
        {
            _canvas = value;
        }
    }

    public UIManager()
    {
        _prefabs = new Dictionary<Type, GameObject>();
        OpenList = new LinkedList<UIBase>();
        HideList = new LinkedList<UIBase>();
        //LoadUIPrefabs();
    }
    /// <summary>
    /// Open 리스트의 첫번째에 위치한 UI를 Hide하며, 이미 Hide된 경우에는 아무것도 하지 않음
    /// </summary>
    public static void HideTopUI()
    {
        if (Instance.OpenList.Count > 0)
        {
            HideUI(Instance.OpenList.First.Value);
        }
    }
    /// <summary>
    /// 해당 스크립트가 붙여진 프리펩을 불러옴, Hide된 게임오브젝트가 있는 경우에는 해당 게임오브젝트를 리턴함
    /// </summary>
    /// <typeparam name="T">프리펩에 붙어있는 클래스</typeparam>
    /// <param name="root">부모 canvas/UI를 의미함</param>
    /// <returns>해당 프리펩이 없으면 null을 리턴함</returns>
    public static T ShowUI<T>(RectTransform root = null) where T : UIBase
    {
        var open = GetHideUI<T>();
        if (open != null)
        {
            Instance.HideList.Remove(open);
            if (root == null)
                open.transform.SetParent(Instance.UIRoot.transform);
            else
                open.transform.SetParent(root);

            open.gameObject.SetActive(true);
            return open;
        }

        if (!Instance._prefabs.ContainsKey(typeof(T)))
            Instance.LoadUIPrefab(typeof(T).Name);

        var prefab = Instance._prefabs[typeof(T)];
        if (prefab != null)
        {
            GameObject obj;
            if (root == null)
                obj = GameObject.Instantiate(prefab, Instance.UIRoot.transform);
            else
                obj = GameObject.Instantiate(prefab, root);
            var uiClass = obj.GetComponent<UIBase>();

            Instance.OpenList.AddFirst(uiClass);

            obj.SetActive(true);
            return uiClass as T;
        }
        else
            return null;
    }
    /// <summary>
    /// 게임 오브젝트를 삭제
    /// </summary>
    public static void CloseUI<T>(T target) where T : UIBase
    {
        if (IsHide(target))
        {
            target.CloseUI();
            Instance.OpenList.Remove(target);
            Instance.HideList.Remove(target);
        }
        else
        {
            target.CloseUI();
            Instance.OpenList.Remove(target);
        }
    }
    /// <summary>
    /// 게임 오브젝트를 비활성화
    /// </summary>
    public static void HideUI<T>(T target) where T : UIBase
    {
        if (!IsHide(target))
        {
            target.HideUI();
            Instance.HideList.AddLast(target);
            Instance.OpenList.Remove(target);
            Instance.OpenList.AddLast(target);
        }
    }
    /// <summary>
    /// 해당 UI가 Open List에 있는지 확인하는 메소드로, Hide 유무는 알려주지 않는다.
    /// 활성화 상태는 activeInHierarchy를 보면 알 수 있고, 사용하기 위해서는 ShowUI(eUIType type)를 부르면 된다.
    /// </summary>
    /// <returns>찾을 수 없으면 null을 리턴함</returns>
    public static T GetOpenUI<T>(T search) where T : UIBase
    {
        foreach (var ui in Instance.OpenList)
        {
            if (ui == search)
                return ui as T;
        }
        return null;
    }
    /// <summary>
    /// 열린 해당 UI Type이 Open List에 있는지 확인하는 메소드로, Hide 유무는 알려주지 않는다.
    /// 활성화 상태는 activeInHierarchy를 보면 알 수 있고, 사용하기 위해서는 ShowUI(eUIType type)를 부르면 된다.
    /// </summary>
    /// <returns>찾을 수 없으면 null을 리턴함</returns>
    public static T GetOpenUI<T>() where T : UIBase
    {
        LinkedListNode<UIBase> ui = Instance.OpenList.First;
        while (ui != null)
        {
            if (ui.Value is T)
                return ui.Value as T;
            ui = ui.Next;
        }
        return null;
    }
    /// <summary>
    /// 해당 UI가 Hide List에 있는지 확인하는 메소드
    /// </summary>
    /// <returns>찾을 수 없으면 null을 리턴함</returns>
    public static T GetHideUI<T>(T search) where T : UIBase
    {
        foreach (var ui in Instance.HideList)
        {
            if (ui == search)
                return ui as T;
        }
        return null;
    }
    /// <summary>
    /// Hide된 해당 UI Type이 Hide 리스트에 있는지 확인하는 메소드
    /// </summary>
    /// <returns>찾을 수 없으면 null을 리턴함</returns>
    public static T GetHideUI<T>() where T : UIBase
    {
        LinkedListNode<UIBase> ui = Instance.HideList.First;
        while (ui != null)
        {
            if (ui.Value is T)
                return ui.Value as T;
            ui = ui.Next;
        }
        return null;
    }
    /// <summary>
    /// Open List에 있는 모든 UI 게임오브젝트를 삭제한다.
    /// </summary>
    public static void CloseAllOpenUI()
    {
        foreach (var ui in Instance.OpenList)
        {
            ui.CloseUI();
        }
        Instance.OpenList.Clear();
        Instance.HideList.Clear();
    }
    /// <summary>
    /// Hide List에 있는 모든 UI 게임오브젝트를 삭제한다.
    /// </summary>
    public static void CloseAllHideUI()
    {
        foreach (var ui in Instance.HideList)
        {
            ui.CloseUI();
            Instance.OpenList.Remove(ui);
        }
        Instance.HideList.Clear();
    }
    /// <summary>
    /// 해당 UI type이 Open List에 포함되어 있나 확인한다. IsHide가 경우에 따라 더 유용하다.
    /// </summary>
    public static bool IsOpen<T>() where T : UIBase
    {
        foreach (var ui in Instance.OpenList)
        {
            if (ui is T)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 해당 UI가 Open List에 포함되어 있나 확인한다.
    /// </summary>
    public static bool IsOpen<T>(T target) where T : UIBase
    {
        foreach (var ui in Instance.OpenList)
        {
            if (ui == target)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 해당 UI type이 Hide List에 포함되어 있나 확인한다.
    /// </summary>
    public static bool IsHide<T>() where T : UIBase
    {
        foreach (var ui in Instance.HideList)
        {
            if (ui is T)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Hide List에 포함되어 있나 확인한다.
    /// </summary>
    public static bool IsHide<T>(T target) where T : UIBase
    {
        foreach (var ui in Instance.HideList)
        {
            if (ui == target)
                return true;
        }
        return false;
    }

    private void LoadAllUIPrefabs()
    {
        var objs = Resources.LoadAll<GameObject>(_prefabPath);
        foreach (var obj in objs)
        {
            var type = obj.GetComponent<UIBase>().GetType();
            _prefabs.Add(type, obj);
            Debug.Log($"{type}({_prefabPath}/{obj.name}) is loaded.");
        }
    }

    private void LoadUIPrefab(string name)
    {
        var obj = Resources.Load<GameObject>(_prefabPath + name);
        if (obj != null)
        {
            var type = obj.GetComponent<UIBase>().GetType();
            _prefabs.Add(type, obj);
            Debug.Log($"{type}({_prefabPath}/{obj.name}) is loaded.");
        }
    }
}
