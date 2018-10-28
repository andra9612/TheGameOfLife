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
        HideObjectsWithGreyColor();
    }

    private void HideObjectsWithGreyColor()
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
        GenerationCount = generationList.Count-1;
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
