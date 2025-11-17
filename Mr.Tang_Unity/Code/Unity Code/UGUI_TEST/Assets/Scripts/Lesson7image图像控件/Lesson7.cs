using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson7 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("bk");
        (transform as RectTransform).sizeDelta = new Vector2(200, 200);
        image.raycastTarget = false;

        image.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
