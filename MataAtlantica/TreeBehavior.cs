using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour {

    public int health;
    

    private void Awake ( ) {
        health = Random.Range(3, 11);
        
    }


}
