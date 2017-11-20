﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class House : MonoBehaviour
{

    public WoodBlock WooBlock;
    public GrassBlock GBlock;
    public StoneBlock SBlock;
    public AirBlock ABlock;

    [Range(5, 10)]
    public int WallHeight = 5;
    [Range(5, 10)]
    public int HiddenFloorLayer = 10;
    public GameObject[] HouseNodes;

    private bool m_Spawned = false;
    private int[][,] m_HouseBlocks;
    private Block[,,] m_HouseArray;
    private void OnTriggerEnter(Collider other)
    {
        if (!m_Spawned)
            CreateHouse();
        else
            DeleteHouse();
    }

    private void CreateHouse()
    {
        int Node = UnityEngine.Random.Range(0, HouseNodes.Length - 1);
        m_HouseBlocks = new int[WallHeight + 3][,];
        m_HouseBlocks[0] = Ground;
        for (int i = 1; i < WallHeight + 1; i++)
        {
            m_HouseBlocks[i] = Walls;
        }
        m_HouseBlocks[WallHeight + 1] = Roof1;
        m_HouseBlocks[WallHeight + 2] = Roof2;
        m_HouseArray = new Block[Ground.GetLength(0), m_HouseBlocks.Length, Ground.GetLength(1)];

        List<Block> Blocks = new List<Block>(GameObject.FindObjectsOfType<Block>());

        foreach (Block B in Blocks)
        {
            if (B == SBlock)
                continue;
            if (B == GBlock)
                continue;
            if (B == ABlock)
                continue;
            if (B == WooBlock)
                continue;

            Destroy(B.gameObject);
        }

        for (int y = 0; y < m_HouseBlocks.Length; y++)
        {
            for (int x = 0; x < m_HouseBlocks[y].GetLength(0); x++)
            {
                for (int z = 0; z < m_HouseBlocks[y].GetLength(1); z++)
                {
                    //todo: Try to use Noise instead of randomrange
                    int Block = UnityEngine.Random.Range(0,5);
                    if (m_HouseBlocks[y][x, z] == 4)
                        m_HouseArray[x, y, z] = Instantiate(WooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                    else if (m_HouseBlocks[y][x, z] == 5)
                    {
                        switch (Block)
                        {
                            case 0:
                                m_HouseArray[x, y, z] = Instantiate(WooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                            case 1:
                                m_HouseArray[x, y, z] = Instantiate(GBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                            case 2:
                                m_HouseArray[x, y, z] = Instantiate(SBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                            case 3:
                                m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                            case 4:
                                m_HouseArray[x, y, z] = Instantiate(WooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                            default:
                                m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
                                break;
                        }
                    }
                    else
                    {
                        m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x, y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);

                    }

                }

            }

        }

        for (int x = 0; x < m_HouseArray.GetLength(0); ++x)
            for (int y = 0; y < m_HouseArray.GetLength(1); ++y)
                for (int z = 0; z < m_HouseArray.GetLength(2); ++z)
                    m_HouseArray[x, y, z].CreateMesh(GetNeighbours(x, y, z));

        m_Spawned = true;
    }
    private void DeleteHouse()
    {

    }

    #region Floors
    // 1 = Grass, 2= Stone, 3 = Air, 4 = Wood, 5 = Random 
    private int[,] Ground = new int[,] { { 4, 4, 4, 4, 4 },
                                         { 4, 4, 4, 4, 4 },
                                         { 4, 4, 4, 4, 4 },
                                         { 4, 4, 4, 4, 4 },
                                         { 4, 4, 4, 4, 4 } };

    private int[,] Walls = new int[,]  { { 5, 5, 5, 5, 5 },
                                         { 5, 3, 3, 3, 5 },
                                         { 5, 3, 3, 3, 5 },
                                         { 5, 3, 3, 3, 5 },
                                         { 5, 5, 5, 5, 5 } };

    private int[,] Roof1 = new int[,]  { { 3, 3, 3, 3, 3 },
                                         { 3, 4, 4, 4, 3 },
                                         { 3, 4, 3, 4, 3 },
                                         { 3, 4, 4, 4, 3 },
                                         { 3, 3, 3, 3, 3 } };

    private int[,] Roof2 = new int[,]  { { 3, 3, 3, 3, 3 },
                                         { 3, 3, 3, 3, 3 },
                                         { 3, 3, 5, 3, 3 },
                                         { 3, 3, 3, 3, 3 },
                                         { 3, 3, 3, 3, 3 } };


    #endregion

    private NeighboursField GetNeighbours(int x, int y, int z)
    {
        NeighboursField Field = 0;
        try
        {
            if (x > 0 && x < m_HouseArray.GetLength(0))
                if (m_HouseArray[x - 1, y, z].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Left);
            if (x >= 0 && x < m_HouseArray.GetLength(0) - 1)
                if (m_HouseArray[x + 1, y, z].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Right);
            if (y > 0 && y < m_HouseArray.GetLength(1))
                if (m_HouseArray[x, y - 1, z].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Bottom);
            if (y >= 0 && y < m_HouseArray.GetLength(1) - 1)
                if (m_HouseArray[x, y + 1, z].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Top);
            if (z > 0 && z < m_HouseArray.GetLength(2))
                if (m_HouseArray[x, y, z - 1].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Back);
            if (z >= 0 && z < m_HouseArray.GetLength(2) - 1)
                if (m_HouseArray[x, y, z + 1].GetBlockType() != BlockType.AIR)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Front);
        }
        catch
        {
            
            Debug.Log(x);
            Debug.Log(y);
            Debug.Log(z);
            Debug.Log(m_HouseArray.GetLength(0));
            Debug.Log(m_HouseArray.GetLength(1));
            Debug.Log(m_HouseArray.GetLength(2));
           
        }

        return Field;
    }
}