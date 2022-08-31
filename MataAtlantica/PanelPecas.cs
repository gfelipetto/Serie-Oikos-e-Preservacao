using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPecas : MonoBehaviour {

    private int nPecasConectadas = 0;
    public static int nPecasCorretas = 0;


    public PieceController_MeuCharGame[] pecas;

    private void Awake() {
        pecas = FindObjectsOfType<PieceController_MeuCharGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        nPecasCorretas = 0;

        if (collision.CompareTag("Peça")) {
            PieceController_MeuCharGame peca = collision.GetComponent<PieceController_MeuCharGame>();

            if (peca.estaArrastando) {
                nPecasConectadas++;
                peca.estaConectado = true;
                if (nPecasConectadas > 1) {
                    foreach (PieceController_MeuCharGame p in pecas) {
                        if (p.estaConectado && p.meuGrupo == peca.meuGrupo && GameController_CriandoChar.ave == peca.meuGrupo) {
                            nPecasCorretas++;
                        }
                    }
                }

            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Peça")) {
            nPecasConectadas--;
            collision.GetComponent<PieceController_MeuCharGame>().estaConectado = false;
        }
        
    }

}
