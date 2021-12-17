using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePhysical : MonoBehaviour
{
    [SerializeField]
    private DieFace face;
    public DieFace Face { get { return face; } }

    [Range(0, 9)]
    public int numberOfPips = 0;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (rend)
        {
            rend.material.SetFloat("_NumberOfPips", numberOfPips);
        }
    }
}
