using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int modulesAmount = 5;

    [SerializeField] private Transform endTrigger;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] public LevelTarckovChain chain;
    [SerializeField] private Transform startModule;
    private Transform startingModule;
    private Transform currentModule;
    private LevelTarckovChain levelChain;
    private GameObject player;

    private void Start()
    {
        startingModule = Instantiate(startModule,transform);
        startingModule.transform.position = Vector3.zero;
        grid.addModule(startingModule.position, startingModule.GetComponent<Module>());
        currentModule = startingModule;
        GenerateLevel(chain);
        InstantiateCarAtOrigin();
    }

    private void InstantiateCarAtOrigin()
    {
        var position = startingModule.position;
        var initialPos =
            new Vector3(position.x, 15 + 3.5f,
                position.z); //30 due the size of the module, maybe better to use a variable here to modify the size
        player = Instantiate(carPrefab, initialPos, initialRotation);
        player.transform.GetChild(0).rotation = initialRotation;
    }

    private Quaternion initialRotation;

    public void GenerateLevel(LevelTarckovChain l)
    {
        levelChain = l;
        Chain c = levelChain.GetChain();
        for (int i = 0; i < modulesAmount; i++)
        {
            Transform t = Instantiate(c.Prefab,transform);
            t.position = currentModule.position + c.GetSpawnPoint(currentModule.position).position;
            if (i == 0)
            {
                Vector3 direction = (t.position - currentModule.position).normalized;
                initialRotation = Quaternion.LookRotation(direction);
            }

            Debug.DrawLine(currentModule.position, t.position, Color.magenta, 100000);
            grid.addModule(t.position, t.GetComponent<Module>());
            currentModule = t;
            c = c.GetChild();
        }

        endTrigger.position = currentModule.position + new Vector3(0, 15, 0);
    }

    public void AddMoreLevel()
    {
        Chain c = levelChain.GetChain();
        for (int i = 0; i < modulesAmount; i++)
        {
            Transform t = Instantiate(c.Prefab,transform);
            t.position = currentModule.position + c.GetSpawnPoint(currentModule.position).position;
            grid.addModule(t.position, t.GetComponent<Module>());
            currentModule = t;
            c = c.GetChild();
        }

        endTrigger.position = currentModule.position + new Vector3(0, 15, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            AddMoreLevel();
        }
    }

    public void ResetLevel()
    {
        var position = startingModule.position;
        var initialPos =
            new Vector3(position.x, 15 + 3.5f,
                position.z); //30 due the size of the module, maybe better to use a variable here to modify the size
        player.transform.position = initialPos;
        player.transform.rotation = initialRotation;
        player.transform.GetChild(0).GetComponent<CarController>().rb.velocity = Vector3.zero;
        player.transform.GetChild(0).GetComponent<CarController>().rb.angularVelocity = Vector3.zero;
        player.transform.GetChild(0).transform.position = initialPos;
        player.transform.GetChild(0).rotation = initialRotation;
        AddMoreLevel();
    }
}