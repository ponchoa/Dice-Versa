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
    MENU_MAIN = 0,
    MENU_GAME = 1,
    MENU_DICE = 2,
    FIGHT_ROLL = 3,
    FIGHT_ACT = 4,
    FIGHT_ENEMIES = 5,
    MAP_CHOICE = 6,
    MAP_EVENT = 7,
    MAP_SHOP = 8,
    MAP_SMITH = 9
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
    delegate void UpdateStateDelegate();
    Dictionary<GameState, UpdateStateDelegate> updateMap;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        InstantiateUpdateMap();
    }

    private void Update()
    {
        UpdateCurrentState();
    }

    private void UpdateCurrentState()
    {
        updateMap[CurrentState]();
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

    private void InstantiateUpdateMap()
    {
        updateMap = new Dictionary<GameState, UpdateStateDelegate>();

        updateMap.Add(GameState.MENU_MAIN, MenuMainUpdate);
        updateMap.Add(GameState.MENU_GAME, MenuGameUpdate);
        updateMap.Add(GameState.MENU_DICE, MenuDiceUpdate);
        updateMap.Add(GameState.FIGHT_ROLL, FightRollUpdate);
        updateMap.Add(GameState.FIGHT_ACT, FightActUpdate);
        updateMap.Add(GameState.FIGHT_ENEMIES, FightEnemiesUpdate);
        updateMap.Add(GameState.MAP_CHOICE, MapChoiceUpdate);
        updateMap.Add(GameState.MAP_EVENT, MapEventUpdate);
        updateMap.Add(GameState.MAP_SHOP, MapShopUpdate);
        updateMap.Add(GameState.MAP_SMITH, MapSmithUpdate);
    }

    #region State Update Methods
    private void MenuMainUpdate()
    {

    }
    private void MenuGameUpdate()
    {

    }
    private void MenuDiceUpdate()
    {

    }
    private void FightRollUpdate()
    {

    }
    private void FightActUpdate()
    {

    }
    private void FightEnemiesUpdate()
    {

    }
    private void MapChoiceUpdate()
    {

    }
    private void MapEventUpdate()
    {

    }
    private void MapShopUpdate()
    {

    }
    private void MapSmithUpdate()
    {

    }
    #endregion
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
        switch (heroClass)
        {
            default:
            case HeroClass.WARRIOR:
                die[(int)DieFace.FRONT].faceID = (int)FaceType.ATTACK;
                die[(int)DieFace.FRONT].numberOfPips = 2;
                die[(int)DieFace.BACK].faceID = (int)FaceType.ATTACK;
                die[(int)DieFace.BACK].numberOfPips = 2;
                die[(int)DieFace.LEFT].faceID = (int)FaceType.DEFEND;
                die[(int)DieFace.LEFT].numberOfPips = 2;
                die[(int)DieFace.RIGHT].faceID = (int)FaceType.DEFEND;
                die[(int)DieFace.RIGHT].numberOfPips = 2;
                die[(int)DieFace.TOP].faceID = (int)FaceType.ATTACK;
                die[(int)DieFace.TOP].numberOfPips = 1;
                die[(int)DieFace.BOTTOM].faceID = (int)FaceType.DEFEND;
                die[(int)DieFace.BOTTOM].numberOfPips = 0;
                break;
            case HeroClass.THIEF:
                die[(int)DieFace.FRONT].faceID = (int)FaceType.ATTACK;
                die[(int)DieFace.FRONT].numberOfPips = 2;
                die[(int)DieFace.BACK].faceID = (int)FaceType.ATTACK;
                die[(int)DieFace.BACK].numberOfPips = 1;
                die[(int)DieFace.LEFT].faceID = (int)FaceType.RANGED;
                die[(int)DieFace.LEFT].numberOfPips = 1;
                die[(int)DieFace.RIGHT].faceID = (int)FaceType.RANGED;
                die[(int)DieFace.RIGHT].numberOfPips = 1;
                die[(int)DieFace.TOP].faceID = (int)FaceType.LOOT;
                die[(int)DieFace.TOP].numberOfPips = 1;
                die[(int)DieFace.BOTTOM].faceID = (int)FaceType.NONE;
                die[(int)DieFace.BOTTOM].numberOfPips = 0;
                break;
            case HeroClass.MAGE:
                die[(int)DieFace.FRONT].faceID = (int)FaceType.FIRE;
                die[(int)DieFace.FRONT].numberOfPips = 2;
                die[(int)DieFace.BACK].faceID = (int)FaceType.FIRE;
                die[(int)DieFace.BACK].numberOfPips = 1;
                die[(int)DieFace.LEFT].faceID = (int)FaceType.PLASMA;
                die[(int)DieFace.LEFT].numberOfPips = 2;
                die[(int)DieFace.RIGHT].faceID = (int)FaceType.NONE;
                die[(int)DieFace.RIGHT].numberOfPips = 1;
                die[(int)DieFace.TOP].faceID = (int)FaceType.HEAL;
                die[(int)DieFace.TOP].numberOfPips = 1;
                die[(int)DieFace.BOTTOM].faceID = (int)FaceType.NONE;
                die[(int)DieFace.BOTTOM].numberOfPips = 0;
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
