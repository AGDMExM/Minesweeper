using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int size = 12;
    [SerializeField] private int bombs = 10;
    private Cell[,] field;

    private bool firstClick = true;

    private void Awake()
    {
        InitializeField();
        
    }

    private void Update()
    {
        if (GetCountCloseCell() == 10 + 44) // 10 - bombs, 44 - count border cells
        {
            OpenAll();
            Debug.LogError("You Win");
            return;
        }
    }

    public void CellClick(Cell cell)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (cell.isBorder)
            {
                return;
            }

            if (firstClick)
            {
                firstClick = false;
                List<Cell> arrayForBomb = new List<Cell>();
                List<int> arrNumberPosCells = new List<int>();

                for (int i = 0; i < size * size - 1; i++)
                {
                    if(cell.x * size + cell.y != i)
                        arrNumberPosCells.Add(i);
                }

                while(arrNumberPosCells.Count - 1 != 0)
                {
                    System.Random rnd = new System.Random();
                    int posCell = arrNumberPosCells[rnd.Next(0, arrNumberPosCells.Count - 1)];
                    //int i = posCell / size;
                    //int j = posCell % size;
                    //if (posCell / size != cell.y && posCell % size != cell.x)
                    arrayForBomb.Add(field[posCell / size, posCell % size]);
                    arrNumberPosCells.Remove(posCell);
                }

                InitializeBombs(arrayForBomb);
                InitializeNeighboursBombsCount();
            }

            if (!cell.isFlag)
            {
                if (cell.isBomb)
                {
                    OpenAll();
                    Debug.LogError("Game over");
                    return;
                }

                if (cell.neighbourBombs == 0)
                {
                    OpenNeighbourFree(cell);
                    return;
                }
                cell.Open();

                if (GetCountCloseCell() == 10 + 44)
                {
                    OpenAll();
                    Debug.LogError("You Win");
                    return;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (!cell.isOpen && !cell.isBorder)
            {
                CellFlag(cell);
            }
        }

    }

    private void CellFlag(Cell cell)
    {
        cell.SetFlag();
    }
    
    private int GetCountCloseCell()
    {
        int count = 0;
        for(int i=0; i < size; i++)
            for(int j=0; j < size; j++)
            {
                if(field[i, j].isOpen == false)
                    count++;
            }
        return count;
    }

    private void InitializeField()
    {
        field = new Cell[size, size];
        for(int i=0; i < size; i++)
        {
            for(int j=0; j < size; j++)
            {
                Cell cell = field[i, j];
                cell = Instantiate(cellPrefab, transform);
                cell.x = i;
                cell.y = j;
                if(i == 0 || j == 0 || i == size - 1 || j == size - 1)
                {
                    cell.SetCharBorder();
                    cell.SetBorder();
                }
                field[i, j] = cell;
            }
        }
    }

    private void InitializeBombs(List<Cell> listForBomb)
    {
        int count = 0;
        while (count < bombs)
        {
            System.Random rnd = new System.Random();
            int pos = rnd.Next(0, listForBomb.Count - 1);
            if (!listForBomb[pos].isBorder)
            {
                listForBomb[pos].SetBomb();
                listForBomb.Remove(listForBomb[pos]);
                count++;
            }
        }
    }

    private void InitializeNeighboursBombsCount()
    {
        for(int i=0; i < size; i++)
        {
            for(int j=0; j < size; j++)
            {
                field[i, j].neighbourBombs = GetNeighboursBombsCount(i, j);
            }
        }
    }

    private int GetNeighboursBombsCount(int x, int y)
    {
        
        int count = 0;
        for(int i= -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (IsOutsideGrid(x, y))
                    continue;

                if(field[x + i, y + j].isBomb)
                count++;
            }
        }
        return count;
    }

    private bool IsOutsideGrid(int x, int y)
    {
        return x - 1 < 0 || x + 1 >= size || y - 1 < 0 || y + 1 >= size;
    }

    private void OpenAll()
    {
        for(int i=0; i < size; i++)
            for(int j=0; j < size; j++)
            {
                if(!field[i, j].isBorder)
                    field[i, j].Open();
            }
    }

    private void OpenNeighbourFree(Cell cell)
    {
        List<Cell> visite = new List<Cell>();
        visite.Add(cell);

        while(visite.Count > 0)
        {
            Cell current = visite[0];
            visite.RemoveAt(0);

            current.Open();
            if(current.neighbourBombs > 0)
                continue;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int x = current.x + i;
                    int y = current.y + j;
                    Cell cellInBorder = field[x, y];

                    if (x < 0 || x >= size || y < 0 || y >= size)
                        continue;

                    if (!cellInBorder.isBomb && !cellInBorder.isOpen && !cellInBorder.isBorder)
                        visite.Add(cellInBorder);
                }
            }
        }
    }
}
