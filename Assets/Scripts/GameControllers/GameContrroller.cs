using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContrroller : MonoBehaviour {

    public GameObject cellPrefab;
    public Transform parent;
    public int rows;
    public int cols;
    public float TIME_TO_NEXT_GENERATION;
    GameObject[,] matrix;
    GenerationController controller;
    private bool isStarted = false;

    private void Initialize()
    {
        matrix = new GameObject[rows, cols];
        controller = GenerationController.GetInstance();
       
    }

    private void Start()
    {
        Initialize();
        GenerateField();
        controller.GameObjectMatrix(matrix);
        StartCoroutine(GetNextGeneration());
    }

    private IEnumerator GetNextGeneration()
    {
        if (!isStarted)
        {
            yield return new WaitUntil(() => isStarted);
            controller.GameObjectMatrix(matrix);
            controller.HideObjectsWithGreyColor();
        }

        PlayLife();
        yield return new WaitForSeconds(TIME_TO_NEXT_GENERATION);
        if (controller.CellCount == 0 || controller.CheckIfThisTheEnd())
            PlayGame();
        StartCoroutine(GetNextGeneration());
    }

    private void PlayLife()
    {
        Invoker invoker = new Invoker();
        GenerationReceiver receiver = new GenerationReceiver();
        invoker.SetCommand(new PlayCommand(receiver));
        invoker.Execute();
    }

    public void MakeNewGeneration()
    {
        isStarted = false;
        Invoker invoker = new Invoker();
        GenerationReceiver receiver = new GenerationReceiver();
        invoker.SetCommand(new NextCommand(receiver));
        invoker.Execute();
        StopGeneration();
    }

    public void PreGeneratin()
    {
        isStarted = false;
        Invoker invoker = new Invoker();
        GenerationReceiver receiver = new GenerationReceiver();
        invoker.SetCommand(new PreCommand(receiver));
        invoker.Execute();
        StopGeneration();
    }


    private void GenerateField()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i,j] = Instantiate(cellPrefab, new Vector3(i, j, 1), Quaternion.identity, parent);
            }
        }
    }


    public void PlayGame()
    {
        isStarted = !isStarted;
        if (!isStarted)
            StopGeneration();
    }

    private void StopGeneration()
    {
        Invoker invoker = new Invoker();
        GenerationReceiver receiver = new GenerationReceiver();
        invoker.SetCommand(new StopCommand(receiver));
        invoker.Execute();
    }

    public void SetRandomGeneration()
    {
        if (isStarted)
            return;
        RandomGenerationContext context = new RandomGenerationContext();
        context.SetNewStrategy(new FullRandomStrategy());
        context.CreateRandomGeneration();
    }

    private void Update()
    {
        LeftMouseButtonClick();
        RightMouseButtonClick();
        ExitGame();
    }

    private void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void LeftMouseButtonClick()
    {
        if (Input.GetMouseButtonDown(0) && !isStarted)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<MeshRenderer>().material.color == Color.white)
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    private void RightMouseButtonClick()
    {
        if (Input.GetMouseButtonDown(1) && !isStarted)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<MeshRenderer>().material.color != Color.white)
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
    }
}
