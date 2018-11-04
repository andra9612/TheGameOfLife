using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Generation {
    private Cell[,] cellMatrix;
    private int columns;
    private int rows;

    public Generation(int rowsCount, int columsCount)
    {
        columns = columsCount;
        rows = rowsCount;
        cellMatrix = new Cell[rows, columns];
        InitializeCellMatrix();
    }


    private void InitializeCellMatrix()
    {
        for (int i = 0; i < cellMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < cellMatrix.GetLength(1); j++)
            {
                cellMatrix[i, j] = new Cell(false);
            }
        }
    }

    public void ChangeCellData(int currentRow, int currentCol, bool isAlive)
    {
        if (CheckIsIndexInRange(currentRow, rows) && CheckIsIndexInRange(currentCol, columns))
        {
            cellMatrix[currentRow, currentCol].IsAlive = isAlive;
        }
    }

    public void ChangeCellData(int currentRow, int currentCol, Color color)
    {
        if (CheckIsIndexInRange(currentRow, rows) && CheckIsIndexInRange(currentCol, columns))
        {
            cellMatrix[currentRow, currentCol].CewllColor = color;
        }
    }

    public Cell GetCellData(int rowIndex, int colIndex)
    {
        return cellMatrix[rowIndex, colIndex];
    }

    private bool CheckIsIndexInRange(int currValue, int maxValue)
    {
        return currValue >= 0 && currValue < maxValue;
    }

    public static bool operator ==(Generation one, Generation two)
    {
        for (int i = 0; i < one.rows; i++)
        {
            for (int j = 0; j < one.columns; j++)
            {
                if ((one.cellMatrix[i, j].IsAlive && !two.cellMatrix[i, j].IsAlive)
                    || (!one.cellMatrix[i, j].IsAlive && two.cellMatrix[i, j].IsAlive))
                    return false;
            }
        }

        return true;
    }

    public static bool operator !=(Generation one, Generation two)
    {
        for (int i = 0; i < one.rows; i++)
        {
            for (int j = 0; j < one.columns; j++)
            {
                if ((one.cellMatrix[i, j].IsAlive && !two.cellMatrix[i, j].IsAlive)
                    || (!one.cellMatrix[i, j].IsAlive && two.cellMatrix[i, j].IsAlive))
                    return false;
            }
        }

        return false;
    }

}
