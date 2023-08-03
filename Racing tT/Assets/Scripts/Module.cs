using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();

    public Transform GetSpawnPoint(Vector3 offset)
    {
        Transform p;
        do
        {
            p = spawnPoints[Random.Range(0, spawnPoints.Count)];

        } while (!Grid.Instance.idPosAvailable(p.position+ offset));
        return p;
    }

}
