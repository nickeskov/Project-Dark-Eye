﻿using UnityEngine;
using System.Collections;

public class MazeLoader : MonoBehaviour
{
    public GameObject wall;
    public GameObject WallFire;
    public GameObject Floor;
    public GameObject FloorWithDebris;
    public GameObject parent;
    public Material mater, exit;
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

        int i = Random.Range(20, 31);
        int j = Random.Range(19, 31);
        bool IsExitGenerated = false;
        while (!IsExitGenerated)
        {
            if (_mazeCells[i, j].NorthWall != null)
            {
                NorthWall(i, j);
                IsExitGenerated = true;
            }
            else if (_mazeCells[i, j].EastWall != null)
            {
                EastWall(i, j);
                IsExitGenerated = true;
            }
            else if (_mazeCells[i, j].WestWall != null)
            {
                WestWall(i, j);
                IsExitGenerated = true;
            }
            else if (_mazeCells[i, j].SoughtWall != null)
            {
                SouthWall(i, j);
                IsExitGenerated = true;
            }
            if (!IsExitGenerated)
            {
                i = Random.Range(20, 31);
                j = Random.Range(19, 31);
            }
        }

    }
    void NorthWall(int i,int j)
    {
        MeshRenderer RenderedImage = _mazeCells[i, j].NorthWall.GetComponent<MeshRenderer>();
        RenderedImage.material = exit;
        BoxCollider BoxWall = _mazeCells[i, j].NorthWall.GetComponent<BoxCollider>();
        BoxWall.isTrigger = true;
        BoxWall.size = new Vector3(BoxWall.size.x, BoxWall.size.y, 8f);
        _mazeCells[i, j].NorthWall.gameObject.tag = "Exit";
        ParticleSystem EffectWall = _mazeCells[i, j].NorthWall.AddComponent<ParticleSystem>();
        EffectWall.Stop();
        ParticleSystem.MainModule Particles = EffectWall.main;
        Particles.startColor = Color.green;
        Particles.startLifetime = 3;
        ParticleSystemRenderer psr = EffectWall.GetComponent<ParticleSystemRenderer>();
        psr.material = mater;
        ParticleSystem.EmissionModule Emission = EffectWall.emission;
        Emission.rateOverTime = 50;
        ParticleSystem.ShapeModule Shape = EffectWall.shape;
        Shape.shapeType = ParticleSystemShapeType.Sphere;
        EffectWall.Play();
    }

    void EastWall(int i, int j)
    {
        MeshRenderer RenderedImage = _mazeCells[i, j].EastWall.GetComponent<MeshRenderer>();
        RenderedImage.material = exit;
        BoxCollider BoxWall = _mazeCells[i, j].EastWall.GetComponent<BoxCollider>();
        BoxWall.isTrigger = true;
        BoxWall.size = new Vector3(BoxWall.size.x, BoxWall.size.y, 8f);
        _mazeCells[i, j].EastWall.gameObject.tag = "Exit";
        ParticleSystem EffectWall = _mazeCells[i, j].EastWall.AddComponent<ParticleSystem>();
        EffectWall.Stop();
        ParticleSystem.MainModule Particles = EffectWall.main;
        Particles.startColor = Color.green;
        Particles.startLifetime = 3;
        ParticleSystemRenderer psr = EffectWall.GetComponent<ParticleSystemRenderer>();
        psr.material = mater;
        ParticleSystem.EmissionModule Emission = EffectWall.emission;
        Emission.rateOverTime = 50;
        ParticleSystem.ShapeModule Shape = EffectWall.shape;
        Shape.shapeType = ParticleSystemShapeType.Sphere;
        EffectWall.Play();
    }

    void WestWall(int i, int j)
    {
        MeshRenderer RenderedImage = _mazeCells[i, j].WestWall.GetComponent<MeshRenderer>();
        RenderedImage.material = exit;
        BoxCollider BoxWall = _mazeCells[i, j].WestWall.GetComponent<BoxCollider>();
        BoxWall.isTrigger = true;
        BoxWall.size = new Vector3(BoxWall.size.x, BoxWall.size.y, 8f);
        _mazeCells[i, j].WestWall.gameObject.tag = "Exit";
        ParticleSystem EffectWall = _mazeCells[i, j].WestWall.AddComponent<ParticleSystem>();
        EffectWall.Stop();
        ParticleSystem.MainModule Particles = EffectWall.main;
        Particles.startColor = Color.green;
        Particles.startLifetime = 3;
        ParticleSystemRenderer psr = EffectWall.GetComponent<ParticleSystemRenderer>();
        psr.material = mater;
        ParticleSystem.EmissionModule Emission = EffectWall.emission;
        Emission.rateOverTime = 50;
        ParticleSystem.ShapeModule Shape = EffectWall.shape;
        Shape.shapeType = ParticleSystemShapeType.Sphere;
        EffectWall.Play();
    }

    void SouthWall(int i, int j)
    {
        MeshRenderer RenderedImage = _mazeCells[i, j].SoughtWall.GetComponent<MeshRenderer>();
        RenderedImage.material = exit;
        BoxCollider BoxWall = _mazeCells[i, j].SoughtWall.GetComponent<BoxCollider>();
        BoxWall.isTrigger = true;
        BoxWall.size = new Vector3(BoxWall.size.x, BoxWall.size.y, 8f);
        _mazeCells[i, j].SoughtWall.gameObject.tag = "Exit";
        ParticleSystem EffectWall = _mazeCells[i, j].SoughtWall.AddComponent<ParticleSystem>();
        EffectWall.Stop();
        ParticleSystem.MainModule Particles = EffectWall.main;
        Particles.startColor = Color.green;
        Particles.startLifetime = 3;
        ParticleSystemRenderer psr = EffectWall.GetComponent<ParticleSystemRenderer>();
        psr.material = mater;
        ParticleSystem.EmissionModule Emission = EffectWall.emission;
        Emission.rateOverTime = 50;
        ParticleSystem.ShapeModule Shape = EffectWall.shape;
        Shape.shapeType = ParticleSystemShapeType.Sphere;
        EffectWall.Play();
    }
}

