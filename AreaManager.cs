using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour {

    //private FadeController fade;
    private SpriteRenderer sprite;
    private LevelLoader levelLoader;
    private bool flag;
    [SerializeField] private float defaultValueAlpha;
    [SerializeField] private bool pulseAtivo;

    public TextMeshProUGUI txtLegenda;
    public string sceneToLoad;
    

    void Start ( ) {
        levelLoader = FindObjectOfType<LevelLoader>();
        flag = false;
        pulseAtivo = false;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        defaultValueAlpha = sprite.color.a;

    }

    private void OnMouseEnter ( ) {
        pulseAtivo = true;
        StartCoroutine(ActivePulse());
    }
    private void OnMouseExit ( ) {
        txtLegenda.text = null;
        pulseAtivo = false;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, defaultValueAlpha);
    }

    private void OnMouseDown ( ) {
        //fade.PlayAnimation(triggerAnimation);
        AudioManager.instance.Play("UI");
        levelLoader.LoadScene(sceneToLoad);
    }

    private void OnMouseOver ( ) {
        txtLegenda.text = this.transform.name;
    }

    public IEnumerator ActivePulse ( ) {
        while (pulseAtivo) {

            if (sprite.color.a >= 0.5f) {
                flag = false;


            }
            if (sprite.color.a <= 0.09f) {
                flag = true;


            }

            if (flag) {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 0.0065f);
            } else {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.0065f);
            }
            yield return new WaitForFixedUpdate();

        }
    }
}
