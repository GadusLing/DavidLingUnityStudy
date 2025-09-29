using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    private static SceneMgr _instance = new SceneMgr();
    public static SceneMgr Instance => _instance;

    private SceneMgr(){}

    public void LoadScene(string name, UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        ao.completed += (a)=>
        {
            action?.Invoke();
        };

    }

}
