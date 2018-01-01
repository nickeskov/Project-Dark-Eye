using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class HuntAndKillAlgoritm : MazeAlgoritm
{
    private int currentRow = 0;
    private int currentColumn = 0;
    private bool CourseVisited = false;

    public HuntAndKillAlgoritm(MazeCell[,] mazecells) : base(mazecells)
    {
    }

    public override void CreateMaze()
    {
        HuntAndKill();
    }

    private void HuntAndKill()
    {
        MazeCells[currentRow, currentColumn].visited = true;

        while (!CourseVisited)
        {
            Kill();
            Hunt();
        }
    }


    private void Kill()
    {
        while (RouteStillAvailiable(currentRow, currentColumn))
        {
            int direction = Random.Range(1,5);
            //int direction = NumberGenerator.GetNextNumber();

            if (direction == 1 && CellAvailiable(currentRow - 1, currentColumn))
            {
                //North
                DestroyWallIfExists(MazeCells[currentRow - 1, currentColumn].SoughtWall);
                DestroyWallIfExists(MazeCells[currentRow, currentColumn].NorthWall);
                currentRow--;
            }

            else if (direction == 2 && CellAvailiable(currentRow + 1, currentColumn))
            {
                // South
                DestroyWallIfExists(MazeCells[currentRow + 1, currentColumn].NorthWall);
                DestroyWallIfExists(MazeCells[currentRow, currentColumn].SoughtWall);
                currentRow++;
            }
            else if (direction == 3 && CellAvailiable(currentRow, currentColumn - 1))
            {
                //west
                DestroyWallIfExists(MazeCells[currentRow, currentColumn - 1].EastWall);
                DestroyWallIfExists(MazeCells[currentRow, currentColumn].WestWall);
                currentColumn--;
            }

            else if (direction == 4 && CellAvailiable(currentRow, currentColumn + 1))
            {
                //east
                DestroyWallIfExists(MazeCells[currentRow, currentColumn + 1].WestWall);
                DestroyWallIfExists(MazeCells[currentRow, currentColumn].EastWall);
                currentColumn++;
            }
            MazeCells[currentRow, currentColumn].visited = true;
        }

    }

    private void Hunt()
    {
        CourseVisited = true;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (MazeCells[r, c].visited || !CellAdjustmentAcess(r, c)) continue;

                CourseVisited = false;
                currentColumn = c; currentRow = r;
                DestroyAdjacentWall(currentRow, currentColumn);
                MazeCells[r, c].visited = true;
                return;
            }
        }
    }

    private bool RouteStillAvailiable(int row, int column)
    {
        int routesAvailiable = 0;

        if (row > 0 && !MazeCells[row - 1, column].visited)
        {
            routesAvailiable++;
        }

        if (row < Rows - 1 && !MazeCells[row + 1, column].visited)
        {
            routesAvailiable++;
        }

        if (column > 0 && !MazeCells[row, column - 1].visited)
        {
            routesAvailiable++;
        }

        if (column < Columns - 1 && !MazeCells[row, column + 1].visited)
        {
            routesAvailiable++;
        }

        return routesAvailiable > 0;
    }

    private bool CellAvailiable(int row, int column)
    {
        return row >= 0 && row < Rows && column >= 0 && column < Columns && !MazeCells[row, column].visited;
    }

    private void DestroyWallIfExists(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.DestroyImmediate(wall);
        }
    }

    private bool CellAdjustmentAcess(int row, int column)
    {
        int visitedCells = 0;

        if (row > 0 && MazeCells[row - 1, column].visited)
        {
            visitedCells++;
        }

        if (row < Rows - 2 && MazeCells[row + 1, column].visited)
        {
            visitedCells++;
        }

        if (column > 0 && MazeCells[row, column - 1].visited)
        {
            visitedCells++;
        }

        if (column < Columns - 2 && MazeCells[row, column + 1].visited)
        {
            visitedCells++;
        }

        return visitedCells > 0;
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while (!wallDestroyed)
        {
            int direction = Random.Range(1, 4);
            // int direction = NumberGenerator.GetNextNumber();
            if (direction == 1 && row > 0 && MazeCells[row - 1, column].visited)
            {
                DestroyWallIfExists(MazeCells[row - 1, column].SoughtWall);
                DestroyWallIfExists(MazeCells[row, column].NorthWall);
                wallDestroyed = true;
            }

            if (direction == 2 && row < Rows - 2 && MazeCells[row + 1, column].visited)
            {
                DestroyWallIfExists(MazeCells[row + 1, column].NorthWall);
                DestroyWallIfExists(MazeCells[row, column].SoughtWall);
                wallDestroyed = true;
            }

            if (direction == 3 && column > 0 && MazeCells[row, column - 1].visited)
            {
                DestroyWallIfExists(MazeCells[row, column - 1].EastWall);
                DestroyWallIfExists(MazeCells[row, column].WestWall);
                wallDestroyed = true;
            }

            if (direction == 4 && column < Columns - 2 && MazeCells[row, column + 1].visited)
            {
                DestroyWallIfExists(MazeCells[row, column + 1].WestWall);
                DestroyWallIfExists(MazeCells[row, column].EastWall);
                wallDestroyed = true;
            }

        }
    }
}



