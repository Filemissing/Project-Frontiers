using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public Carry playerCarry;
    public Recipe[] recipes;
    private void Awake()
    {
        instance = this;
        playerCarry = player.GetComponent<Carry>();
    }
    void Update()
    {
        
    }
}
