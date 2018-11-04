using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenerationController
{
    private static GenerationController instance;
    private int currentIndex;
    private List<Generation> generationList;
    private GameObject[,] gameObjectMatrix;

    public delegate void OnGenerationChanged();
    public event OnGenerationChanged GenerationChangedObserver;

    public int RowsCount
    {
        get
        {
            return gameObjectMatrix.GetLength(0);
        }
    }

    public int ColCount
    {
        get
        {
            return gameObjectMatrix.GetLength(1);
        }
    }

    public void InitializeMatrix(int rows, int cols)
    {
        Debug.Log("this");
        gameObjectMatrix = new GameObject[rows, cols];
    }


    public int GenerationCount { get; private set; }
    public int CellCount { get; private set; }

    private GenerationController()
    {
        generationList = new List<Generation>();
        generationList.Add(null);
        currentIndex = 0;
        GenerationCount = 0;
    }

    public static GenerationController GetInstance()
    {
        if (instance == null)
            instance = new GenerationController();

        return instance;
    }

    public void GameObjectMatrix(GameObject[,] matrix)
    {
        gameObjectMatrix = matrix;
        if (currentIndex < generationList.Count - 1)
        {
            generationList.RemoveRange(currentIndex + 1, generationList.Count - currentIndex - 1);
        }

        generationList[currentIndex] = new Generation(matrix.GetLength(0), matrix.GetLength(1));

    }

    public bool CheckIfThisTheEnd()
    {
        bool result = true;
        if (generationList.Count < 2)
            result =  false;

        if (generationList[generationList.Count - 1] == generationList[generationList.Count - 2])
            result = true;
        else
            result = false;

        if (generationList.Count >3 && generationList[generationList.Count - 1] == generationList[generationList.Count - 3])
            result = true;
        else
            result = false;

        return result;
    }



    public void HideObjectsWithGreyColor()
    {
        Debug.Log("This");

        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {

                if (gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color == Color.white)
                {
                    gameObjectMatrix[i, j].SetActive(false);
                    generationList[currentIndex].ChangeCellData(i, j, false);
                }
                else
                {
                    generationList[currentIndex].ChangeCellData(i, j, true);
                    gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
        }
    }

    public void SetCurrentGenerationData(Generation generation)
    {
        if (generationList.Count == 0)
        {
            generationList.Add(null);
        }
        generationList[currentIndex] = generation;
        ShowRedCells(currentIndex);
    }

    private void ShowRedCells(int index)
    {
        GenerationCount = 0;
        CellCount = 0;
        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {
                if (generationList[index].GetCellData(i, j).CewllColor == Color.red)
                {
                    gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color = generationList[index].GetCellData(i, j).CewllColor;
                    CellCount++;
                }
                else
                    gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        GenerationChangedObserver();
    }

    public void ClearAllCells()
    {
        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {
                generationList[currentIndex].ChangeCellData(i, j, false);
                generationList[currentIndex].ChangeCellData(i, j, Color.white);
            }
        }

        ChangMatrixGO(currentIndex);
    }



    public void Play()
    {
        CheckGenerationRules();
    }

    public void Next()
    {
        if (currentIndex > generationList.Count - 2)
            return;

        currentIndex++;
        ChangMatrixGO(currentIndex);
    }

    public void Pre()
    {
        if (currentIndex == 0)
            return;
        currentIndex--;
        ChangMatrixGO(currentIndex);
    }

    public void Stop()
    {
        ShowFiled();
    }

    private void ShowFiled()
    {
        if (gameObjectMatrix == null)
            return;

        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {
                if (!gameObjectMatrix[i, j].activeInHierarchy)
                {
                    gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color = Color.white;
                    gameObjectMatrix[i, j].SetActive(true);
                }
            }
        }
    }

    private void CheckGenerationRules()
    {
        Generation newGeneration = new Generation(gameObjectMatrix.GetLength(0), gameObjectMatrix.GetLength(1));
        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {
                if (gameObjectMatrix[i, j].activeInHierarchy)
                    CheckIsCellWillBeLive(i, j, newGeneration);
                else
                    CheckIsSellWillBorn(i, j, newGeneration);
            }
        }

        generationList.Add(newGeneration);
        GenerationCount = generationList.Count - 1;
        currentIndex++;

        ChangMatrixGO(currentIndex);
    }

    private void ChangMatrixGO(int listIndex)
    {
        if (listIndex < 0 || listIndex > generationList.Count - 1)
            return;

        CellCount = 0;
        for (int i = 0; i < gameObjectMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameObjectMatrix.GetLength(1); j++)
            {

                gameObjectMatrix[i, j].SetActive(generationList[listIndex].GetCellData(i, j).IsAlive);
                gameObjectMatrix[i, j].GetComponent<MeshRenderer>().material.color = generationList[listIndex].GetCellData(i, j).CewllColor;

                if (gameObjectMatrix[i, j].activeInHierarchy)
                    CellCount++;
            }
        }

        GenerationChangedObserver();
    }

    private void CheckIsSellWillBorn(int rowIndex, int colIndex, Generation generation)
    {
        int neighborsCount = GetNeighbornsCount(rowIndex, colIndex);

        if (neighborsCount == 3)
        {
            generation.ChangeCellData(rowIndex, colIndex, true);
            generation.ChangeCellData(rowIndex, colIndex, Color.red);
        }

    }

    private void CheckIsCellWillBeLive(int rowIndex, int colIndex, Generation generation)
    {
        int neighborsCount = GetNeighbornsCount(rowIndex, colIndex);
        if (neighborsCount < 2 || neighborsCount > 3)
        {
            generation.ChangeCellData(rowIndex, colIndex, false);
        }
        else
            generation.ChangeCellData(rowIndex, colIndex, true);
    }

    private int GetNeighbornsCount(int rowIndex, int colIndex)
    {
        int neighborsCount = 0;
        for (int i = rowIndex - 1; i <= rowIndex + 1; i++)
        {
            for (int j = colIndex - 1; j <= colIndex + 1; j++)
            {
                if (i >= 0
                    && j >= 0
                    && i < gameObjectMatrix.GetLength(0)
                    && j < gameObjectMatrix.GetLength(1)
                    && !(i == rowIndex && j == colIndex))
                {

                    if (gameObjectMatrix[i, j].activeInHierarchy)
                        neighborsCount++;


                }
            }
        }

        return neighborsCount;
    }



}
