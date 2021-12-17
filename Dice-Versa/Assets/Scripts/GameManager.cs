using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DieFace
{
    NONE = -1,
    FRONT = 0,
    BACK = 1,
    LEFT = 2,
    RIGHT = 3,
    TOP = 4,
    BOTTOM = 5
}

public enum HeroClass
{
    WARRIOR = 0,
    THIEF = 1,
    MAGE = 2
}

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

    [Header("Game Bounds")]
    [SerializeField]
    private Transform ground;

    [SerializeField]
    private Transform topWall, bottomWall, leftWall, rightWall, ceiling;

    [Header("Settings")]
    public LayerMask raycastMask;

    [Header("Debug Values")]
    public List<DiePhysical> testDies;
    public DieUI dieUI;

    private bool preventRoll = false;
    private bool CanRoll
    {
        get
        {
            if (preventRoll)
                return false;
            foreach (DiePhysical die in testDies)
            {
                if (die.CurrentFace == DieFace.NONE)
                    return false;
            }
            return true;
        }
    }

    private void Update()
    {
        if (CanRoll)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (DiePhysical die in testDies)
                {
                    die.RollDie();
                }
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10f, raycastMask))
                {
                    DiePhysical die = hit.transform.GetComponent<DiePhysical>();
                    if (die && dieUI)
                    {
                        dieUI.DisplayDie(die);
                    }
                }
            }
        }
    }

    public DieFace[,] GetBaseLayout(HeroClass heroClass)
    {
        DieFace[,] res = new DieFace[4, 3]
        {
            {DieFace.NONE, DieFace.NONE, DieFace.NONE},
            {DieFace.NONE, DieFace.NONE, DieFace.NONE},
            {DieFace.NONE, DieFace.NONE, DieFace.NONE},
            {DieFace.NONE, DieFace.NONE, DieFace.NONE}
        };
        switch (heroClass)
        {
            default:
            case HeroClass.WARRIOR:
                res[1, 1] = DieFace.FRONT;
                res[3, 1] = DieFace.BACK;
                res[0, 1] = DieFace.LEFT;
                res[2, 1] = DieFace.RIGHT;
                res[1, 2] = DieFace.TOP;
                res[1, 0] = DieFace.BOTTOM;
                return res;
            case HeroClass.THIEF:
                res[1, 1] = DieFace.FRONT;
                res[3, 1] = DieFace.BACK;
                res[0, 1] = DieFace.LEFT;
                res[2, 1] = DieFace.RIGHT;
                res[0, 2] = DieFace.TOP;
                res[3, 0] = DieFace.BOTTOM;
                return res;
            case HeroClass.MAGE:
                res[1, 1] = DieFace.FRONT;
                res[3, 0] = DieFace.BACK;
                res[0, 2] = DieFace.LEFT;
                res[2, 1] = DieFace.RIGHT;
                res[1, 2] = DieFace.TOP;
                res[2, 0] = DieFace.BOTTOM;
                return res;
        }
    }
    public Vector3 GetRandomPosition()
    {
        Vector3 res = new Vector3();

        res.x = Random.Range(leftWall.position.x / 2f, rightWall.position.x / 2f);
        res.y = Random.Range(1f, ceiling.position.y);
        res.z = Random.Range(bottomWall.position.z / 2f, topWall.position.z / 2f);

        return res;
    }
}
