using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerationContext{
    private IStrategy strategy;
    public void SetNewStrategy(IStrategy newStrategy)
    {
        strategy = newStrategy;
    }

    public void CreateRandomGeneration()
    {
        strategy.CreateFirstGeneration();
    }
}
