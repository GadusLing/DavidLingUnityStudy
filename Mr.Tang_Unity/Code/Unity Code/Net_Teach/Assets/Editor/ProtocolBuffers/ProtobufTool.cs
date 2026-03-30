using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProtobufTool
{
    private static string PROTO_PATH = "E:\\GithubDownLoad\\DavidLingUnityStudy\\Mr.Tang_Unity\\Code\\Unity Code\\Net_Teach\\Protobuf\\proto"; // 协议配置文件夹路径
    private static string PROTOC_PATH = "E:\\GithubDownLoad\\DavidLingUnityStudy\\Mr.Tang_Unity\\Code\\Unity Code\\Net_Teach\\Protobuf\\protoc.exe"; // protoc.exe编译器路径
    private static string CSHARP_PATH = "E:\\GithubDownLoad\\DavidLingUnityStudy\\Mr.Tang_Unity\\Code\\Unity Code\\Net_Teach\\Protobuf\\csharp"; // 生成的代码文件保存路径
    private static string CPP_PATH = "E:\\GithubDownLoad\\DavidLingUnityStudy\\Mr.Tang_Unity\\Code\\Unity Code\\Net_Teach\\Protobuf\\cpp"; // 生成的代码文件保存路径
    private static string JAVA_PATH = "E:\\GithubDownLoad\\DavidLingUnityStudy\\Mr.Tang_Unity\\Code\\Unity Code\\Net_Teach\\Protobuf\\java"; // 生成的代码文件保存路径

    [MenuItem("ProtobufTool/生成C#代码")]
    private static void GenerateCSharp()
    {
        Generate("csharp_out", CSHARP_PATH);
    }

    [MenuItem("ProtobufTool/生成C++代码")]
    private static void GenerateCCpp()
    {
        Generate("cpp_out", CPP_PATH);
    }

    [MenuItem("ProtobufTool/生成JAVA代码")]
    private static void GenerateJava()
    {
        Generate("java_out", JAVA_PATH);
    }




    private static void Generate(string outCmd, string outPath)
    {
        UnityEngine.Debug.Log(outCmd + "代码生成中..."); // 输出日志，提示代码生成中...

        //1.遍历协议配置文件夹得到所有的.proto文件
        DirectoryInfo directoryInfo = Directory.CreateDirectory(PROTO_PATH); // 创建目录对象
        FileInfo[] files = directoryInfo.GetFiles("*.proto", SearchOption.AllDirectories); // 获取目录下所有的.proto文件 递归搜索子目录
        foreach (FileInfo file in files) // 遍历所有的.proto文件
        {

            Process cmd = new Process(); // 创建进程对象
            cmd.StartInfo.FileName = PROTOC_PATH; // 设置要执行的程序为protoc.exe
            cmd.StartInfo.Arguments = $"-I=\"{PROTO_PATH}\" --{outCmd}=\"{outPath}\" \"{file.Name}\"";// 路径中包含空格时需要使用引号括起来，格式为 -I="配置路径" --csharp_out="输出路径" "配置文件名"
            
            cmd.StartInfo.WorkingDirectory = PROTO_PATH; // 让 protoc.exe 在 proto 目录下执行，这样命令行参数里的 proto 文件名（如 "test.proto"）就能被正确找到，不需要写绝对路径
            cmd.StartInfo.UseShellExecute = false; // 不使用 Shell 必须设为 false，才能重定向标准输出（StandardOutput）和标准错误（StandardError），否则后面两句会报错
            cmd.StartInfo.RedirectStandardOutput = true; // 重定向标准输出，这样我们就可以通过 cmd.StandardOutput.ReadToEnd() 来获取 protoc.exe 的输出内容，通常是生成成功的提示信息
            cmd.StartInfo.RedirectStandardError = true; // 重定向标准错误，这样我们就可以通过 cmd.StandardError.ReadToEnd() 来获取 protoc.exe 的错误信息，如果生成失败，这里会有错误提示
            cmd.StartInfo.CreateNoWindow = true;  // 不创建新窗口，这样执行 protoc.exe 时不会弹出黑色命令行窗口，保持界面整洁

            cmd.Start(); // 启动进程，执行 protoc.exe 进行代码生成
            string output = cmd.StandardOutput.ReadToEnd(); // 读取 protoc.exe 的输出内容，通常是生成成功的提示信息
            string error = cmd.StandardError.ReadToEnd(); // 读取 protoc.exe 的错误信息，如果生成失败，这里会有错误提示
            
            cmd.WaitForExit(); // 等待 protoc.exe 执行完成，确保生成过程结束后再继续执行后续代码
            if (!string.IsNullOrEmpty(output)) // 如果 protoc.exe 有输出内容，打印出来，通常是生成成功的提示信息
                UnityEngine.Debug.Log("protoc输出: " + output);
            if (!string.IsNullOrEmpty(error)) // 如果 protoc.exe 有错误信息，打印出来，通常是生成失败的错误提示
                UnityEngine.Debug.LogError("protoc错误: " + error);

            UnityEngine.Debug.Log(file.Name + " 生成结束");

        }
        UnityEngine.Debug.Log("所有" + outCmd + "代码生成完成！"); // 输出日志，提示所有C#代码生成完成
    }

}
