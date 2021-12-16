using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public Transform topWall, bottomWall, leftWall, rightWall;

    public DieRoller testDie;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            testDie.RollDie();
        }
    }
}
