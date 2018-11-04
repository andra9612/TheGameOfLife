using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullRandomStrategy : IStrategy
{
    private GenerationController controller;

    public FullRandomStrategy()
    {
        controller = GenerationController.GetInstance();
    }

    public void CreateFirstGeneration()
    {
        Generation newGeneration = new Generation(controller.RowsCount,controller.ColCount);
        for (int i = 0; i < controller.RowsCount; i++)
        {
            for (int j = 0; j < controller.ColCount; j++)
            {
                if(Random.Range(0,2) == 1)
                {
                    //newGeneration.ChangeCellData(i,j,true);
                    newGeneration.ChangeCellData(i, j, Color.red);
                }
                   
            }
        }

        controller.SetCurrentGenerationData(newGeneration);
    }
}
