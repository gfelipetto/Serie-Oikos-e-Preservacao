using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour {

    private SpriteRenderer sprite;
    private float timeToLerp = 20;


    [HideInInspector]
    public Vector3 startPosition, endPosition;
    [HideInInspector]
    public bool canMove = false, cancelPiece = false;


	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove) {
            sprite.sortingOrder = 1;
            Vector3 mouseP = Input.mousePosition;
            mouseP.z = transform.position.z - Camera.main.transform.position.z;
            transform.position = Camera.main.ScreenToWorldPoint(mouseP);
        }
        

        if (cancelPiece) {
            CancelPiece();
        }
	}

    private void OnMouseOver() {
        if(Time.timeScale == 1) {
            if (Input.GetMouseButtonDown(0) &&
                !cancelPiece &&
                GameManager.currentPiece == null) {

                GameManager.currentPiece = gameObject;
                canMove = true;
            }
            //if (Input.GetMouseButtonDown(1) &&
            //    !cancelPiece &&
            //    canMove) {

            //    cancelPiece = true;
            //}
            if (Input.GetMouseButtonUp(0) && !cancelPiece && canMove) {
                cancelPiece = true;
            }

        }

    }

    private void CancelPiece() {
        GameManager.currentPiece = null;
        transform.position = Vector2.MoveTowards(transform.position, startPosition, Time.deltaTime * timeToLerp);
        canMove = false;
        if (transform.position == startPosition) {
            sprite.sortingOrder = 0;
            cancelPiece = false;

        }
        
    }
}
