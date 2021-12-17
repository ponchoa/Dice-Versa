using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DieUI : MonoBehaviour
{
    private FaceUI[,] faces;
    [HideInInspector]
    public DiePhysical physDie;

    private void Start()
    {
        faces = new FaceUI[4, 3];
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                faces[x, y] = transform.Find(x + "_" + y).GetComponent<FaceUI>();
                faces[x, y].dieUI = this;
            }
        }
    }

    public void DisplayDie(DiePhysical die)
    {
        physDie = die;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                faces[x, y].SetFace(physDie.layout[x, y]);
                if (faces[x, y].Face != DieFace.NONE)
                {
                    faces[x, y].numberOfPips = physDie[(int)faces[x, y].Face].numberOfPips;
                }
            }
        }
    }

    public void IncreasePips(Transform faceUI)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (faces[x, y].transform == faceUI && faces[x, y].Face != DieFace.NONE)
                {
                    int n = (faces[x, y].numberOfPips + 1) % 10;
                    faces[x, y].numberOfPips = n;
                    physDie[(int)faces[x, y].Face].numberOfPips = n;
                }
            }
        }
    }
    public void DecreasePips(Transform faceUI)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (faces[x, y].transform == faceUI && faces[x, y].Face != DieFace.NONE)
                {
                    int n = (faces[x, y].numberOfPips - 1) % 10;
                    faces[x, y].numberOfPips = n;
                    physDie[(int)faces[x, y].Face].numberOfPips = n;
                }
            }
        }
    }
    public void SetPipsValue(Transform faceUI, int n)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (faces[x, y].transform == faceUI && faces[x, y].Face != DieFace.NONE)
                {
                    n %= 10;
                    faces[x, y].numberOfPips = n;
                    physDie[(int)faces[x, y].Face].numberOfPips = n;
                }
            }
        }
    }
}
