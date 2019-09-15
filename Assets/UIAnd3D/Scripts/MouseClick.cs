using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{

    private bool flag = false;
    private GraphicRaycaster _raycast;

    // Start is called before the first frame update
    void Start()
    {
        _raycast = FindObjectOfType<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsUI())
        {
            if (flag)
                GetComponent<MeshRenderer>().material.color = Color.white;
            else
                GetComponent<MeshRenderer>().material.color = Color.black;
            flag = !flag;
        }
    }

    bool IsUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        _raycast.Raycast(eventData, list);
        return list.Count > 0;
    }
}
