using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.UI;

public class LuaCustomLoader : LuaFileUtils
{
    public override byte[] ReadFile(string fileName)
    {
        // //在这里实现自定义的加载逻辑
        // //例如，可以根据自己的规则来定位Lua脚本文件的位置
        //Debug.Log("使用自定义加载器加载Lua文件" + fileName);
        
        // Unity不论从AB包还是Resources中加载文件 都不支持.Lua后缀，所以tolua在copy文件时自动加上了bytes后缀
        // 我们自己定义规则时可以加上.txt后缀 方便后面打AB包时识别
        if(!fileName.EndsWith(".lua"))// 这里加.lua是因为 require 语法通常写成 require("XXX/Lesson2_Loader")，传到ReadFile时，fileName 一般是没有后缀的
        {
            fileName += ".lua";
        }

        byte[] buffer = null;
        // 因为进行热更新的lua代码肯定是业务代码 优先从AB包中加载
        // 第二种 从AB包中加载
        // 我们传入的可能是"CSharpCallLua/Lesson2_Loader"这样的文件名 需要把它拆分一下
        string[] strs = fileName.Split('/'); // 把 fileName 按照 / 字符切割成字符串数组
        TextAsset luaCode = ABManager.Instance.LoadRes<TextAsset>("lua", strs[strs.Length - 1]); // 参数1是AB包名 参数2是文件名
        if (luaCode != null)
        {
            buffer = luaCode.bytes;
            Resources.UnloadAsset(luaCode); // 卸载资源 避免内存泄漏
        }

        // 而tolua自带的代码和lua类我们不需要去热更 都是放在Resources下的
        if(buffer == null) // 如果 AB 包里没有，说明是基础代码，就从 Resources 里找
        {
            // 第一种 从Resources文件夹加载
            string path = "Lua/" + fileName; // 假设Lua脚本都放在Resources/Lua目录下
            TextAsset text = Resources.Load<TextAsset>(path);
            if (text != null)
            {
                buffer = text.bytes;
                Resources.UnloadAsset(text); // 卸载资源 避免内存泄漏
            }
        }
        return buffer;
    }
}
