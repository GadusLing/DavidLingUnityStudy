using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeNamePanel : MonoBehaviour
{

    public static ChangeNamePanel panel;
    public TMP_InputField inputName;
    public Button btnSure;

    void Awake()
    {
        panel = this;
    }

    void Start()
    {
        btnSure.onClick.AddListener(()=>
        {
            GamePanel.panel.txtName.text = inputName.text;
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
