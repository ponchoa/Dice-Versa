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

public enum FaceType
{
    ATTACK = 0,
    DEFEND = 1,
    HEAL = 2,
    RANGED = 3,
    FIRE = 4,
    PLASMA = 5,
    REGEN = 6,
    ATKALL = 7,
    SMOKE = 8,
    ABSORB = 9,
    EXTRADIE = 10,
    KILL = 11,
    LOOT = 12,
    REVIVE = 13,
    POISON = 14,
    NONE = 15
}

public enum GameState
{
    MENU_MAIN,
    MENU_GAME,
    MENU_DICE,
    FIGHT_ROLL,
    FIGHT_ACT,
    FIGHT_ENEMIES,
    MAP_CHOICE,
    MAP_EVENT,
    MAP_SHOP,
    MAP_SMITH
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

    [Header("UI Elements")]
    public Transform dieEditPanel;

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
    public GameState CurrentState { get; private set; } = GameState.MENU_MAIN;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }

    #region States
    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentStateExit(newState);
        GameState prevState = CurrentState;
        CurrentState = newState;
        CurrentStateEnter(prevState);
    }

    private void CurrentStateExit(GameState nextState)
    {
        //TODO: Add what happens when a state is about to end.
    }
    private void CurrentStateEnter(GameState previousState)
    {
        //TODO: Add what happens when a state was just started.
    }
    #endregion

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
    public void SetStartingDie(DiePhysical die, HeroClass heroClass)
    {
        //TODO: Create basic layouts for each class.

        switch (heroClass)
        {
            case HeroClass.THIEF:
                die[(int)DieFace.FRONT].faceID = (int)FaceType.ATTACK;
                break;
            case HeroClass.MAGE:
                break;
            case HeroClass.WARRIOR:
            default:
                break;
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
