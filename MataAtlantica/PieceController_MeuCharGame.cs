using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PieceController_MeuCharGame : MonoBehaviour {
    private Vector2 screenBounds;
    private float objectHeight;
    private float objectWidth;
    private Vector3 posDestino;
    private Vector3 vecDirecao;
    private Rigidbody2D _meuRigidBody2D;

    public bool flag = false;
    public bool wasPositionChanged;

    public Vector3 posInicial;
    public Vector3 posAtual;

    public bool estaArrastando;
    public bool estaConectado;
    public bool cancelPiece;
    public bool estaCerto;

    public AvesGrupoController meuGrupo;

    [Range(0.1f, 2f)]
    public float distanciaMinimaConector = 0.5f;

    void Start ( ) {
        cancelPiece = false;
        posInicial = this.transform.position;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        _meuRigidBody2D = transform.root.GetComponent<Rigidbody2D>();
        _meuRigidBody2D.gravityScale = 0;
        _meuRigidBody2D.angularDrag = 50f;
        _meuRigidBody2D.drag = 50f;
        this.screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        this.objectWidth = this.transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2 
        this.objectHeight = this.transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }


    private void LateUpdate ( ) {
        KeepInScreen();

        if (!estaConectado && !estaArrastando && !GameController_CriandoChar.gameCriandoCharDone && !estaCerto) {
            GoToFirstPos();
        }

    }

    private void OnMouseDown ( ) {
        posAtual = transform.root.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        estaArrastando = true;
        estaConectado = false;
    }

    private void OnMouseDrag ( ) {
        posDestino = posAtual + Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vecDirecao = posDestino - transform.root.position;
        _meuRigidBody2D.velocity = vecDirecao * 50f;

    }

    private void OnMouseUp ( ) {
        estaArrastando = false;
    }
    private void OnMouseOver ( ) {
        if (Input.GetMouseButton(1)) {
            Vector2.MoveTowards(transform.root.position, posInicial, 0.02f);
        }
    }

    void GoToFirstPos ( ) {
        this.transform.position = Vector2.MoveTowards(this.transform.position, posInicial, Time.deltaTime * 15f);
        if (this.transform.position == posInicial) {
            cancelPiece = false;
        }
    }
    private void KeepInScreen ( ) {
        Vector3 viewPos = this.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        this.transform.position = viewPos;
    }

}
