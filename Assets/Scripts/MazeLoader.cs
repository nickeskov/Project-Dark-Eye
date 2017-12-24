using UnityEngine;
using System.Collections;

public class MazeLoader : MonoBehaviour
{
    public GameObject wall;
    public GameObject WallFire;
    public GameObject Floor;
    public GameObject FloorWithDebris;
    public GameObject parent;
    public int Rows, Columns;
    public float Size = 2f;

    private MazeCell[,] _mazeCells;

    private void Start()
    {
        InitializeCells();
        MazeAlgoritm mazeAlgoritm = new HuntAndKillAlgoritm(_mazeCells);
        mazeAlgoritm.CreateMaze();
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

                _mazeCells[r, c].Floor = Instantiate(chance1 == 4 ? FloorWithDebris : Floor, new Vector3(r * Size, 0, c * Size - 3f), Quaternion.identity);
               
                _mazeCells[r, c].Floor.transform.parent = parent.transform;
                _mazeCells[r, c].Floor.name = "Floor " + r + " " + c;
                _mazeCells[r, c].Floor.transform.Rotate(Vector3.right, 90f); 


                if (r == 0)
                {
                    _mazeCells[r, c].NorthWall = Instantiate(wall, new Vector3(r * Size - Size / 2f, 0, c * Size), Quaternion.identity);
                    _mazeCells[r, c].NorthWall.transform.parent = parent.transform;
                    _mazeCells[r, c].NorthWall.name = "NorthWall" + r + "," + c;
                    _mazeCells[r, c].NorthWall.transform.Rotate(Vector3.up * 90f);
                }

                _mazeCells[r, c].SoughtWall = Instantiate(chance1 == 3 ? WallFire : wall, new Vector3(r * Size + Size / 2f, 0, c * Size), Quaternion.identity);

                _mazeCells[r, c].SoughtWall.transform.parent = parent.transform;
                _mazeCells[r, c].SoughtWall.name = "SouthWall " + r + "," + c;
                _mazeCells[r, c].SoughtWall.transform.Rotate(Vector3.up * 90f);

                if (c == 0)
                {
                    _mazeCells[r, c].WestWall = Instantiate(wall, new Vector3(r * Size, 0, c * Size - (Size / 2f)), Quaternion.identity);
                    _mazeCells[r, c].WestWall.transform.parent = parent.transform;
                    _mazeCells[r, c].WestWall.name = "Westwall" + r + "," + c;
                }

                _mazeCells[r, c].EastWall = Instantiate(wall, new Vector3(r * Size, 0, c * Size + Size / 2f), Quaternion.identity);

                _mazeCells[r, c].EastWall.transform.parent = parent.transform;
                _mazeCells[r, c].EastWall.name = "EastWall" + r + "," + c;
            }
        }

        MeshRenderer RenderedImage = _mazeCells[0, 1].NorthWall.GetComponent<MeshRenderer>();
        RenderedImage.enabled = false;
        BoxCollider BoxWall = _mazeCells[0, 1].NorthWall.GetComponent<BoxCollider>();
        BoxWall.isTrigger = true;
        BoxWall.size = new Vector3(BoxWall.size.x, BoxWall.size.y, 8f);
        ParticleSystem EffectWall = _mazeCells[0, 1].NorthWall.AddComponent<ParticleSystem>();
        _mazeCells[0, 1].NorthWall.gameObject.tag = "Exit";
        EffectWall.Play();
    }
}

