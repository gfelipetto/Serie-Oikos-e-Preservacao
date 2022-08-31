using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    
    //public bool triggerSpawn = false;
    public ObjectPooler theObjectPool;
    public GameObject newObject;
    public GameObject otherObject;

    [SerializeField] private float chanceToSpawnAnother = 10f;
    Vector3 newPosition;
    
    Bounds bounds;
    private void Start ( ) {
        bounds = transform.GetComponent<BoxCollider2D>().bounds;
        
    }

    
    
    public void Spawn ( ) {
        float random = Random.Range(0f, 101f);
        //if (triggerSpawn) {

        //newElevator = theObjectPool.GetPooledObject();
        newObject = theObjectPool.GetObject(Random.Range(0, theObjectPool.pooledObjects.Count));

        newPosition = transform.position;
        newPosition.x = Random.Range(bounds.min.x, bounds.max.x);
        newObject.transform.position = newPosition;
        
        newObject.SetActive(true);

        if (random <= (100f - chanceToSpawnAnother)) {
            otherObject = theObjectPool.GetObject(Random.Range(0, theObjectPool.pooledObjects.Count));
            newPosition = transform.position;
            newPosition.x = Random.Range(bounds.min.x, bounds.max.x);
            otherObject.transform.position = newPosition;

            otherObject.SetActive(true);
        } 
        

        //triggerSpawn = false;
        //}



    }
}
