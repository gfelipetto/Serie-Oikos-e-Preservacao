using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DissolveCaatingaEffect : MonoBehaviour {

    public Slider progressBar;
    public Material myMaterial;
    private float fade;

    void Start ( ) {
        fade = myMaterial.GetFloat("_Fade");
        myMaterial.SetFloat("_Fade", progressBar.value);

    }

    public void Dis () {
        myMaterial.SetFloat("_Fade", progressBar.value);
    }
    
}
