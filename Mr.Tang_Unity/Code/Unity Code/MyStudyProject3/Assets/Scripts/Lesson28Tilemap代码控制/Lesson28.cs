using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lesson28 : MonoBehaviour
{
    public Tilemap map; // 瓦片地图信息，可以通过它来得到瓦片格子
    public Grid grid; // 格子网格信息，可以通过它来转换坐标
    public TileBase tileBase; // 瓦片基类，可以通过它来得得到瓦片资源
    void Start()
    {
        //map.ClearAllTiles(); // 清除所有瓦片

        TileBase tmp = map.GetTile(new Vector3Int(0, 0, 0)); // 获取坐标(0,0,0)处的瓦片信息
        print(tmp.name); // 打印瓦片名称

        map.SetTile(new Vector3Int(5, 5, 5), tileBase); // 设置坐标(5,5,5)处的瓦片
        map.SetTile(new Vector3Int(0, 0, 0), null); // 删除(设置为空)坐标(0,0,0)处的瓦片

        map.SetTiles(new Vector3Int[] { new Vector3Int(1, 1, 1), new Vector3Int(2, 2, 2) },
                      new TileBase[] { tileBase, tileBase }); // 批量设置瓦片
        
        map.SwapTile(tmp, tileBase); // 将所有tmp这个类型的瓦片替换为tileBase这个瓦片

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main; // 获取主摄像机
            Vector3 screenPos = Input.mousePosition; // 获取鼠标在屏幕的坐标
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos); // 将屏幕坐标转换为世界坐标

            Vector3Int cellPos = grid.WorldToCell(worldPos); // 将世界坐标转换为格子坐标

            //操作格子坐标
            map.SetTile(cellPos, tileBase); // 在鼠标点击位置设置瓦片
        }
    }
}
