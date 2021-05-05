using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject Parent;
    public GameObject[] Prefabs;
    
    void Start()
    {
        foreach (var p in Prefabs)
        {
            if (p != null)
            {
                try
                {
                    var go = GameObject.Instantiate(p, this.transform.parent, false);
                    go.name = p.name;
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        Destroy(this);
    }

}
