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
    public Carry playerCarry;
    public Recipe[] recipes;
    private void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
    }
    void Update()
    {
        
    }
}
