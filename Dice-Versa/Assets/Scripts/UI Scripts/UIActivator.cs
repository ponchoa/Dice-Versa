using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivator : MonoBehaviour
{
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Notes = "This component shouldn't be removed, it activates the UI at the start even if it's disabled, to make the workflow easier.";

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
