using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AutomataSpawner : MonoBehaviour
{

    private CraftGrid<GameObject> grid;
    private Vector3 middleoffset;
    private Rect area;


    [Header("Prefabs")]
    public GameObject inhabitedCellPrefab;
    public GameObject emptyCellPrefab;

    [Space]

    [Header("Parameters")]
    public bool showDebug;
    public int cellSize;
    public int width;
    public int height;
    public Vector3 originPosition;
    


    [Space]

    [Header("Automata Rules")]
    public bool enableUnderpopulation;
    public int underpopulation;
    public bool enableSurvival;
    public int survival;
    public bool enableOverpopulation;
    public int overpopulation;
    public bool enableReproduction;
    public int reproduction;

    [Space]

    [Header("Other objects")]
    public Camera cameraMain;
    public GameObject reproductionCondition;
    public GameObject survivalCondition;
    public GameObject overpopulationCondition;


    //Is called before starting the scene
    private void Awake()
    {
        middleoffset = new Vector3(cellSize/2, cellSize/2);
        area = new Rect(cameraMain.WorldToScreenPoint(originPosition), cameraMain.WorldToScreenPoint(new Vector3(width * cellSize, height * cellSize) ));
        

         grid = new CraftGrid<GameObject>(width, height, cellSize, originPosition, showDebug, (CraftGrid<GameObject> g, int x, int y) => Instantiate(emptyCellPrefab, cellToWorld(x, y), Quaternion.identity));

        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        reproductionCondition.GetComponent<TMP_InputField>().text = reproduction+"";
        survivalCondition.GetComponent<TMP_InputField>().text = survival + "";
        overpopulationCondition.GetComponent<TMP_InputField>().text = overpopulation + "";



    }

    // Update is called once per frame
    void Update()
    {
       
        int.TryParse(reproductionCondition.GetComponent<TMP_InputField>().text, out reproduction);
        int.TryParse(survivalCondition.GetComponent<TMP_InputField>().text, out survival);
        int.TryParse(overpopulationCondition.GetComponent<TMP_InputField>().text, out overpopulation);
        
        

        AddAliveCell();
        KillAliveCell();
       

        
    }

    public Vector3 cellToWorld(int x, int y)
    {
        return ((new Vector3(x, y, 0) * cellSize + originPosition) + middleoffset);
    }

    public void ChangeCell(int x, int y, GameObject newObjectPrefab)
    {

        GameObject newObject = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity);
        newObject.SetActive(false);

        GameObject oldObject = grid.GetGridObject(x, y);

        newObject.transform.position = oldObject.transform.position;
        newObject.SetActive(true);
        Destroy(oldObject);

        grid.SetGridObject(x, y, newObject);

    }

    public void AddAliveCell()
    {
        if (Input.GetMouseButtonDown(0))
        {

            

            Vector3 clickPosition = Input.mousePosition; 
            if(area.Contains(clickPosition))
            {
                Vector3 clickWorldPosition = cameraMain.ScreenToWorldPoint(clickPosition);
                int[] gridCoordinates = grid.VectorToCell(clickWorldPosition);
               

                if (grid.GetGridObject(gridCoordinates[0], gridCoordinates[1]).name.Equals("DeadCell(Clone)"))
                {

                    ChangeCell(gridCoordinates[0], gridCoordinates[1], inhabitedCellPrefab);
                }
                else
                {
                    Debug.Log("This cell is already alive!");
                }
            }
        }
    }


    public void KillAliveCell()
    {


        if (Input.GetMouseButtonDown(1))
        {
           

            Vector3 clickPosition = Input.mousePosition;
            if (area.Contains(clickPosition))
            {
                Vector3 clickWorldPosition = cameraMain.ScreenToWorldPoint(clickPosition);
                int[] gridCoordinates = grid.VectorToCell(clickWorldPosition);
               

                if (grid.GetGridObject(gridCoordinates[0], gridCoordinates[1]).name.Equals("AliveCell(Clone)"))
                {
                    ChangeCell(gridCoordinates[0], gridCoordinates[1], emptyCellPrefab);
                }
                else
                {
                    Debug.Log("This cell is already dead!");
                }
            }
        }
    }


    public void PlayAutomataIteration()
    {
        List<GameObject> cellsToReproduce = new List<GameObject>();
        List<GameObject> cellsToDie = new List<GameObject>();


        foreach (GameObject cell in grid.gridArray)
        {
            if (cell.name.Equals("DeadCell(Clone)"))
            {
                int[] gridCoordinates = grid.VectorToCell(cell.transform.position);

                List<GameObject> neighbours = grid.GetNeighbourList(gridCoordinates[0], gridCoordinates[1]);

                List<GameObject> aliveNeighbours = neighbours.FindAll((neighbour) => neighbour.name.Equals("AliveCell(Clone)"));





                if (enableReproduction)
                {
                    if (aliveNeighbours.Count >= reproduction)
                    {
                        cellsToReproduce.Add(cell);
                      
                    }

                }
            }
        }


        foreach(GameObject cell in grid.gridArray)
        {
            if(cell.name.Equals("AliveCell(Clone)"))
            {

                int[] gridCoordinates = grid.VectorToCell(cell.transform.position);

                List<GameObject> neighbours = grid.GetNeighbourList(gridCoordinates[0], gridCoordinates[1]);

                List<GameObject> aliveNeighbours = neighbours.FindAll((neighbour) => neighbour.name.Equals("AliveCell(Clone)"));

                if (enableSurvival)
                {
                    if(aliveNeighbours.Count < survival)
                    {
                        cellsToDie.Add(cell);
                    }
                    
                }

                if(enableOverpopulation)
                {
                    if(aliveNeighbours.Count > overpopulation)
                    {
                        cellsToDie.Add(cell);
                        
                    }

                }
            }
            
        }


        foreach(GameObject cell in cellsToReproduce)
        {
            int[] gridCoordinates = grid.VectorToCell(cell.transform.position);
            ChangeCell(gridCoordinates[0], gridCoordinates[1], inhabitedCellPrefab);
        }

        foreach(GameObject cell in cellsToDie)
        {
            int[] gridCoordinates = grid.VectorToCell(cell.transform.position);
            ChangeCell(gridCoordinates[0], gridCoordinates[1], emptyCellPrefab);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
