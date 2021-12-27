using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCollectables : MonoBehaviour
{

    private CurrentScene sceneCheck;

    [SerializeField] private int amountToSpawn;
    
    public SpawnPoint[] spawnPoints;

    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> fuel;
    private GameObject collectable;

    [SerializeField] private bool needSpawning = true;

    

    private void Awake()
    {
        sceneCheck = FindObjectOfType<CurrentScene>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        if (needSpawning == true)
        {
            spawnPoints = FindObjectsOfType<SpawnPoint>();

            fuel = new List<GameObject>();
            amountToSpawn = spawnPoints.Length;

            SceneHandler _sceneHandler = FindObjectOfType<SceneHandler>();

            _sceneHandler.ActiveScene();

            for (int i = 0; i < amountToSpawn; i++)
            {
                Spawn();
            }
        }
        else
        {
            for (int i = 0; i < fuel.Count; i++)
            {
                ResetObjects(fuel[i]);
            }
        }
    }

    private void Spawn()
    {
        //Debug.Log("SPAWN COLLECTABLES!!!!");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            //GameObject collectable = ObjectPool.Instance.GetPooledObjects();

            if (spawnPoints[i].GetComponent<SpawnPoint>().spotTaken == false)
            {
                collectable = Instantiate(prefab);

                //Debug.Log("SPAWNING FUEL!");
                fuel.Add(collectable);

                collectable.transform.position = spawnPoints[i].transform.position;
                collectable.transform.rotation = spawnPoints[i].transform.rotation;
                spawnPoints[i].GetComponent<SpawnPoint>().SpotTaken(true);
                collectable.SetActive(true);
            }

            //Debug.Log(i);
            if (i == spawnPoints.Length - 1)
            {
                needSpawning = false;
            }
        }
    }

    private void ResetObjects(GameObject _objects)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            foreach (GameObject _object in fuel)
            {
                collectable = _object;

                collectable.transform.position = spawnPoints[i].transform.position;
                collectable.transform.rotation = spawnPoints[i].transform.rotation;
                collectable.SetActive(true);
            }
        }
    }
}
