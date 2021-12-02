using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject map0;
    [SerializeField] private GameObject map1;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance != null)
        {
            if(GameManager.instance.GameMap == 1)
            {
                map0.SetActive(false);
            }
        }
        else
        {
            map1.SetActive(false);
        }
    }
}
