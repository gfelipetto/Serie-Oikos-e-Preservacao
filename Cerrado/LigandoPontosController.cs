using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LigandoPontosController : MonoBehaviour {

    //public List<GameObject> pieces;
    //public List<GameObject> grid;
    private string gridAnswer, pieceAnswer;

    [Header("My Events:")]
    [SerializeField] private UnityEvent OnAnswer;

    [Header("Piece Settings:")]
    [Range(3, 10)]
    public int _tentative;

    [Header("Runtime:")]
    public GameObject[] spawnObjs;
    public GameObject gridDragged;
    public GameObject pieceTarget;
    public int countObjCorrect;

    public bool isTimeOfSubtract;
    public bool gameOver;
    public bool isWin;

    [Header("Scene Layers:")]
    public LayerMask layerGrid;
    public LayerMask layerPiece;

    [Header("UI:")]
    public TextMeshProUGUI tentativeTxt;
    public GameObject gameOverUI;
    public TextMeshProUGUI finishTxt_TMP;
    public Button btnAvancar;

    


    private void Awake ( ) {
        DrawController.count = 0;
        countObjCorrect = 0;
        isTimeOfSubtract = false;
        gameOver = false;
        tentativeTxt.text = $"Tentativas: {_tentative}";

    }
    private void Start ( ) {
        RandomSpawn();
        
    }

    private void Update ( ) {
        GetPiece();
        GetGrid();
        if (!isWin && !gameOver) {
            CheckAnswer(gridAnswer, pieceAnswer);

        }
    }

    private void RandomSpawn ( ) {
        int rand = Random.Range(0, spawnObjs.Length);
        spawnObjs[rand].SetActive(true);

    }

    private void GetPiece ( ) {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerPiece);
        if (hit.collider != null) {
            if (hit.collider.tag == "Peça") {
                pieceTarget = hit.collider.gameObject;
                pieceAnswer = hit.collider.name;
            }

        } else {
            pieceAnswer = " ";
            pieceTarget = null;
            if (gridDragged != null) {
                PieceController_Cerrado grid = gridDragged.GetComponent<PieceController_Cerrado>();
                grid.isCorrect = false;
            }
        }
    }

    private void GetGrid ( ) {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerGrid);
        if (hit.collider != null) {
            if (hit.collider.tag == "Grid") {
                if (Input.GetMouseButtonDown(0)) {
                    gridDragged = hit.collider.gameObject;
                    gridAnswer = hit.collider.name;

                } else if (Input.GetMouseButtonUp(0)) {
                    gridAnswer = "";
                    gridDragged = null;
                }
            }

        }
    }

    private void CheckAnswer ( string s1, string s2 ) {

        if (s1 == s2) {
            if (gridDragged != null) {
                if (gridDragged.GetComponent<PieceController_Cerrado>()) {
                    PieceController_Cerrado grid = gridDragged.GetComponent<PieceController_Cerrado>();
                    grid.isCorrect = true;
                }
            }
            
        } else {
            if (gridDragged != null) {
                if (pieceTarget != null) {
                    if (gridDragged.GetComponent<PieceController_Cerrado>()) {
                        PieceController_Cerrado grid = gridDragged.GetComponent<PieceController_Cerrado>();
                        grid.isCorrect = false;
                    }
                }

            }

        }
        if (Input.GetMouseButtonUp(0)) {
            CheckEndGame();
        }
        if (isTimeOfSubtract) {
            OnAnswer.Invoke();
            isTimeOfSubtract = false;
        }

        
        if (gameOver) {
            GameOver(1);
            //gameOver = false;
        }

    }

    private void CheckEndGame ( ) {
        List<GameObject> o = GameObject.FindGameObjectsWithTag("Grid").ToList();
        Dictionary<GameObject, bool> objs = new Dictionary<GameObject, bool>();

        foreach (GameObject item in o) {
            bool _isCorrect = item.GetComponent<PieceController_Cerrado>().isCorrect;
            if (_isCorrect && !objs.ContainsKey(item)) {
                print(item.name);
                objs.Add(item, _isCorrect);

            }
        }
        //print(countObjCorrect);
        countObjCorrect = objs.Count;
        if (countObjCorrect == o.Count ) { // se ganhar
            isWin = true;
            TooltipBehavior.HideTooltip_Static();
            GameOver(0);
            btnAvancar.gameObject.SetActive(true);
        }

    }
    private void GameOver ( int op ) {
        if (op == 0) {

            finishTxt_TMP.text = "Você conseguiu salvar o Cerrado!";
            AudioManager.instance.Play("Win");
        } else {
            AudioManager.instance.Play("Lose");
            gameOverUI.SetActive(true);
            OnAnswer.Invoke();
        }
        finishTxt_TMP.gameObject.SetActive(true);

        //voltarBtn.SetActive(false);
        foreach (GameObject item in spawnObjs) {
            item.SetActive(false);

        }
        List<GameObject> obj = GameObject.FindGameObjectsWithTag("Grid").ToList();
        foreach (GameObject item in obj) {
            item.SetActive(false);
        }
        obj.Clear();
        obj = GameObject.FindGameObjectsWithTag("Brush").ToList();
        foreach (GameObject item in obj) {
            item.SetActive(false);
        }
    }
    public void SubstractTentative ( ) {
        if(_tentative > 0) {
            _tentative -= 1;
            tentativeTxt.text = $"Tentativas: {_tentative}";
            if (_tentative <= 0) {
                gameOver = true;
            }
        }
        
    }

}
