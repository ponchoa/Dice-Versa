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

public class DieRoller : MonoBehaviour
{
    private DieFace currentFace = DieFace.NONE;
    public DieFace CurrentFace
    {
        get { return currentFace; }
    }

    private bool isRolling = false;
    private Transform[] faces;
    private Rigidbody rb;

    private void Start()
    {
        faces = new Transform[6];
        faces[(int)DieFace.FRONT] = transform.Find("Front");
        faces[(int)DieFace.BACK] = transform.Find("Back");
        faces[(int)DieFace.LEFT] = transform.Find("Left");
        faces[(int)DieFace.RIGHT] = transform.Find("Right");
        faces[(int)DieFace.TOP] = transform.Find("Top");
        faces[(int)DieFace.BOTTOM] = transform.Find("Bottom");

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isRolling)
        {
            currentFace = RollUpdate();
            if (currentFace != DieFace.NONE)
            {
                isRolling = false;
                print(currentFace);
            }
        }
    }

    public void RollDie(float forceScale = 1f)
    {
        isRolling = true;

        Vector3 startPos = transform.position;
        Vector3 endPos;
        do
        {
            endPos = GetRandomPosition();
        } while (startPos == endPos);

        rb.AddTorque(Vector3.Cross(endPos, startPos) * 100f * forceScale, ForceMode.Impulse);
        rb.AddForce((endPos - startPos).normalized * 10f * forceScale * rb.mass, ForceMode.Impulse);
    }
    private DieFace RollUpdate()
    {
        if (rb.IsSleeping())
        {
            DieFace res = DieFace.NONE;
            float yPos = float.MinValue;
            for (int i = 0; i < 6; i++)
            {
                if (faces[i].position.y > yPos)
                {
                    yPos = faces[i].position.y;
                    res = (DieFace)i;
                }
            }

            if (Vector3.Cross(faces[(int)res].forward, Vector3.down).sqrMagnitude > .01f)
            {
                RollDie(.5f);
                print("MISTHROWN");
                return DieFace.NONE;
            }

            return res;
        }

        return DieFace.NONE;
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 res = new Vector3();

        res.x = Random.Range(GameManager.Instance.leftWall.position.x / 2f, GameManager.Instance.rightWall.position.x / 2f);
        res.y = Random.Range(1f, 5f);
        res.z = Random.Range(GameManager.Instance.bottomWall.position.z / 2f, GameManager.Instance.topWall.position.z / 2f);

        return res;
    }
}
