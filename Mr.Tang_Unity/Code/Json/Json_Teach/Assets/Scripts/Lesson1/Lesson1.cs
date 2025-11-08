using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class customClass
{
    public int id;
    public string name;
}

public class Student
{
    public string name;
    public int age;
    public bool sex;
    public float height;
    public double weight;
    public int[] ids;
    public List<int> ids2;
    public List<string> hobbys;
    public Dictionary<int, string> dic1;
    public Dictionary<string, string> dic2;
    public customClass custom1;
    public List<customClass> custom2;
    [SerializeField] // 使用SerializeField特性使私有字段被序列化
    private string privateInfo; // 私有字段，不会被序列化
    [SerializeField] // 使用SerializeField特性使受保护字段被序列化
    protected string protectedInfo; // 受保护字段，不会被序列化

}

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //使用File类进行文件的读写操作
        //第一个参数填写的是存储的路径
        //第二个参数填写的是存储的字符串内容
        //注意：第一个参数必须是存在的路径，如果没有对应文件夹会报错
        File.WriteAllText(Application.persistentDataPath + "/Json/lesson1.json", "Lesson1测试json");

        string str = File.ReadAllText(Application.persistentDataPath + "/Json/lesson1.json");
        Debug.Log(str);

        // 使用JsonUtility进行Json的序列化和反序列化操作
        Student student = new Student();
        student.name = "张三";
        student.age = 18;
        student.sex = true;
        student.height = 1.75f;
        student.weight = 70.5;
        student.ids = new int[] { 1001, 1002, 1003 };
        student.ids2 = new List<int>() { 2001, 2002, 2003 };
        student.hobbys = new List<string>() { "篮球", "足球", "羽毛球" };
        student.dic1 = new Dictionary<int, string>()
        {
            {1, "语文" },
            {2, "数学" },
            {3, "英语" }
        };
        student.dic2 = new Dictionary<string, string>()
        {
            {"A", "优秀" },
            {"B", "良好" },
            {"C", "及格" }
        };
        student.custom1 = new customClass() { id = 1, name = "自定义类1" };
        student.custom2 = new List<customClass>()
        {
            new customClass() { id = 2, name = "自定义类2" },
            new customClass() { id = 3, name = "自定义类3" }
        };
        string jsonStr = JsonUtility.ToJson(student);
        File.WriteAllText(Application.persistentDataPath + "/Json/student.json", jsonStr);
        //注意：
        //1.float序列化时看起来会有一些误差
        //2.自定义类需要加上序列化特性[System.Serializable]
        //3.想要序列化私有变量需要加上特性[SerializeField]
        //4.JsonUtility不支持字典
        //5.JsonUtility存储null对象不会是null而是默认值的数据

        // 读取Json文件并反序列化为Student对象
        string studentJsonStr = File.ReadAllText(Application.persistentDataPath + "/Json/student.json");
        // Student student2 = JsonUtility.FromJson(studentJsonStr， typeof(Student)) as Student; // 另一种写法
        Student student2 = JsonUtility.FromJson<Student>(studentJsonStr);


        // 注意：
        // 1.JsonUtility无法直接读取数据集合[{},{}]这样的数据 需要外部有一个对象 然后用{}包裹,例如：{"data":[{},{}]}
        // 2.文本编码格式需要时UTF-8不然无法加载


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
