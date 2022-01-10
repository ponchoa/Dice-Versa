using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceUI : MonoBehaviour, IPointerClickHandler, IScrollHandler
{
    [SerializeField]
    private DieFace face;
    public DieFace Face { get { return face; } }

    [HideInInspector]
    public DieUI dieUI;

    [Range(0, 9)]
    public int numberOfPips = 0;
    [Range(0, 15)]
    public int faceID = 0;

    private Image pipsImage;
    private Image faceImage;
    private Image backImage;

    public Sprite[] pipSprites;
    public Sprite[] faceSprites;

    private void Start()
    {
        if (transform.childCount > 1)
        {
            faceImage = transform.GetChild(0).GetComponent<Image>();
            pipsImage = transform.GetChild(1).GetComponent<Image>();
        }
        backImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (face == DieFace.NONE)
        {
            if (pipsImage)
            {
                pipsImage.color = new Color(1f, 1f, 1f, 0f);
            }
            if (faceImage)
            {
                faceImage.color = new Color(1f, 1f, 1f, 0f);
            }
            if (backImage)
            {
                backImage.color = new Color(1f, 1f, 1f, 0f);
                backImage.raycastTarget = false;
            }
        }
        else
        {
            if (pipsImage)
            {
                if (numberOfPips > 0 && numberOfPips <= 9)
                {
                    pipsImage.color = new Color(1f, 1f, 1f, 1f);
                    pipsImage.sprite = pipSprites[numberOfPips - 1];
                }
                else
                {
                    pipsImage.color = new Color(1f, 1f, 1f, 0f);
                }
            }
            if (faceImage)
            {
                faceImage.color = new Color(1f, 1f, 1f, 1f);
                faceImage.sprite = faceSprites[faceID];

            }
            if (backImage)
            {
                backImage.color = new Color(1f, 1f, 1f, 1f);
                backImage.raycastTarget = true;

            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (face == DieFace.NONE)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            numberOfPips++;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            numberOfPips--;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            numberOfPips = 0;
        }

        numberOfPips %= 10;
        dieUI.physDie[(int)face].numberOfPips = numberOfPips;
    }

    /// <summary>
    /// To change the value of this face. ONLY USED BY DieUI WHEN LOADING A DIE!
    /// </summary>
    /// <param name="newFace">The location of the face</param>
    public void SetFace(DieFace newFace)
    {
        face = newFace;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (face == DieFace.NONE)
            return;
        if (eventData.scrollDelta.y > 0f)
        {
            faceID++;
        }
        else if (eventData.scrollDelta.y < 0f)
        {
            faceID--;
        }
        faceID += 16;
        faceID %= 16;
        dieUI.physDie[(int)face].faceID = faceID;
    }
}
