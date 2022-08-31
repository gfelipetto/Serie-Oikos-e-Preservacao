using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TooltipBehavior : MonoBehaviour {

    public static TooltipBehavior Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;

    private void Awake ( ) {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(gameObject); }
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        HideTooltip();
    }
    private void Update ( ) {
        if (!this.gameObject.activeSelf) { return; }

        Vector2 anchoredPos = Input.mousePosition / canvasRectTransform.localScale.x;

        //if (anchoredPos.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
        //    anchoredPos.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        //}

        //if (anchoredPos.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
        //    anchoredPos.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        //}
        rectTransform.anchoredPosition = anchoredPos;
    }

    private void SetText ( string tooltipText ) {
        textMeshPro.SetText(tooltipText);

        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void ShoowTooltip ( string tooltipText ) {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }
    private void HideTooltip ( ) {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static ( string tooltipText ) {
        byte[] bytes = Encoding.Default.GetBytes(tooltipText);
        tooltipText = Encoding.UTF8.GetString(bytes);
        Instance.ShoowTooltip(tooltipText);
    }

    public static void HideTooltip_Static ( ) {
        Instance.HideTooltip();
    }
}
