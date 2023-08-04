using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chain", menuName = "Chain")]
public class Chain : ScriptableObject
{
    [SerializeField]
    private Transform prefab;
    public Transform Prefab => prefab;

    [SerializeField]
    private List<Chain> chains = new List<Chain>();
    public List<Chain> Chains => chains;

    public Chain GetChild()
    {
        return chains[Random.Range(0, chains.Count)];
    }

    public Transform GetSpawnPoint(Vector3 offset)
    {
        return prefab.GetChild(0).GetComponent<Module>().GetSpawnPoint(offset);
    }

}

