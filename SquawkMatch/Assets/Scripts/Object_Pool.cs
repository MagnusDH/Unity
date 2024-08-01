using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{
    public GameObject[] objectPrefabs;                                      //Array of prefab objects to put in the pool
    private Dictionary<string, Queue<GameObject>> ObjectPool_Dictionary;    //Dictionary
    public static Object_Pool Instance {get; private set;}                   //a pattern which ensures that only one instance of the object pool exists

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Awake is called()");

        if(Instance == null){
            Instance = this;
            InitializeObjectPool();
        }
        else{
            Destroy(gameObject);
        }
    }

    /*
    -Takes all the elements in the given objectPrefabs array to Initialize a dictionary.
    -The objects can be fetched by using their name*/
    private void InitializeObjectPool()
    {
        Debug.Log("InitializePool is called");
        ObjectPool_Dictionary = new Dictionary<string, Queue<GameObject>>();    //Creating a dictionary where keys are strings and each entry contains a list of gameObjects

        foreach(GameObject prefab in objectPrefabs){                            //For each prefab in the given array
            Debug.Log("Adding element to dictionary: " + prefab.name);
            
            string prefabName = prefab.name;                                    //Fetch the name of the prefab
            
            ObjectPool_Dictionary[prefabName] = new Queue<GameObject>();        //Create a queue for the current prefab in the object pool dictionary

            //Populate the queue with instances of the given prefab!!!!!!!!!!!!!!!!!!!!!!!!!1

        }

        Debug.Log("Size of entire object_pool: " + ObjectPool_Dictionary.Count);
        if(ObjectPool_Dictionary.ContainsKey("Bullet")){
            Debug.Log("Object pool contains the key 'Bullet'!!!!!");
        }
    }

    /*
    -Spawn a game object from the pool based on the input prefab name.
    -The object is spawned at a position, velocity and rotation*/
    public GameObject SpawnObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        Debug.Log("SpawnObject is called with name: " + prefabName);
        //If the given object is in the dictionary
        if(ObjectPool_Dictionary.ContainsKey(prefabName)){
            Queue<GameObject> objectQueue = ObjectPool_Dictionary[prefabName];  //Create a list of objects

            Debug.Log("Count in object queue: " + objectQueue.Count);
            //If there are elements in the queue/list
            if(objectQueue.Count > 0){
                GameObject obj = objectQueue.Dequeue();                         //Remove the object from the original list of objects
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            //If there are no elements in the queue/list
            else{
                Debug.LogWarning("No objects left in the pool for" + prefabName);
            }
        }
        //If the dictionary does not contain given key
        else{
            Debug.LogError("Object pool does not contain prefab: " + prefabName);
        }

        return null;
    }

    //Return a gameObject to the pool for later reuse
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        ObjectPool_Dictionary[obj.name].Enqueue(obj);
    }
}
