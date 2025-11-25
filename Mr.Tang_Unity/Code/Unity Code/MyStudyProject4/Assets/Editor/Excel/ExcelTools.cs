using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 入口按钮 + 遍历文件 + 调用解析
/// </summary>
public static class ExcelTools
{
    /// <summary>
    /// 文件目录
    /// </summary>
    public static string Excel_Path = "Assets/Excel";
    /// <summary>
    /// 文件夹输出目录
    /// </summary>
    public static string Excel_Out_Path = "Assets/StreamingAssets";
    /// <summary>
    /// 文件后缀
    /// </summary>
    public static string[] Excel_Suffix = new string[] { ".xlsx" };
    /// <summary>
    /// 文件列表
    /// </summary>
    private static List<string> excelList;

    // Encoding encoding=Encoding.GetEncoding("utf-8");

    /// <summary>
	/// 加载Excel
	/// </summary>
    [MenuItem("Tools/ExcelTools", false, 4)] // 在 Unity 菜单栏创建一个按钮 Tools → ExcelTools 
                                             //点击它会执行 LoadExcel()
    private static void LoadExcel()
    {
        if (excelList == null) excelList = new List<string>();
        excelList.Clear();
        // 清理旧的文件
        GameUtility.SafeClearDir(Excel_Out_Path);// 用工具类把原有 txt/json 全部删掉，保证新的产出是干净的
        string[] allfils = GameUtility.GetSpecifyFilesInFolder(Excel_Path, Excel_Suffix);// 获取所有 Excel 文件
        for (int i = 0; i < allfils.Length; i++)
        {
            excelList.Add(allfils[i]);
        }
        Convert();
    }

    /// <summary>
	/// 转换Excel文件
	/// </summary>
	private static void Convert()
    {
        foreach (string assetsPath in excelList)
        {
            //获取Excel文件的绝对路径
            string excelPath = assetsPath;
            //构造Excel工具类
            ExcelUtility excel = new ExcelUtility(excelPath);
            excel.ConvertToJson(Excel_Out_Path, Encoding.GetEncoding("utf-8"));
            //刷新本地资源
            AssetDatabase.Refresh();
        }
    }


}
