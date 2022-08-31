using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CropTextures : MonoBehaviour {


    private Texture2D sourceTexture;
    private List<Vector2> positions = new List<Vector2>();
    private List<Vector2> sortedPositions = new List<Vector2>();
    private Vector2 resolutionPieces, position, distancePieces;
    public int amountPieces = 3;

    public static string ave;

    public GameObject piecePrefab, gridPrefab;
    public Image img;
    public Image bio_Ave;
    public GameObject ninho;



    // Use this for initialization
    void Start() {
        StartComponents();
        CreatePositions();
        CreatePiece();
    }

    private void StartComponents() {

        sourceTexture = RandomTexture(); // randomiza imagem do quebra-cabeças
        img.sprite = Sprite.Create(sourceTexture, new Rect(0, 0, sourceTexture.width, sourceTexture.height), new Vector2(0.5f, 0.5f));
        //amountPieces = (int) GridType;
        resolutionPieces = new Vector2(sourceTexture.width / amountPieces,
                                        sourceTexture.height / amountPieces);
        GameManager.currentScore = 0;
        GameManager.scoreTotal = amountPieces * amountPieces;


    }


    private Texture2D CropTexture(int row, int line) {
        var resolutionX = Mathf.RoundToInt(resolutionPieces.x);
        var resolutionY = Mathf.RoundToInt(resolutionPieces.y);

        Color[] pixels = sourceTexture.GetPixels(row * resolutionX, line * resolutionY,
                                                    resolutionX, resolutionY);
        Texture2D texture = new Texture2D(resolutionX, resolutionY);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
    

    private void CreatePositions() {
        distancePieces = new Vector2(resolutionPieces.x / 100.0f, resolutionPieces.y / 100.0f);
        for (int i = 0;i < amountPieces;i++) {
            for (int j = 0;j < amountPieces;j++) {
                positions.Add(new Vector2(i * distancePieces.x+10f, j * distancePieces.y - 5f));
            }
        }
    }
    private Vector2 RandomPosition() {
        bool sorted = false;
        Vector2 pos = Vector2.zero;

        while (!sorted) {
            pos = positions[Random.Range(0, positions.Count)];
            sorted = !sortedPositions.Contains(pos);
            if (sorted) {
                sortedPositions.Add(pos);
            }
        }
        return pos;
    }

    private void CreatePiece() {
        var start = amountPieces - 1;
        for (int i = start;i >= 0;i--) {
            for (int j = 0;j < amountPieces;j++) {

                var texture = CropTexture(j, i);
                position = RandomPosition();
                var quad = Instantiate(piecePrefab, position, Quaternion.identity) as GameObject;
                quad.GetComponent<SpriteRenderer>().sprite =
                    Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                quad.GetComponent<BoxCollider2D>().size =
                    new Vector2(distancePieces.x, distancePieces.y);
                quad.GetComponent<PieceScript>().startPosition = position;
                quad.transform.parent = GameObject.FindGameObjectWithTag("Finish").transform;
                CreateGrid(j, i, quad);




            }
        }
    }

    private void CreateGrid(int j, int i, GameObject quad) {
        var grid = Instantiate(gridPrefab,
                                new Vector2(( j * distancePieces.x ) - 10f, i * distancePieces.y - 5f),
                                Quaternion.identity) as GameObject;
        grid.transform.parent = GameObject.FindGameObjectWithTag("Finish").transform;
        var newScale = new Vector2(resolutionPieces.x / 150f, resolutionPieces.y / 150f);
        grid.transform.localScale = new Vector3(newScale.x, newScale.y, 0);
        quad.GetComponent<PieceScript>().endPosition = grid.transform.position;
    }

    private Texture2D RandomTexture() {
        Texture2D[] imagens;
        imagens = Resources.LoadAll<Texture2D>("Imagens_pampa/");
        int rand = Random.Range(0, imagens.Length);
        bio_Ave.sprite = Resources.Load<Sprite>("Final/"+ imagens[rand].name);
        ninho.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Ninho/" + imagens[rand].name);
        ave = imagens[rand].name;

        return imagens[rand];
    }

}
