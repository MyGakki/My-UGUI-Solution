using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCubeByMosueDown : MonoBehaviour
{

    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (flag)
            GetComponent<MeshRenderer>().material.color = Color.white;
        else
            GetComponent<MeshRenderer>().material.color = Color.black;
        flag = !flag;
    }
}
