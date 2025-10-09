using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel<T> : MonoBehaviour where T : BasePanel<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        instance = this as T;
    }
    protected virtual void Start()
    {
        Init();
    }

    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }

    public abstract void Init();
}
