using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单例模板
public class Singleton<T> where T : Singleton<T>, new()
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    private static T instance;
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = attached?.GetComponent<T>();
            }
            return instance;
        }
    }

    private static T instance;
    private static GameObject attached;

    protected virtual void Awake()
    {
        instance = (T)this;
    }

    public static void Ensure()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject(typeof(T).Name + "_Instance");
            instance = obj.AddComponent(typeof(T)) as T;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}