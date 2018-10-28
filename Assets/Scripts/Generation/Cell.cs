using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    private bool isNew;
    private GameObject prefab;
    private Color color;


    public bool IsAlive { get; set; }
    public Color CewllColor { get; set; }
    public Cell(bool isAlive)
    {
        IsAlive = isAlive;
    }
}
