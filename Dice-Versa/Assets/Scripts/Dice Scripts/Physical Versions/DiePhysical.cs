using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DiePhysical : MonoBehaviour
{
    private bool isRolling = false;
    private Rigidbody rb;
    private FacePhysical[] faces;

    public DieFace CurrentFace { get; private set; } = DieFace.TOP;

    public FacePhysical this[int i]
    {
        get { return faces[i]; }
    }
    public HeroClass heroClass = HeroClass.WARRIOR;
    public DieFace[,] layout;

    private void Start()
    {
        faces = new FacePhysical[6];
        faces[(int)DieFace.FRONT] = transform.Find("Front").GetComponent<FacePhysical>();
        faces[(int)DieFace.BACK] = transform.Find("Back").GetComponent<FacePhysical>();
        faces[(int)DieFace.LEFT] = transform.Find("Left").GetComponent<FacePhysical>();
        faces[(int)DieFace.RIGHT] = transform.Find("Right").GetComponent<FacePhysical>();
        faces[(int)DieFace.TOP] = transform.Find("Top").GetComponent<FacePhysical>();
        faces[(int)DieFace.BOTTOM] = transform.Find("Bottom").GetComponent<FacePhysical>();

        rb = GetComponent<Rigidbody>();

        layout = GameManager.Instance.GetBaseLayout(heroClass);
    }
    private void Update()
    {
        if (!rb.IsSleeping())
            isRolling = true;
        if (isRolling)
        {
            CurrentFace = RollUpdate();
            if (CurrentFace != DieFace.NONE)
            {
                isRolling = false;
                //print(currentFace);
                //print(heroClass + ": " + faces[(int)currentFace].numberOfPips);
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
            endPos = GameManager.Instance.GetRandomPosition();
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
                if (faces[i].transform.position.y > yPos)
                {
                    yPos = faces[i].transform.position.y;
                    res = (DieFace)i;
                }
            }

            if (transform.position.y > transform.lossyScale.y * 1.1f || Vector3.Cross(faces[(int)res].transform.forward, Vector3.down).sqrMagnitude > .01f)
            {
                RollDie(.5f);
                //print("MISTHROWN");
                return DieFace.NONE;
            }

            return res;
        }

        return DieFace.NONE;
    }
}
