using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int modulesAmount = 25;
        
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] public LevelTarckovChain chain;
    [SerializeField] private Transform startModule;
    private Transform startingModule;
    private Transform currentModule;

    private void Start()
    {
        startingModule = Instantiate(startModule);
        startingModule.transform.position = Vector3.zero;
        grid.addModule(startingModule.position, startingModule.GetComponent<Module>());
        currentModule = startingModule;
        InstantiateCarAtOrigin();
        GenerateLevel(chain);
    }

    private void InstantiateCarAtOrigin()
    {
        var position = startingModule.position;
        var initialPos = new Vector3( position.x, 15 + 1.5f, position.z); //30 due the size of the module, maybe better to use a variable here to modify the size
        Instantiate(carPrefab, initialPos, Quaternion.identity);
    }
    public void GenerateLevel(LevelTarckovChain levelChain)
    {
        Chain c = levelChain.GetChain();
        for (int i = 0; i < modulesAmount; i++)
        {
            Transform t = Instantiate(c.Prefab);
            t.position = currentModule.position + c.GetSpawnPoint(currentModule.position).position;
            grid.addModule(t.position, t.GetComponent<Module>());
            currentModule = t;
            c = c.GetChild();
        }
        
    }
}