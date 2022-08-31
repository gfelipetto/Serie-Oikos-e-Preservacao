using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMangerGerencia : MonoBehaviour {

    [SerializeField] private List<SpawnButtonBehavior> buttons;
    [SerializeField] private float timeToWin = 60f;


    public GameObject[] enemeysPrefabs;
    public bool win, gameOver;
    public static bool inGame;
    public TextMeshProUGUI textTime;
    public GameObject winObj, gameOverObj;
    private void Awake ( ) {
        inGame = true;
        StartCoroutine(CheckButtons());
        int r = Random.Range(0, 3);

        for (int i = 0; i < 4; i++) {
            Instantiate(enemeysPrefabs[r]);

        }
        if (r == 2) {
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[1]);
            }
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[0]);
            }
        } else if (r == 1) {
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[2]);
            }
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[0]);
            }
        } else {
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[2]);
            }
            for (int i = 0; i < Random.Range(1, 3); i++) {
                Instantiate(enemeysPrefabs[1]);
            }
        }
    }
    private void FixedUpdate ( ) {
        if (inGame) {
            textTime.text = $"Tempo: {timeToWin.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}";

        }

    }

    private IEnumerator CheckButtons ( ) {
        bool decrease, increase;
        List<SpawnButtonBehavior> tmpDecreaseTimerList, tmpIncreaseTimerList;
        //print("entrou");
        yield return new WaitForEndOfFrame();
        while (inGame) {
            tmpDecreaseTimerList = buttons.FindAll(x => x.decreaseTimer == true);
            tmpIncreaseTimerList = buttons.FindAll(x => x.increaseTimer == true);
            yield return new WaitForEndOfFrame();
            decrease = tmpDecreaseTimerList.Count == 3;
            increase = tmpIncreaseTimerList.Count == 3 || tmpIncreaseTimerList.Count == 1;
            if (decrease) {
                StartCoroutine(TimerControl(false));
                yield return new WaitForEndOfFrame();
            }
            if (increase) {
                StartCoroutine(TimerControl(true));
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator TimerControl ( bool op ) {

        if (!op) {
            if (timeToWin >= 0f) {
                timeToWin -= Time.fixedDeltaTime;
                Color cor = textTime.color;
                cor.r -= Time.fixedDeltaTime * 0.01f;
                cor.g -= Time.fixedDeltaTime * 0.01f;
                cor.b += Time.fixedDeltaTime * 0.01f;
                textTime.color = cor;
                if (timeToWin <= 0f) {
                    win = true;
                    winObj.SetActive(true);
                    inGame = false;

                }
            }

        } else {
            if (timeToWin <= 60f + (60f * .50f)) {
                timeToWin += Time.fixedDeltaTime;
                Color cor = textTime.color;
                cor.r += Time.fixedDeltaTime * 0.01f;
                cor.g -= Time.fixedDeltaTime * 0.01f;
                cor.b -= Time.fixedDeltaTime * 0.01f;
                textTime.color = cor;
                if (timeToWin >= 60f + (60f * .50f)) {
                    gameOver = true;
                    gameOverObj.SetActive(true);
                    inGame = false;
                }

            }

        }
        yield return new WaitForFixedUpdate();
    }


}
