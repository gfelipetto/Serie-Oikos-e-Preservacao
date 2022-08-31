using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipShowBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string textToShow;

    public void OnPointerEnter ( PointerEventData eventData ) {
        TooltipBehavior.ShowTooltip_Static(textToShow);

    }
    public void OnPointerExit ( PointerEventData eventData ) {
        TooltipBehavior.HideTooltip_Static();
    }
    private void OnMouseEnter ( ) {
        TooltipBehavior.ShowTooltip_Static(textToShow);

    }

    private void OnMouseExit ( ) {
        TooltipBehavior.HideTooltip_Static();
    }

}

