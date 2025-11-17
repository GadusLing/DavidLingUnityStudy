using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lesson11 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.isOn = true;

        ToggleGroup toggleGroup = GetComponentInParent<ToggleGroup>();
        toggleGroup.allowSwitchOff = false;

        foreach(Toggle t in toggleGroup.ActiveToggles())
        {
            Debug.Log(t.name);
        }

        toggle.onValueChanged.AddListener(OnToggleChanged1);
        toggle.onValueChanged.AddListener((b) => {
            Debug.Log("Lambda Toggle state changed to: " + b);
        });

    }

    public void OnToggleChanged(bool isOn)
    {
        Debug.Log("Toggle is " + (isOn ? "On" : "Off"));
    }

    private void OnToggleChanged1(bool isOn)
    {
        Debug.Log("Toggle state changed to: " + isOn);
    }
}
