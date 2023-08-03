using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New LevelTarkovChain", menuName = "LevelTarkovChain")]
public class LevelTarckovChain : ScriptableObject
{
    [SerializeField]
    private List<Chain> chains = new List<Chain>();
    public List<Chain> Chains => chains;


    public Chain GetChain()
    {
        return chains[Random.Range(0, chains.Count)];
    }
}

