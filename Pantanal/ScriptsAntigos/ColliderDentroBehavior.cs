using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDentroBehavior : MonoBehaviour {

    private EnemyAI parent;
    private void Awake ( ) {
        parent = transform.parent.GetComponent<EnemyAI>();
    }
}
