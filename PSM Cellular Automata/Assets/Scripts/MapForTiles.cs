using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapForTiles 
{
    //Code Cleaned 07.04.2021

    public event EventHandler OnEntireMapRevealed;

    private CraftGrid<MapObject> grid;


    public MapForTiles()
    {
        grid = new CraftGrid<MapObject>(6, 6, 10f, Vector3.zero, (CraftGrid<MapObject> g, int x, int y) => new MapObject(g, x, y));

        int minesPlaced = 0;
        int generateMineAmount = 3;
        while (minesPlaced < generateMineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.getWidth());
            int y = UnityEngine.Random.Range(0, grid.getHeight());
            
            MapObject mapGridObject = grid.GetGridObject(x, y);
            if (mapGridObject.GetGridType() != MapObject.Type.Mine)
            {
                mapGridObject.SetGridType(MapObject.Type.Mine);
               
                minesPlaced++;
            }
        }

        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapObject.Type.Empty)
                {
                    // Calculate neighbours with mines
                    List<MapObject> neighbourList = GetNeighbourList(x, y);

                    int mineCount = 0;
                    foreach (MapObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }

                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapObject.Type.Num_1); break;
                        case 2: mapGridObject.SetGridType(MapObject.Type.MineNum_2); break;
                        case 3: mapGridObject.SetGridType(MapObject.Type.MineNum_3); break;
                        case 4: mapGridObject.SetGridType(MapObject.Type.MineNum_4); break;
                        case 5: mapGridObject.SetGridType(MapObject.Type.MineNum_5); break;
                        case 6: mapGridObject.SetGridType(MapObject.Type.MineNum_6); break;
                        case 7: mapGridObject.SetGridType(MapObject.Type.MineNum_7); break;
                        case 8: mapGridObject.SetGridType(MapObject.Type.MineNum_8); break;
                    }
                }
            }
        }

        grid.OnGridObjectChanged += CraftGrid_OnGridObjectChanged;
    }


    public MapForTiles(List<Vector3> restrictedPositionsList, int width, int height, float cellSize, Vector3 pointOfOrigin)
    {
        grid = new CraftGrid<MapObject>(width, height, cellSize, pointOfOrigin, (CraftGrid<MapObject> g, int x, int y) => new MapObject(g, x, y));

        bool IsWrondPlace = false;
        
        List<int[]> restrictedCells = restrictedPositionsList.ConvertAll(new Converter<Vector3, int[]>(grid.VectorToCell));

       

        int minesPlaced = 0;
        int generateMineAmount = 3;
        while (minesPlaced < generateMineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.getWidth());
            int y = UnityEngine.Random.Range(0, grid.getHeight());

            foreach (int[] c in restrictedCells)
            {
                if( x == c[0] && y == c[1])
                {
                   
                    IsWrondPlace = true;
                }

            }

            MapObject mapGridObject = grid.GetGridObject(x, y);
            if ((mapGridObject.GetGridType() != MapObject.Type.Mine) && IsWrondPlace == false)
            {
                mapGridObject.SetGridType(MapObject.Type.Mine);

                minesPlaced++;
            }

            else
            {
                IsWrondPlace = false;
            }
        }

        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapObject.Type.Empty)
                {
                    // Calculate neighbours with mines
                    List<MapObject> neighbourList = GetNeighbourList(x, y);

                    int mineCount = 0;
                    foreach (MapObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }

                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapObject.Type.Num_1); break;
                        case 2: mapGridObject.SetGridType(MapObject.Type.MineNum_2); break;
                        case 3: mapGridObject.SetGridType(MapObject.Type.MineNum_3); break;
                        case 4: mapGridObject.SetGridType(MapObject.Type.MineNum_4); break;
                        case 5: mapGridObject.SetGridType(MapObject.Type.MineNum_5); break;
                        case 6: mapGridObject.SetGridType(MapObject.Type.MineNum_6); break;
                        case 7: mapGridObject.SetGridType(MapObject.Type.MineNum_7); break;
                        case 8: mapGridObject.SetGridType(MapObject.Type.MineNum_8); break;
                    }
                }
            }
        }

        grid.OnGridObjectChanged += CraftGrid_OnGridObjectChanged;
    }

    public MapForTiles(int minesAmount, List<Vector3> restrictedPositionsList, int width, int height, float cellSize, Vector3 pointOfOrigin, bool showDebugGrid) //This one is used
    {
        grid = new CraftGrid<MapObject>(width, height, cellSize, pointOfOrigin, showDebugGrid, (CraftGrid<MapObject> g, int x, int y) => new MapObject(g, x, y));

        bool IsWrondPlace = false;

        List<int[]> restrictedCells = restrictedPositionsList.ConvertAll(new Converter<Vector3, int[]>(grid.VectorToCell));



        int minesPlaced = 0;
        int generateMineAmount = minesAmount;
        while (minesPlaced < generateMineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.getWidth());
            int y = UnityEngine.Random.Range(0, grid.getHeight());

            foreach (int[] c in restrictedCells)
            {
                if (x == c[0] && y == c[1])
                {

                    IsWrondPlace = true;
                }

            }

            MapObject mapGridObject = grid.GetGridObject(x, y);
            if ((mapGridObject.GetGridType() != MapObject.Type.Mine) && IsWrondPlace == false)
            {
                mapGridObject.SetGridType(MapObject.Type.Mine);

                minesPlaced++;
            }

            else
            {
                IsWrondPlace = false;
            }
        }

        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapObject.Type.Empty)
                {
                    // Calculate neighbours with mines
                    List<MapObject> neighbourList = GetNeighbourList(x, y);

                    int mineCount = 0;
                    foreach (MapObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }

                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapObject.Type.Num_1); break;
                        case 2: mapGridObject.SetGridType(MapObject.Type.MineNum_2); break;
                        case 3: mapGridObject.SetGridType(MapObject.Type.MineNum_3); break;
                        case 4: mapGridObject.SetGridType(MapObject.Type.MineNum_4); break;
                        case 5: mapGridObject.SetGridType(MapObject.Type.MineNum_5); break;
                        case 6: mapGridObject.SetGridType(MapObject.Type.MineNum_6); break;
                        case 7: mapGridObject.SetGridType(MapObject.Type.MineNum_7); break;
                        case 8: mapGridObject.SetGridType(MapObject.Type.MineNum_8); break;
                    }
                }
            }
        }

        grid.OnGridObjectChanged += CraftGrid_OnGridObjectChanged;
    }


   





    public MapForTiles(CraftGrid<MapObject> gridTaken)
    {
        this.grid = gridTaken;

        int minesPlaced = 0;
        int generateMineAmount = 4;
        while (minesPlaced < generateMineAmount)
        {
            int x = UnityEngine.Random.Range(0, grid.getWidth());
            int y = UnityEngine.Random.Range(0, grid.getHeight());
           
            MapObject mapGridObject = grid.GetGridObject(x, y);
            if (mapGridObject.GetGridType() != MapObject.Type.Mine)
            {
                mapGridObject.SetGridType(MapObject.Type.Mine);
                
                minesPlaced++;

            }
        }
       
        
        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() == MapObject.Type.Empty)
                {
                    // Calculate neighbours with mines
                    List<MapObject> neighbourList = GetNeighbourList(x, y);

                    int mineCount = 0;
                    foreach (MapObject neighbour in neighbourList)
                    {
                        if (neighbour.GetGridType() == MapObject.Type.Mine)
                        {
                            mineCount++;
                        }
                    }

                    switch (mineCount)
                    {
                        case 1: mapGridObject.SetGridType(MapObject.Type.Num_1); break;
                        case 2: mapGridObject.SetGridType(MapObject.Type.MineNum_2); break;
                        case 3: mapGridObject.SetGridType(MapObject.Type.MineNum_3); break;
                        case 4: mapGridObject.SetGridType(MapObject.Type.MineNum_4); break;
                        case 5: mapGridObject.SetGridType(MapObject.Type.MineNum_5); break;
                        case 6: mapGridObject.SetGridType(MapObject.Type.MineNum_6); break;
                        case 7: mapGridObject.SetGridType(MapObject.Type.MineNum_7); break;
                        case 8: mapGridObject.SetGridType(MapObject.Type.MineNum_8); break;
                    }
                }
            }
        }
    }

    

    public CraftGrid<MapObject> GetGrid()
    {
        return grid;
    }

    public List<MapObject> GetNeighbourList(MapObject mapGridObject)
    {
        return GetNeighbourList(mapGridObject.GetX(), mapGridObject.GetY());
    }


    public List<MapObject> GetNeighbourList(int x, int y)
    {
        List<MapObject> neighbourList = new List<MapObject>();

        if (x - 1 >= 0)
        {
            // Left
            neighbourList.Add(grid.GetGridObject(x - 1, y));
            // Left Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x - 1, y - 1));
            // Left Up
            if (y + 1 < grid.getHeight()) neighbourList.Add(grid.GetGridObject(x - 1, y + 1));
        }
        if (x + 1 < grid.getWidth())
        {
            // Right
            neighbourList.Add(grid.GetGridObject(x + 1, y));
            // Right Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x + 1, y - 1));
            // Right Up
            if (y + 1 < grid.getHeight()) neighbourList.Add(grid.GetGridObject(x + 1, y + 1));
        }
        // Up
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridObject(x, y - 1));
        // Down
        if (y + 1 < grid.getHeight()) neighbourList.Add(grid.GetGridObject(x, y + 1));


        return neighbourList;
    }


    public MapObject.Type RevealGridPosition(Vector3 worldPosition)
    {
        MapObject mapGridObject = grid.GetGridObject(worldPosition);
        return RevealGridPosition(mapGridObject);
    }

    

    public MapObject.Type RevealGridPosition(MapObject mapGridObject)
    {
        if (mapGridObject == null) return MapObject.Type.Empty;
        // Reveal this object
       /* mapGridObject.Reveal();

        // Is it an Empty grid object?
        if (mapGridObject.GetGridType() == MapObject.Type.Empty)
        {
            // Is Empty, reveal connected nodes

            // Keep track of nodes already checked
            List<MapObject> alreadyCheckedNeighbourList = new List<MapObject>();
            // Nodes queued up for checking
            List<MapObject> checkNeighbourList = new List<MapObject>();
            // Start checking this node
            checkNeighbourList.Add(mapGridObject);

            // While we have nodes to check
            while (checkNeighbourList.Count > 0)
            {
                // Grab the first one
                MapObject checkMapGridObject = checkNeighbourList[0];
                // Remove from the queue
                checkNeighbourList.RemoveAt(0);
                alreadyCheckedNeighbourList.Add(checkMapGridObject);

                // Cycle through all its neighbours
                foreach (MapObject neighbour in GetNeighbourList(checkMapGridObject))
                {
                    if (neighbour.GetGridType() != MapObject.Type.Mine)
                    {
                        // If not a mine, reveal it
                        neighbour.Reveal();
                        if (neighbour.GetGridType() == MapObject.Type.Empty)
                        {
                            // If empty, check add it to queue
                            if (!alreadyCheckedNeighbourList.Contains(neighbour))
                            {
                                checkNeighbourList.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }
    

        if (IsEntireMapRevealed())
        {
            // Entire map revealed, game win!
            OnEntireMapRevealed.Invoke(this, EventArgs.Empty);
        }
        */

        return mapGridObject.GetGridType();
    }

    private bool IsEntireMapRevealed()
    {
        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                if (mapGridObject.GetGridType() != MapObject.Type.Mine)
                {
                    if (!mapGridObject.IsRevealed())
                    {
                        // This is not a mine and is not revealed
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void RevealEntireMap()
    {
        for (int x = 0; x < grid.getWidth(); x++)
        {
            for (int y = 0; y < grid.getHeight(); y++)
            {
                MapObject mapGridObject = grid.GetGridObject(x, y);
                mapGridObject.Reveal();
            }
        }
    }


    private void CraftGrid_OnGridObjectChanged(object sender, CraftGrid<MapObject>.OnGridObjectChangedEventArgs e)
    {
        grid.debugTextArray[e.x, e.y].text = grid.gridArray[e.x, e.y].ToString();

    }

    public int[] VectorToGridCell(Vector3 worldPosition)
    {
        return grid.VectorToCell(worldPosition);
    }
}




