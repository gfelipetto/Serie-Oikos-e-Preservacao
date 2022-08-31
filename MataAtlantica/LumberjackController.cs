using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LumberjackController : MonoBehaviour {

    private float dist;

    public static bool lumberjackWin;

    public Transform lastPos;
    public bool isLumberjackMoving;
    public GameObject lumberjackPrefab;
    public TreeBehavior treeTargetPrefab;
    public List<TreeBehavior> treeList;

    [Range(0.1f, 10f)]
    public float cutSpeed;
    [Range(0.1f, 10f)]
    public float moveSpeed;

    void Start ( ) {
        lumberjackWin = false;
        isLumberjackMoving = false;
        treeList = FindObjectsOfType<TreeBehavior>().ToList();
        treeTargetPrefab = NextTree();
        Quaternion rot = Quaternion.Euler(0, 180, 0);
        lumberjackPrefab.transform.rotation = rot;
    }

    void FixedUpdate ( ) {
        dist = Vector2.Distance(lumberjackPrefab.transform.position, treeTargetPrefab.transform.position);
        if (isLumberjackMoving) {
            MoveLumberjack();
        }

        if (dist <= 1.5f) {
            if (treeTargetPrefab.name.Contains("Final")) {
                Debug.Log("Jogador Perdeu Criando Char");
                lumberjackWin = true;
            }
            if (isLumberjackMoving) {
                isLumberjackMoving = false;
                StartCoroutine(KillTree(treeTargetPrefab));
            }

        } else {
            isLumberjackMoving = true;
        }
    }

    private IEnumerator KillTree ( TreeBehavior tree ) {
        while (!isLumberjackMoving) {
            //if (tree.health <= 0 && treeList.Count > 1) {
            if (tree.health <= 0 ) {
                tree.gameObject.SetActive(false);
                treeList.Remove(tree);
                treeTargetPrefab = NextTree();
                StopAllCoroutines();
                isLumberjackMoving = true;
            }
            tree.health--;
            yield return new WaitForSeconds(cutSpeed);
        }



    }

    private void MoveLumberjack ( ) {
        lumberjackPrefab.transform.position = Vector2.MoveTowards(lumberjackPrefab.transform.position, treeTargetPrefab.transform.position, moveSpeed * Time.deltaTime);
        //Quaternion rot = Quaternion.LookRotation(lumberjackPrefab.transform.position - treeTargetPrefab.transform.position, Vector3.forward);
        //Quaternion rot = Quaternion.Euler(0, 180, 0);
        //lumberjackPrefab.transform.rotation = rot;
        //lumberjackPrefab.transform.Translate(Vector3.right * Time.deltaTime);
    }
    private TreeBehavior NextTree ( ) {
        float distance = Vector2.Distance(lumberjackPrefab.transform.position, treeList[0].transform.position);
        float minorDistance = distance;
        int index = 0;
        TreeBehavior nextTree;
        for (int i = 0; i < treeList.Count; i++) {
            distance = Vector2.Distance(lumberjackPrefab.transform.position, treeList[i].transform.position);
            if (minorDistance >= distance) {
                minorDistance = distance;
                index = i;
            }

        }
        nextTree = treeList[index];

        return nextTree;
    }
}
