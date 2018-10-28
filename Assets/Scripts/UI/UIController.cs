using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text generationCount;
    public Text cellCount;
    private GenerationController controller;
    private void Start()
    {
        controller = GenerationController.GetInstance();
        controller.GenerationChangedObserver += UpdateUI;
    }

    private void UpdateUI()
    {
        generationCount.text = controller.GenerationCount.ToString();
        cellCount.text = controller.CellCount.ToString();
    }


}
