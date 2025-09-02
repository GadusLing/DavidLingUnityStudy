using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person
{
    public string name;
    public int age;
    public bool sex; // true表示男性，false表示女性
    public float height;

    public List<string> hobbies; // 兴趣爱好列表

    public Person()
    {
        name = "";
        age = 0;
        sex = true;
        height = 0f;
        hobbies = new List<string> { "羽毛球" , "游泳" , "阅读" };
    }

    public Person(string name, int age, bool sex, float height)
    {
        this.name = name;
        this.age = age;
        this.sex = sex;
        this.height = height;
        this.hobbies = new List<string> { "羽毛球" , "游泳" , "阅读" };
    }

    public override string ToString()
    {
        return $"Person: {name}, Age: {age}, Sex: {(sex ? "男" : "女")}, Height: {height}, Hobbies: {string.Join(", ", hobbies)}";
    }
}