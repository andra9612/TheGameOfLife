using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationReceiver
{

    public void GetNextGeneration()
    {
        GenerationController controller = GenerationController.GetInstance();
        controller.Next();
    }

    public void GetPreGeneration()
    {
        GenerationController controller = GenerationController.GetInstance();
        controller.Pre();
    }

    public void PlayLife()
    {
        GenerationController controller = GenerationController.GetInstance();
        controller.Play();
    }

    public void StopGeneration()
    {
        GenerationController controller = GenerationController.GetInstance();
        controller.Stop();
    }

}
