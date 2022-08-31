using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class AmazoniaPlayerConfig : MonoBehaviour {

    [SerializeField] TextMeshProUGUI text;
    
    private Collider2D collider2D;

    public int objectCount = 0;
    private void OnEnable ( ) {
        StartCoroutine(UpdateScoreText());
    }

    private void OnDisable ( ) {
        StopAllCoroutines();
    }

    private void Start ( ) {
        collider2D = this.gameObject.GetComponent<Collider2D>();
        GameObject[] playerToAvoid = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject item in playerToAvoid) {
            if (item.name == this.name) continue;

            Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), collider2D);
        }
        

    }

    private IEnumerator UpdateScoreText ( ) {
        while (AmazoniaGameManager.inGame) {
            if(this.GetComponent<AmazoniaPlayerMove>().enabled)
                text.text = $"Jogador: {objectCount}";
            else
                text.text = $"PC: {objectCount}";

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision ) {

        if (collision.CompareTag("ObjetoAmazonia")) {
            objectCount++;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

}
