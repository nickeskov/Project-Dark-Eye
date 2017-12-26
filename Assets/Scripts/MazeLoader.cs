using UnityEngine;
using System.Collections;
using Microsoft.Win32.SafeHandles;

public class MazeLoader : MonoBehaviour
{
    public GameObject wall;
    public GameObject WallFire;
    public GameObject Floor;
    public GameObject FloorWithDebris;
    public GameObject parent;
    public Material mater, exit;
    public int Rows, Columns;

    public float MinRowsLabirinthPart = 0.25f;
    public float MinColumnsLabirinthPart = 0.25f;

    public float Size = 2f;

    private MazeCell[,] _mazeCells;

    private void Start()
    {
        InitializeCells();
        MazeAlgoritm mazeAlgoritm = new HuntAndKillAlgoritm(_mazeCells);
        mazeAlgoritm.CreateMaze();
        GenerateExit();
    }

    public void InitializeCells()
    {
        _mazeCells = new MazeCell[Rows, Columns];
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                _mazeCells[r, c] = new MazeCell();

                int chance1 = Random.Range(0, 19);
                //int screamerchance = Random.Range(0, 100);

                _mazeCells[r, c].Floor = Instantiate(chance1 == 4 ? FloorWithDebris : Floor,
                    new Vector3(r * Size, 0, c * Size - 3f), Quaternion.identity);

                _mazeCells[r, c].Floor.transform.parent = parent.transform;
                _mazeCells[r, c].Floor.name = "Floor " + r + " " + c;
                _mazeCells[r, c].Floor.transform.Rotate(Vector3.right, 90f);
                //if (screamerchance <= 10) _mazeCells[r, c].Floor.tag = "Screamer";
                _mazeCells[r, c].Floor.tag = "Screamer";

                if (r == 0)
                {
                    _mazeCells[r, c].NorthWall = Instantiate(wall, new Vector3(r * Size - Size / 2f, 0, c * Size),
                        Quaternion.identity);
                    _mazeCells[r, c].NorthWall.transform.parent = parent.transform;
                    _mazeCells[r, c].NorthWall.name = "NorthWall" + r + "," + c;
                    _mazeCells[r, c].NorthWall.transform.Rotate(Vector3.up * 90f);
                }

                _mazeCells[r, c].SoughtWall = Instantiate(chance1 == 3 ? WallFire : wall,
                    new Vector3(r * Size + Size / 2f, 0, c * Size), Quaternion.identity);

                _mazeCells[r, c].SoughtWall.transform.parent = parent.transform;
                _mazeCells[r, c].SoughtWall.name = "SouthWall " + r + "," + c;
                _mazeCells[r, c].SoughtWall.transform.Rotate(Vector3.up * 90f);

                if (c == 0)
                {
                    _mazeCells[r, c].WestWall = Instantiate(wall, new Vector3(r * Size, 0, c * Size - (Size / 2f)),
                        Quaternion.identity);
                    _mazeCells[r, c].WestWall.transform.parent = parent.transform;
                    _mazeCells[r, c].WestWall.name = "Westwall " + r + "," + c;
                }

                _mazeCells[r, c].EastWall = Instantiate(wall, new Vector3(r * Size, 0, c * Size + Size / 2f),
                    Quaternion.identity);

                _mazeCells[r, c].EastWall.transform.parent = parent.transform;
                _mazeCells[r, c].EastWall.name = "EastWall " + r + "," + c;
            }
        }
    }

    private void GenerateExit()
    {
        bool isExitGenerated = false;

        if (MinRowsLabirinthPart > 1f)
        {
            MinRowsLabirinthPart = 1;
        }

        if (MinColumnsLabirinthPart > 1f)
        {
            MinColumnsLabirinthPart = 1;
        }

        int minDistanceRow = (int)(Rows - Rows * MinRowsLabirinthPart);
        int minDistanceColumns = (int)(Columns - Columns * MinColumnsLabirinthPart);

        while (!isExitGenerated)
        {
            int i = Random.Range(minDistanceRow, Rows - 1);
            int j = Random.Range(minDistanceColumns, Columns - 1);

            MazeCell exitMazeCell = _mazeCells[i, j];

            if (exitMazeCell.EastWall)
            {
                MakeExit(exitMazeCell.EastWall);
                isExitGenerated = true;
            }
            else if (exitMazeCell.SoughtWall)
            {
                MakeExit(exitMazeCell.SoughtWall);
                isExitGenerated = true;
            }
            else if (exitMazeCell.NorthWall)
            {
                MakeExit(exitMazeCell.NorthWall);
                isExitGenerated = true;
            }
            else if (exitMazeCell.WestWall)
            {
                // Debug.Log(exitMazeCell.WestWall.GetType().ToString());

                MakeExit(exitMazeCell.WestWall);
                isExitGenerated = true;
                //Debug.Log("IsExitGenerated - WestWall " + i + ", " + j);
            }
        }
    }

    private void MakeExit(GameObject orientatedWall)
    {
        MeshRenderer renderedImage = orientatedWall.GetComponent<MeshRenderer>();
        renderedImage.material = exit;

        BoxCollider boxWall = orientatedWall.GetComponent<BoxCollider>();
        boxWall.isTrigger = true;

        boxWall.size = new Vector3(boxWall.size.x, boxWall.size.y, boxWall.size.z);
        orientatedWall.gameObject.tag = "Exit";

        ParticleSystem effectWall = orientatedWall.AddComponent<ParticleSystem>();
        effectWall.Stop();

        ParticleSystem.MainModule particles = effectWall.main;

        particles.startColor = Color.cyan;

        particles.startLifetime = 3;
        ParticleSystemRenderer psr = effectWall.GetComponent<ParticleSystemRenderer>();

        psr.material = mater;

        ParticleSystem.EmissionModule emission = effectWall.emission;
        emission.rateOverTime = 50;

        ParticleSystem.ShapeModule shape = effectWall.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        effectWall.Play();
    }
}
