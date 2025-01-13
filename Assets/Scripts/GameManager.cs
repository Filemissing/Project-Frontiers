using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Reset()
    {
        instance = this;
    }

    public GameObject player;

    void Update()
    {
        
    }
}
