using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmazoniaPlayerSkills : MonoBehaviour {
    [SerializeField] private bool _CanMove = true;
    [SerializeField] private TypePower _typePower;
    private bool canUseSkill = true;

    public ParticleSystem ps_stun_galo;

    public enum TypePower {
        Dash,
        Stun
    }

    public TypePower ReturnTypePower ( ) {
        return _typePower;
    }
    public bool ReturnCanMove ( ) {
        return _CanMove;
    }
    public bool ReturnCanUseSkill ( ) {
        return canUseSkill;
    }
    public Vector2 ReturnDashValue ( float axis, float vel ) {
        return new Vector2(axis * vel, 0);
    }
    public void SetStun ( Transform currentLocal, Transform enemyLocal, float MaxDistance ) {
        StartCoroutine(CooldownSkill());
        if (Vector2.Distance(currentLocal.position, enemyLocal.position) < MaxDistance) {
            StartCoroutine(CooldownStun());
        }
    }
    public void SetCoolDownSkill ( ) {
        StartCoroutine(CooldownSkill());
    }
    private IEnumerator CooldownSkill ( ) {
        canUseSkill = false;
        yield return new WaitForSeconds(6f);
        canUseSkill = true;
    }
    private IEnumerator CooldownStun ( ) {
        _CanMove = false;
        GetComponent<Rigidbody2D>().position = this.transform.position;
        if (this.name.StartsWith("G")) {
            ps_stun_galo.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        _CanMove = true;
    }
}
