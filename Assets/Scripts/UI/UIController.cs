using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text generationCount;
    public Text cellCount;
    private GenerationController controller;
    public GameContrroller gameController;
    public Text timeToNewGeneration;

    private void Start()
    {
        controller = GenerationController.GetInstance();
        controller.GenerationChangedObserver += UpdateUI;
        timeToNewGeneration.text = gameController.TIME_TO_NEXT_GENERATION.ToString();
    }

    public void ChangeTimeToNewGeneration()
    {
        float result;
        if (float.TryParse(timeToNewGeneration.text, out result) && result > 0)
            gameController.TIME_TO_NEXT_GENERATION = result;
        //else
        //{
        //    timeToNewGeneration.text = gameController.TIME_TO_NEXT_GENERATION.ToString() ; 
        //    Debug.Log(gameController.TIME_TO_NEXT_GENERATION);
        //}
           
    }

    private void UpdateUI()
    {
        generationCount.text = controller.GenerationCount.ToString();
        cellCount.text = controller.CellCount.ToString();
    }


}
