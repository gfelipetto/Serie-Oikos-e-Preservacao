using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class PieceController_Cerrado : MonoBehaviour {
    private Vector2 screenBounds;
    private float objectHeight;
    private float objectWidth;
    private Vector3 startPosition;
    private LigandoPontosController _gameManager;
    //private DissolveStep dissolve;

    private bool flag;

    public bool isFocus;
    public bool isDragging;
    public bool canMove;
    public bool isCorrect;
    public bool cancelPiece;

    public string answer;

    //public static int count = 0;

    private void Awake ( ) {
        
        _gameManager = FindObjectOfType<LigandoPontosController>();
        startPosition = this.transform.position;
        this.name = answer;
        cancelPiece = isCorrect = isFocus = isDragging = flag = false;
        canMove = true;
        this.screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        this.objectWidth = this.transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2 
        this.objectHeight = this.transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

    }
    private void LateUpdate ( ) {
        
        KeepInScreen();
    }
    private void Update ( ) {
        if (isFocus) {
            flag = true;
        } else {
            flag = false;
        }

        if (!isCorrect) {
            if (cancelPiece) {
                GoBackFirstPos();
            }
        } else if (!isDragging && flag) {
            GoToCorrectPos();
        }


    }

    private void OnMouseEnter ( ) {
        isFocus = true;

    }
    private void OnMouseExit ( ) {
        isFocus = false;
    }

    private void OnMouseUp ( ) {
        isDragging = false;
        SpriteRenderer mySpriteRender = this.GetComponent<SpriteRenderer>();
        //mySpriteRender.sortingOrder = 1;
        if (!isCorrect) {
            cancelPiece = true;
            canMove = false;
        }

    }
    private void OnMouseDrag ( ) {
        if (canMove) {
            isDragging = true;
            SpriteRenderer mySpriteRender = this.GetComponent<SpriteRenderer>();
            mySpriteRender.sortingOrder = 2;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = mousePosition;

        }
    }
    private void KeepInScreen ( ) {
        Vector3 viewPos = this.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        this.transform.position = viewPos;
    }
    /// <summary>
    /// É chamado quando o jogador erra a posição da peça.
    /// </summary>
    void GoBackFirstPos ( ) {
        this.transform.position = Vector2.MoveTowards(this.transform.position, startPosition, Time.deltaTime * 15f);
        if (this.transform.position == startPosition) {
            cancelPiece = false;
            canMove = true;
            _gameManager.isTimeOfSubtract = true;


        }
    }
    /// <summary>
    /// É chamado quando o jogador acerta a posição da peça.
    /// </summary>
    public void GoToCorrectPos ( ) {

        if (_gameManager.pieceTarget != null) {

            Vector3 finalPosition = _gameManager.pieceTarget.transform.position;
            this.transform.position = Vector2.MoveTowards(this.transform.position, finalPosition, Time.deltaTime * 2f);
            if (this.transform.position == finalPosition) {
                _gameManager.pieceTarget = null;
            }
        }

    }

}


