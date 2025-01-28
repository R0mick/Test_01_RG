using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DarkBackground : MonoBehaviour,IPointerClickHandler
{
    public PopupWindowScript popupWindowScript;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        popupWindowScript.GetComponent<PopupWindowScript>().HidePopup();
    }
}
