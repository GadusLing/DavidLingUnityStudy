using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lesson16 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TMP_Dropdown dd = GetComponent<TMPro.TMP_Dropdown>();
        print(dd.value);

        print(dd.options[dd.value].text);

        dd.options.Add(new TMP_Dropdown.OptionData("test A"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
