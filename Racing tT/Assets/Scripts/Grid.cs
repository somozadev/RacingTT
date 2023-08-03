using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public static Grid Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private Dictionary<Vector3, Module> grid = new Dictionary<Vector3, Module>();

    public void addModule(Vector3 pos, Module m)
    {
        grid[pos] = m;
    }

    public bool idPosAvailable(Vector3 pos)
    {
        return !grid.ContainsKey(pos);
    }
}
