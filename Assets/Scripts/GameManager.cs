using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public Carry playerCarry;
    public Recipe[] recipes;
    public OrderRequest[] orders;

    void Awake()
    {
        playerCarry = player.GetComponent<Carry>();
        instance = this;
    }

    void Update()
    {

    }
}
