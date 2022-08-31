using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveBehavior : MonoBehaviour {
    [Range(1f, 5f)]
    [SerializeField] private float vel = 1f;

    Rigidbody2D rb;
    Collider2D coll;
    SpriteRenderer render;
    ObjectPooler objectPool;


    private void Awake ( ) {
        objectPool = FindObjectOfType<ObjectPooler>();
        vel = Random.Range(1f, 3f);
        render = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable ( ) {
        coll.enabled = true;
        render.enabled = true;

    }
    private void Update ( ) {
        rb.velocity = Vector2.down * vel;
        if (transform.position.y < -AmazoniaGameManager.viewBounds.y - 1f) {
            objectPool.pooledObjects.Find(x => x.gameObject == this.gameObject).SetActive(false);
            //Destroy(this.gameObject);
        }
    }
}
