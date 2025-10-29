using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMgr
{
    //1.写一个工具类，让我们可以更加方便的加载Multiple类型的图集资源
    //2.用我提供的角色资源，制作一个通过wasd键控制其上下左右移动的功能
    private static MultipleMgr _instance = new MultipleMgr();
    public static MultipleMgr Instance => _instance;
    // 为了把加载的资源存储起来方便下次使用，我们使用一个嵌套的字典
    private Dictionary<string, Dictionary<string, Sprite>> multipleDict = new Dictionary<string, Dictionary<string, Sprite>>();
    private MultipleMgr() { }

    /// <summary>
    /// 获取Multiple图集中的指定Sprite
    /// </summary>
    /// <param name="multipleName">图集名称</param>
    /// <param name="spriteName">单张精灵名称</param>
    public Sprite GetSprite(string multipleName, string spriteName)
    {
        if (multipleDict.TryGetValue(multipleName, out Dictionary<string, Sprite> spriteDict))
        {
            if (spriteDict.TryGetValue(spriteName, out Sprite sprite))
            {
                return sprite;
            }
        }
        else
        {
            // 如果图集还没有加载过，则加载它
            Sprite[] sprites = Resources.LoadAll<Sprite>(multipleName);
            Dictionary<string, Sprite> tmpSpriteDict = new Dictionary<string, Sprite>();
            foreach (Sprite s in sprites)
            {
                tmpSpriteDict[s.name] = s;
            }
            multipleDict[multipleName] = tmpSpriteDict;
            // 递归调用以获取Sprite
            return GetSprite(multipleName, spriteName);
        }
        return null;
    }

    public void ClearInfo()
    {
        multipleDict.Clear();
        Resources.UnloadUnusedAssets();
    }
}
