using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Space]
    [SerializeField]
    List<Pool> pools;

    public static ObjectManager Instance;

    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (!Instance)
            Instance = this;

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab);
                gameObject.SetActive(false);
                gameObject.transform.parent = this.transform;
                objectPool.Enqueue(gameObject);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject Spawn(string tag, Transform parent, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        if (poolDictionary[tag].Count == 0)
        {
            var pool = pools.Find(x => x.tag == tag);

            for (int i = 0; i < 10; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab);
                gameObject.SetActive(false);
                gameObject.transform.parent = this.transform;
                poolDictionary[tag].Enqueue(gameObject);
            }
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.parent = parent;
        objectToSpawn.transform.localPosition = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.localScale = Vector3.one;

        return objectToSpawn;
    }

    public void Destroy(GameObject gameObject, string tag)
    {
        gameObject.SetActive(false);
        poolDictionary[tag].Enqueue(gameObject);
        gameObject.transform.parent = this.transform;
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
