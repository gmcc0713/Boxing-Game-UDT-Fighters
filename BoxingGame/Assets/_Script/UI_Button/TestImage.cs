using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestImage : MonoBehaviour
{
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        //image = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            image.SetActive(false);
        }
    }
}
