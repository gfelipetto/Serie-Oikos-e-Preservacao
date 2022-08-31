using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController_CriandoChar : MonoBehaviour {

    public static bool gameCriandoCharDone;

    public GameObject UiPanel;
    public GameObject UIPanelPecas;
    public Button btnConcluido;
    public Button btnFinalizar;
    public Button btnReset;
    public LumberjackController lenhador;

    public TextMeshProUGUI txtCaracteristicas;
    public TextMeshProUGUI txtFinal;

    [Header("Pecas"),Space(10)]
    public List<PieceController_MeuCharGame> pieceList;
    public List<AvesGrupoController> grupos;
    public static AvesGrupoController ave;

    private void Awake ( ) {
        pieceList = FindObjectsOfType<PieceController_MeuCharGame>().ToList();
        MisturaPecas(pieceList);
        ave = SorteiaGrupo();
        txtCaracteristicas.text = "Dicas: \n"+ ave.caracteristicas.Replace("1","\n");


    }
    private void LateUpdate ( ) {
        if (LumberjackController.lumberjackWin) {
            TiraPecas();
            gameCriandoCharDone = true;
            lenhador.gameObject.SetActive(false);
            UiPanel.SetActive(false);
            UIPanelPecas.SetActive(false);
            btnConcluido.gameObject.SetActive(false);
            btnReset.gameObject.SetActive(true);
            txtFinal.text = "Voçê Perdeu!";
            txtFinal.gameObject.SetActive(true);
            AudioManager.instance.Play("Lose");
        }
    }

    private AvesGrupoController SorteiaGrupo ( ) {
        return grupos[Random.Range(0, grupos.Count)];

    }

    private void MisturaPecas(List<PieceController_MeuCharGame> _pieceList ) {
        int randomTree = Random.Range(0, _pieceList.Count);
       
        foreach (PieceController_MeuCharGame p in _pieceList) {
            if (p.wasPositionChanged) continue;
            
            while (_pieceList[randomTree].wasPositionChanged && _pieceList[randomTree] != p) {
                randomTree = Random.Range(0, _pieceList.Count);
            }
            Vector3 thisPos = p.transform.position;
            p.transform.position = _pieceList[randomTree].transform.position;
            _pieceList[randomTree].transform.position = thisPos;
            p.wasPositionChanged = true;
            _pieceList[randomTree].wasPositionChanged = true;
        }
    }

    private void ChecaGame ( ) {
        if (!LumberjackController.lumberjackWin) {
            if (PanelPecas.nPecasCorretas == 3) {
                Debug.Log("Jogador Ganhou Criando Char");
                TiraPecas();
                Instantiate(ave.objCompleto);
                //foreach (PieceController_MeuCharGame item in pieceList) {
                //    item.GetComponent<SliderJoint2D>().enabled = true;
                //}
                gameCriandoCharDone = true;
                txtCaracteristicas.gameObject.SetActive(false);
                lenhador.gameObject.SetActive(false);
                UiPanel.SetActive(false);
                UIPanelPecas.SetActive(false);
                btnConcluido.gameObject.SetActive(false);
                btnFinalizar.gameObject.SetActive(true);
                txtFinal.gameObject.SetActive(true);
                AudioManager.instance.Play("Win");
            } 
        } 
    }

    private void TiraPecas() {
        GameObject[] pecas;
        pecas = GameObject.FindGameObjectsWithTag("Peça");
        pieceList.Clear();
        foreach (GameObject i in pecas) {
            i.SetActive(false);
            //if (!i.GetComponent<PieceController_MeuCharGame>().estaConectado) {
            //    i.SetActive(false);
               
            //} else {
            //    if (i.GetComponent<SliderJoint2D>()) 
            //        pieceList.Add(i.GetComponent<PieceController_MeuCharGame>());
                
            //    continue;
            //}

        }
        
    }



    


    
}

