using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson9 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RawImage raw = GetComponent<RawImage>();
        raw.texture = Resources.Load<Texture>("bk");
        raw.uvRect = new Rect(0, 0, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
