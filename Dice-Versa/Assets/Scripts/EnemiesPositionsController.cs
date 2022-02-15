using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPositionsController : MonoBehaviour
{
    static EnemiesPositionsController instance;
    public static EnemiesPositionsController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemiesPositionsController>();
            return instance;
        }
    }

    Transform[][] placementsArray;

    private void Start()
    {
        Transform[][] placementsArray = new Transform[transform.childCount][];

        for (int i = 0; i < transform.childCount; i++)
        {
            placementsArray[i] = new Transform[transform.GetChild(i).childCount];
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                placementsArray[i][j] = transform.GetChild(i).GetChild(j);
            }
        }
    }

    public List<Transform> GetEnemiesPlacement(int nbOfEnemies)
    {
        List<Transform> res = new List<Transform>();

        if (nbOfEnemies < 1 || nbOfEnemies > placementsArray.Length)
        {
            return res;
        }

        res.AddRange(placementsArray[nbOfEnemies - 1]);
        return res;
    }
}
