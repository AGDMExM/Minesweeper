using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int size = 12;
    [SerializeField] private int bombs = 10;
    public KeyCode RightKey;
    public KeyCode LeftKey;
    private Cell[,] field;

    private void Awake()
    {
        InitializeField();
        InitializeBombs();
        InitializeNeighboursBombsCount();
    }

    public void CellClick(Cell cell)
    {
        if (cell.isBorder)
        {
            return;
        }

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
                field[i, j] = Instantiate(cellPrefab, transform);
                field[i, j].x = i;
                field[i, j].y = j;
                if(i == 0 || j == 0 || i == size - 1 || j == size - 1)
                {
                    field[i, j].SetCharBorder();
                    field[i, j].SetBorder();
                }
            }
        }
    }

    private void InitializeBombs()
    {
        int count = 0;
        while(count < bombs)
        {
            int position = Random.Range(0, size * size);
            int x = position / size;
            int y = position % size;

            if(!field[x, y].isBomb && !field[x, y].isBorder)
            {
                field[x, y].SetBomb();
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
                if(x + i < 0 || x + i >= size || y + j < 0 || y + j >= size)
                    continue;

                if(field[x + i, y + j].isBomb)
                count++;
            }
        }
        return count;
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

            for(int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++)
                {
                    int x = current.x + i;
                    int y = current.y + j;

                    if(x < 0 || x >= size || y < 0 || y >= size)
                        continue;

                    if(!field[x, y].isBomb && !field[x, y].isOpen && !field[x, y].isBorder)
                        visite.Add(field[x, y]);
                }
        }
    }
}
