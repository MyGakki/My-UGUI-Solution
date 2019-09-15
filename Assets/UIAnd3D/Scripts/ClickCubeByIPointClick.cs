using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCubeByIPointClick : MonoBehaviour, IPointerClickHandler
{

    private bool flag = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (flag)
            GetComponent<MeshRenderer>().material.color = Color.white;
        else
            GetComponent<MeshRenderer>().material.color = Color.black;
        flag = !flag;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
