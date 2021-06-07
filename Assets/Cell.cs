using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Grid grid;

    public int x;
    public int y;
    public int neighbourBombs;
    public Sprite spriteFlag;
    public Sprite defaultBackground;

    //public Color[] colors;
    public bool isBomb;
    public bool isFlag;
    public bool isBorder;

    [Space]
    [SerializeField] private Image background;
    [SerializeField] private Text bombCountText;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    public void SetBomb()
    {
        isBomb = true;
    }

    public void SetBorder()
    {
        isBorder = true;
    }

    public void SetFlag()
    {
        if (isFlag)
        {
            isFlag = false;
            background.sprite = defaultBackground;
            
        }
        else
        {
            isFlag = true;
            background.sprite = spriteFlag;
        }
    }

    public bool isOpen{ get; private set; }
    public void Open()
    {
        isOpen = true;
        background.enabled = false;
        if (isBomb)
        {
            bombCountText.text = "*";
            bombCountText.color = Color.red;
            return;
        }

        bombCountText.text = neighbourBombs > 0 ? neighbourBombs.ToString() : "";
        if (neighbourBombs > 0)
            bombCountText.color = Color.green;
        if (neighbourBombs > 2)
            bombCountText.color = Color.blue;
        if (neighbourBombs > 4)
            bombCountText.color = Color.yellow;
        if (neighbourBombs > 6)
            bombCountText.color = Color.red;
    }

    public void CellClick()
    {
        grid.CellClick(this);
    }

    public void SetCharBorder()
    {
        bombCountText.text = "#";
    }

    
}

