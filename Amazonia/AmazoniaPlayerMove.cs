using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmazoniaPlayerMove : MonoBehaviour
{

    public float velocity = 2f;

    private AmazoniaPlayerSkills skillsRef;
    private SpriteRenderer spriteRender;
    Rigidbody2D rb;

    private void Start()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        skillsRef = GetComponent<AmazoniaPlayerSkills>();
    }

    private void Update()
    {
        if (AmazoniaGameManager.inGame)
        {
            if (skillsRef.ReturnCanMove())
            {
                var input  = new Vector2(Input.GetAxis("Horizontal") * velocity, 0);
                if (input.x > 0) spriteRender.flipX = true;
                else spriteRender.flipX = false;
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, 0);
            }
            if (skillsRef.ReturnCanUseSkill())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (skillsRef.ReturnTypePower() == AmazoniaPlayerSkills.TypePower.Dash)
                    {
                        rb.velocity = skillsRef.ReturnDashValue(Input.GetAxis("Horizontal"), 100);
                        skillsRef.SetCoolDownSkill();
                    }
                    else
                    {
                        GameObject enemy = FindObjectOfType<CpuMoveAI>().gameObject;
                        enemy.GetComponent<AmazoniaPlayerSkills>().SetStun(transform, enemy.transform, 1f);
                        skillsRef.SetCoolDownSkill();
                    }   
                }
            }
        }
    }
}
