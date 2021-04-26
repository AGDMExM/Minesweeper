using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public int neighbourBombs;
    public bool isBomb{get; private set; }
    public bool isBorder{get; private set; }
    public void SetBomb()
    {
        isBomb = true;
    }

    public void SetBorder()
    {
        isBorder = true;
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
        }
        else
        {
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
    }

    public void CellClick()
    {
        grid.CellClick(this);
    }

    [Space]
    [SerializeField] private Image background;
    [SerializeField] private Text bombCountText;

    public void SetCharBorder()
    {
        bombCountText.text = "#";
    }

    private Grid grid;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }
}

