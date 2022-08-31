using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderForaBehavior : MonoBehaviour {
    private CircleCollider2D mCollider;
    public List<AquaticosController> grupoTarget;
    private void Awake ( ) {
        mCollider = GetComponent<CircleCollider2D>();
    }
    private IEnumerator Start ( ) {
        mCollider.radius = Random.Range(0.123f, 0.5f);
        yield return null;
    }
    private void OnTriggerEnter2D ( Collider2D collision ) {
        if (grupoTarget.Contains(collision.transform.parent.GetComponent<EnemyAI>().meuGrupo) && GameMangerGerencia.inGame) {
            transform.parent.GetComponent<EnemyAI>().targetMain = collision.gameObject;
            transform.parent.GetComponent<EnemyAI>().hasTarget = true;
            StartCoroutine(transform.parent.GetComponent<EnemyAI>().SeekTarget());


        }
    }
    private void OnTriggerExit2D ( Collider2D collision ) {
        if (grupoTarget.Contains(collision.transform.parent.GetComponent<EnemyAI>().meuGrupo) && GameMangerGerencia.inGame) {
            if (transform.parent.GetComponent<EnemyAI>().hasTarget) {
                StartCoroutine(transform.parent.GetComponent<EnemyAI>().SeekTarget());
            }


        }
    }

    private void OnTriggerStay2D ( Collider2D collision ) {
        if (grupoTarget.Contains(collision.transform.parent.GetComponent<EnemyAI>().meuGrupo)) {
            if (!transform.parent.GetComponent<EnemyAI>().hasTarget) {
                transform.parent.GetComponent<EnemyAI>().targetMain = collision.gameObject;
            }


        }
    }
}
