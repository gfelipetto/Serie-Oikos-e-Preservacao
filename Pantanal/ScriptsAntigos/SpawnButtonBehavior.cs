using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonBehavior : MonoBehaviour {
    private Button me;

    [SerializeField] private Image myGauge;
    [SerializeField] private EnemyAI myPrefabEnemyAI;
    public List<EnemyAI> mySpawns;
    [SerializeField, Space(5), Range(0f, 1f)] private float valueOfGauge;
    [Space(5), Range(4, 8)] public int maxSpawns;

    public AquaticosController myGroup;
    public bool decreaseTimer, increaseTimer;
    private void Start ( ) {
        maxSpawns = 4;
        me = GetComponent<Button>();

        mySpawns = FindObjectsOfType<EnemyAI>().Where(x => x.meuGrupo == this.myGroup).ToList();
    }
    // Update is called once per frame
    void Update ( ) {
        valueOfGauge = (float)mySpawns.Count / maxSpawns;
        myGauge.fillAmount = valueOfGauge;
        if (mySpawns.Count < maxSpawns) {
            me.interactable = true;
        } else {
            me.interactable = false;
        }

        if (myGauge.fillAmount == 1f || myGauge.fillAmount == 0f) {
            increaseTimer = true;
        } else {
            increaseTimer = false;
        }

        if (myGauge.fillAmount == 0.5f) {
            decreaseTimer = true;
        } else {
            decreaseTimer = false;
        }
    }
    private void LateUpdate ( ) {
        
    }
    public void SpawnEnemy ( ) {
        if (mySpawns.Count < maxSpawns) {
            mySpawns.Add(Instantiate(myPrefabEnemyAI));


        }
    }

    public void RemoveSpawn ( EnemyAI enemy ) {
        mySpawns.Remove(enemy);


    }
}
