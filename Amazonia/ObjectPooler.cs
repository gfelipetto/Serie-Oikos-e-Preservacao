using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public GameObject[] objects;

    public int pooledAmount;

    public List<GameObject> pooledObjects;

    private void Start ( ) {
        pooledObjects = new List<GameObject>();
        CreatePooledObjects(pooledAmount);

    }

    public void CreatePooledObjects ( int amount ) {
        pooledObjects.Clear();
        for (int i = 0; i < amount; i++) {
            GameObject obj = Instantiate(objects[Random.Range(0, objects.Length)]);
            obj.name += i;
            //GameObject obj = Instantiate(elevators[i]);
            obj.SetActive(false);
            pooledObjects.Add(obj);

        }
    }

    //public GameObject GetPooledObject ( ) {
    //    for (int i = 0; i < pooledObjects.Count; i++) {
    //        if (!pooledObjects[i].activeInHierarchy) {
    //            return pooledObjects[i];
    //        }
    //    }
    //    GameObject obj = Instantiate(elevators[Random.Range(0, elevators.Length)], transform.position,Quaternion.identity);
    //    obj.SetActive(false);
    //    pooledObjects.Add(obj);
    //    return obj;
    //}

    public GameObject GetObject ( int r ) {
        if (!pooledObjects[r].activeInHierarchy) {
            return pooledObjects[r];
        }

        return pooledObjects.FindLast(x => !x.activeInHierarchy).gameObject;
    }

}
