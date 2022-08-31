using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveStep : MonoBehaviour
{

    private Material myMaterial;
    private LigandoPontosController _gameManger;
    private float fade;
    private int tentative;

    private void Start()
    {

        myMaterial = this.GetComponent<SpriteRenderer>().material;
        fade = myMaterial.GetFloat("_Fade");
        _gameManger = FindObjectOfType<LigandoPontosController>();
        tentative = _gameManger._tentative;
    }

    public void Dis()
    {
        StartCoroutine(DissolveObj());

    }

    public IEnumerator DissolveObj()
    {

        float f = 1f / (float)this.tentative;
        float fd = fade - f;

        bool isDissolving = true;

        do
        {
            fade -= 0.01f;
            
            myMaterial.SetFloat("_Fade", fade);
            if (fade <= fd)
            {
                fade = fd;
                isDissolving = false;
            }
            yield return new WaitForSeconds(0.05f);
        } while (isDissolving);
        if (fade <= 0f)
        {
            _gameManger.gameOver = true;
            fade = 0f;
        }


    }
}
