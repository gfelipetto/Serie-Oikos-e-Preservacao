using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        

        if (GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            Check();
        }

    }

    private void Check() {
                
        if (Input.GetMouseButton(0)  && GameManager.currentPiece != null) {

            if (GameManager.currentPiece.GetComponent<PieceScript>().endPosition == transform.position) {

                //GameManager.currentPiece.transform.position = Vector2.MoveTowards(GameManager.currentPiece.transform.position, transform.position, Time.deltaTime * 20f);
                
                GameManager.currentPiece.transform.position = transform.position;
                GameManager.currentPiece.GetComponent<SpriteRenderer>().sortingOrder = 0;
                Destroy(GameManager.currentPiece.GetComponent<PieceScript>());
                GameManager.currentPiece = null;
                GameManager.currentScore++;
                Destroy(gameObject);
            } 
            else {
                if (Input.GetMouseButtonUp(0)) {
                    GameManager.currentPiece.GetComponent<PieceScript>().cancelPiece = true;
                }

            }
        }
        
    }
}
