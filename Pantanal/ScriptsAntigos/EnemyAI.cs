using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private SpawnButtonBehavior myButton;
    private EnemyAI enemyAITarget;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 myPosition;

    private SpriteRenderer spriteRender;

    private float bottomLimit = -4.5f, topLimit = 4.5f, leftLimit = -6f, rightLimit = 8f;
    [Header("Debug:")]
    public GameObject targetMain;
    [SerializeField] private float tiredParam = 0f;
    [SerializeField] private float tParam;
    [SerializeField] private float accuracy;
    [SerializeField] private float tiredValue;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private bool routeRoutineAllowed;
    [Header("Status:"), Space(10)]
    public bool hasTarget = false;
    public bool isTouching;
    public bool isTired;
    [Header("Atributos:"), Space(10)]
    public AquaticosController meuGrupo;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int life = 100;
    [SerializeField] private int damage = 20;

    //public static float chanceToSpawn = 0f;
    public float spawnParam;
    public string buttonName;
    public GameObject myPrefab;
    //public static int instancesNumber = 0;

    private void Start ( ) {
        //instancesNumber++;
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        myButton = GameObject.Find(buttonName).GetComponent<SpawnButtonBehavior>();
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = Random.Range(.8f, 2f);
        accuracy = Random.Range(43f, 100f);
        float x = Random.Range(leftLimit, rightLimit);
        float y = Random.Range(bottomLimit, topLimit);
        myPosition = startPosition = transform.position = new Vector2(x, y);
        tiredValue = Random.Range(1f, 3f);
        tParam = 0f;
        routeRoutineAllowed = true;

    }
    private void FixedUpdate ( ) {

        if (GameMangerGerencia.inGame) {
            if (routeRoutineAllowed) {

                StartCoroutine(GoByTheRoute());

            }


            if (targetMain != null) {
                float distance = Vector2.Distance(transform.position, targetMain.transform.position);
                if (distance < 0.5f) {
                    isTouching = true;
                } else {
                    isTouching = false;
                }
            } else {
                if (hasTarget) {
                    hasTarget = false;
                    routeRoutineAllowed = true;
                }
            }

            if (life <= 0f) {
                StopAllCoroutines();
                myButton.RemoveSpawn(this.GetComponent<EnemyAI>());
                Destroy(gameObject);
            }
        } else {
            StopAllCoroutines();
        }



    }

    IEnumerator Attack ( ) {
        while (hasTarget && targetMain != null && isTouching) {

            yield return new WaitForFixedUpdate();
            float random = Random.Range(0f, 100f);
            if (random < (100 - accuracy)) {
                Debug.LogWarning("Eu "+ this.name+"Causei dano em " + enemyAITarget.name);
                StartCoroutine(enemyAITarget.Hit(damage));

                if (enemyAITarget.life <= 0f || targetMain == null) {
                    startPosition = transform.position;
                    hasTarget = false;
                    isTouching = false;
                    targetMain = null;
                    routeRoutineAllowed = true;
                    StopCoroutine(SeekTarget());
                    StopCoroutine(Attack());


                }



            }
            //else {
            //    Debug.LogWarning("Errei o ataque em " + enemyAITarget.name);
            //}
            yield return new WaitForSeconds(1f);
            random = Random.Range(35f, 100f);
            if (random < (100 - accuracy)) {
                tiredParam += 0.25f;
                spawnParam = Random.Range(0.25f, 51f);
                if (spawnParam >= 45f) {
                    spawnParam = 0f;
                    Debug.LogWarning("instance");
                    if (myButton.mySpawns.Count < myButton.maxSpawns) {
                        GameObject go = Instantiate(myPrefab);
                        myButton.mySpawns.Add(go.GetComponent<EnemyAI>());

                    }
                }
            }
        }

        if (tiredParam >= tiredValue) {
            isTired = true;
            StartCoroutine(TakeARest());

        }
    }
    IEnumerator GoByTheRoute ( ) {
        routeRoutineAllowed = false;
        Vector2 p0, p1, p2, p3;
        p0 = startPosition;
        p1 = new Vector2(Random.Range(leftLimit, rightLimit), Random.Range(bottomLimit, topLimit));
        p2 = new Vector2(Random.Range(leftLimit, rightLimit), Random.Range(bottomLimit, topLimit));
        p3 = new Vector2(Random.Range(leftLimit, rightLimit), Random.Range(bottomLimit, topLimit));

        if (tiredParam >= tiredValue) {
            isTired = true;
            StartCoroutine(TakeARest());

        }
        yield return new WaitUntil(( ) => !isTired);
        while (tParam < 1) {
            if (targetMain != null) {
                tParam = 0f;
                routeRoutineAllowed = targetMain == null ? true : false;
                break;

            }
            tParam += Time.fixedDeltaTime * (movementSpeed / 10);
            myPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;
            Vector2 direction =  myPosition - (Vector2) transform.position;
            //float angle = Mathf.Clamp( Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, 0f, 90f);
            //rb.rotation = angle;
            //if (angle == 0 ) {
            //    spriteRender.flipY = true;
            //} else {
            //    spriteRender.flipY = false;
            //}
            //transform.rotation.SetLookRotation(myPosition);

            //transform.position = myPosition;
            transform.position = Vector2.MoveTowards(transform.position, myPosition, movementSpeed);

            yield return new WaitForFixedUpdate();
            //float distance = Vector2.Distance(p0, p3);
            //if (distance > 5) {
            //    yield return new WaitForSeconds(Time.deltaTime * distance / 10);

            //}
        }
        startPosition = p3;
        tParam = 0f;
        float random = Random.Range(15f, 100f);
        if (random < (100 - accuracy))
            tiredParam += 0.25f;


        //routeToGo += 1;

        //if (routeToGo > routes.Length -1) {
        //    routeToGo = 0;
        //}
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        if (!hasTarget) {
            routeRoutineAllowed = true;
        } else {
            yield return null;
        }

    }
    IEnumerator TakeARest ( ) {
        while (tiredParam > 0f) {
            tiredParam -= 0.1f;

            yield return new WaitForSeconds(0.3f);
        }
        tiredParam = 0f;
        isTired = false;
        if (hasTarget) {
            StartCoroutine(SeekTarget());
        }

    }
    IEnumerator Hit ( int hitDamage ) {
        life -= hitDamage / 2;
        yield return new WaitForSeconds(1f);
    }
    void MoveAtTarget ( Vector2 direction ) {
        transform.position = Vector2.MoveTowards(transform.position, direction, movementSpeed);
        //rb.MovePosition((Vector2)transform.position + (direction * movementSpeed * Time.fixedDeltaTime));
    }
    public IEnumerator SeekTarget ( ) {
        if (targetMain != null) {
            if (enemyAITarget != targetMain.transform.parent.GetComponent<EnemyAI>()) {
                enemyAITarget = targetMain.transform.parent.GetComponent<EnemyAI>();
            }
        }

        while (hasTarget && targetMain != null && !isTouching) {
            Vector3 direction = targetMain.transform.position - transform.position;
            //float angle = Mathf.Clamp( Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, 0f, 90f);
            //rb.rotation = angle;
            //if (angle == 0) {
            //    spriteRender.flipY = true;
            //} else {

            //    spriteRender.flipY = false;
            //}
            direction.Normalize();
            movement = direction;
            MoveAtTarget(movement);
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(( ) => !isTired);
        }
        if (hasTarget && targetMain != null && isTouching) {
            StartCoroutine(Attack());

        }

    }

}


