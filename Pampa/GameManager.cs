using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private bool tratorMovimentando = true;
    private bool gameOverSong = false;

    public static GameObject currentPiece;
    public static int currentScore, scoreTotal;

    public GameObject fimObj;
    public Button btnAvancar;

    public Toggle verImagem;
    public Toggle gearObject;


    public Image verImg;
    public Image bioAve;
    public Image background;
    public GameObject trator;
    public GameObject ninho;
    public Image aveFinal;

    public TextMeshProUGUI textFinal;

    [Range(2, 10), Space(10)]
    public float tempoDeLoad;
    [Range(0.01f, 0.5f), Space(10)]
    public float velocidadeTrator;

    [Range(1, 10), Space(10)]
    public float tempoDeLeitura = 5f;
    void FixedUpdate ( ) {
        if (trator.transform.position.x >= 0 && trator.transform.position.x < 1) {
            trator.GetComponent<AudioSource>().Play();
        }
        if (currentScore == scoreTotal) {

            GameObject[] grid = GameObject.FindGameObjectsWithTag("Grid");
            GameObject[] peca = GameObject.FindGameObjectsWithTag("Peça");
            foreach (GameObject gridObj in grid) {
                gridObj.gameObject.SetActive(false);
            }
            foreach (GameObject pecaObj in peca) {
                pecaObj.gameObject.SetActive(false);
            }
            aveFinal.gameObject.SetActive(true);
            aveFinal.sprite = Resources.Load<Sprite>("Imagens_pampa_normal/" + CropTextures.ave);
            trator.gameObject.SetActive(false);
            verImg.gameObject.SetActive(false);

            verImagem.interactable = false;
            gearObject.interactable = false;

            textFinal.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            bioAve.gameObject.SetActive(true);
            btnAvancar.gameObject.SetActive(true);
            tratorMovimentando = false;
            AudioManager.instance.Play("Win");

        }

        if (tratorMovimentando) {
            background.fillAmount -= 0.00028f;
            Vector3 tratorPos = trator.transform.position;
            tratorPos.x += velocidadeTrator;
            trator.transform.position = tratorPos;
        }



        if (trator.transform.position.x > 45f) {
            tratorMovimentando = false;
            GameObject[] grid = GameObject.FindGameObjectsWithTag("Grid");
            GameObject[] peca = GameObject.FindGameObjectsWithTag("Peça");
            foreach (GameObject gridObj in grid) {
                gridObj.gameObject.SetActive(false);
            }
            foreach (GameObject pecaObj in peca) {
                pecaObj.gameObject.SetActive(false);
            }
            //textTime.text = "Tempo Restante: 0";
            textFinal.text = "Tempo Esgotado!\nPampa Destruido\nDeseja Continuar ?";
            AudioManager.instance.Play("Lose");
            ninho.SetActive(false);
            gearObject.interactable = false;
            verImagem.interactable = false;
            textFinal.gameObject.SetActive(true);
            fimObj.SetActive(true);
            background.gameObject.SetActive(true);


        }
    }

}
