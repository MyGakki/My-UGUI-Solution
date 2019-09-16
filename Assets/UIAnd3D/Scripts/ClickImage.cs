using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickImage : MonoBehaviour, IPointerClickHandler
{

    private bool index = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (index)
            GetComponent<Image>().color = Color.white;
        else
            GetComponent<Image>().color = Color.red;
        index = !index;
        ExecuteAll(eventData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExecuteAll(PointerEventData eventData)
    {
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);

        foreach (RaycastResult result in list)
        {
            if(result.gameObject != gameObject)
            {
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
