using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpuMoveAI : MonoBehaviour
{

    [SerializeField] private ObjectSpawner spawner;

    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private float velocityDelta;

    private AmazoniaPlayerSkills skillsRef;
    private SpriteRenderer spriteRenderer;

    public bool canMove;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        skillsRef = GetComponent<AmazoniaPlayerSkills>();
        StartCoroutine(UseSkills());
    }
    private void FixedUpdate()
    {
        if (spawner.newObject != null)
        {
            if (skillsRef.ReturnCanMove())
            {
                targetPosition = spawner.newObject.transform.position;
                transform.position = Vector2.Lerp(this.transform.position, new Vector2(targetPosition.x, this.transform.position.y), Time.fixedDeltaTime * velocityDelta);
                if (Vector2.Lerp(this.transform.position, new Vector2(targetPosition.x, this.transform.position.y), Time.fixedDeltaTime * velocityDelta).x > 0) spriteRenderer.flipX = false;
                else spriteRenderer.flipX = true;
            }
        }
    }

    private IEnumerator UseSkills()
    {
        while (AmazoniaGameManager.inGame)
        {
            int time = Random.Range(0, 10);
            yield return new WaitForSeconds(time);
            int randomChange = Random.Range(0, 10);
            if (randomChange <= 5)
            {
                if (skillsRef.ReturnCanUseSkill())
                {
                    if (skillsRef.ReturnTypePower() == AmazoniaPlayerSkills.TypePower.Dash)
                    {
                        transform.position = Vector2.Lerp(this.transform.position, new Vector2(targetPosition.x, this.transform.position.y), Time.fixedDeltaTime * velocityDelta * 10);
                        skillsRef.SetCoolDownSkill();
                    }
                    else
                    {
                        GameObject enemy = FindObjectOfType<AmazoniaPlayerMove>().gameObject;
                        enemy.GetComponent<AmazoniaPlayerSkills>().SetStun(transform, enemy.transform, 1f);
                        skillsRef.SetCoolDownSkill();
                    }
                }
            }
        }
    }
}
