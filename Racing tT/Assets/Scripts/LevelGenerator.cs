 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [SerializeField]
    public LevelTarckovChain chain;

    private Transform currentModule;
    [SerializeField]
    private Transform startModule;
    private Transform startingModule;

    private void Start()
    {
        startingModule = Instantiate(startModule);
        startingModule.transform.position = Vector3.zero;
        grid.addModule(startingModule.position, startingModule.GetComponent<Module>());
        currentModule = startingModule;
        GenerateLevel(chain);
    }

    public void GenerateLevel(LevelTarckovChain levelChain)
    {
        Chain c = levelChain.GetChain();
        for (int i = 0; i < 25; i++)
        {
            Transform t = Instantiate(c.Prefab);
            t.position = currentModule.position + c.GetSpawnPoint(currentModule.position).position;
            grid.addModule(t.position, t.GetComponent<Module>());
            currentModule = t;
            c = c.GetChild();
        }
    }
}
