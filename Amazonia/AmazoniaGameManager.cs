using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmazoniaGameManager : MonoBehaviour {

    [SerializeField] private GameObject finishGameObject;
    GameObject obj;
    bool canInstantiate = true;
    AmazoniaPlayerConfig[] players;
    public static Vector2 viewBounds;
    //public static float vel = 2;

    public int objectSpawnCount = 0;
    //public GameObject[] objects;

    public Transform objectTrigger;
    public ObjectSpawner spawner;
    public TextMeshProUGUI text;
    public TextMeshProUGUI textWinner;
    public static bool inGame = false;

    public List<GameObject> gameObjectToEnd;

    [Space(10), Range(30f, 240f)]
    public float timer = 60f;

    private void Awake ( ) {
        viewBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    private void FixedUpdate ( ) {
        if (inGame) {
            GameUpdate();
            if (timer <= 0f) {
                FinishGame();
            }
        }
    }

    public void StartGame ( ) {
        inGame = true;

        StartCoroutine(TimerCountdown());
    }

    private void FinishGame ( ) {
        inGame = false;
        players = FindObjectsOfType<AmazoniaPlayerConfig>();
        if (players[0].transform.GetComponent<AmazoniaPlayerMove>().isActiveAndEnabled && players[0].objectCount > players[1].objectCount ||
            players[1].transform.GetComponent<AmazoniaPlayerMove>().isActiveAndEnabled && players[1].objectCount > players[0].objectCount) {
            AudioManager.instance.Play("Win");
            foreach (GameObject item in gameObjectToEnd) {
                item.SetActive(false);
            }
        } else {
            foreach (GameObject item in gameObjectToEnd) {
                item.SetActive(false);
            }
            AudioManager.instance.Play("Lose");
        }
        textWinner.text = players[0].objectCount > players[1].objectCount ? $"{players[0].name} ganhou!" : $"{players[1].name} ganhou!";
        finishGameObject.SetActive(true);


    }

    private void GameUpdate ( ) {
        objectTrigger.transform.position = new Vector3(0, Mathf.PingPong(Time.time, 4), 1);
        //canInstantiate = (obj != null && obj.transform.position.y < objectTrigger.position.y) && inGame;
        if ((obj != null && obj.transform.position.y < objectTrigger.position.y)) {
            canInstantiate = true;
        }
        if (canInstantiate) {
            objectSpawnCount++;
            canInstantiate = false;
            spawner.Spawn();
            obj = spawner.newObject;
        }
    }

    private IEnumerator TimerCountdown ( ) {
        while (inGame) {
            timer -= Time.fixedDeltaTime;
            text.text = $"Tempo: {timer.ToString("F0", System.Globalization.CultureInfo.InvariantCulture)}";
            yield return new WaitForFixedUpdate();
        }
    }
}
