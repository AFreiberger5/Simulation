﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class House : MonoBehaviour
{

    public NavMeshSurface[] n = new NavMeshSurface[2];
    public WoodBlock wooBlock;
    public GrassBlock GBlock;
    public StoneBlock SBlock;
    public AirBlock ABlock;
    public WaterBlock waterBlock;
    public DoorBottom bottomBlock;
    public DoorTop topBlock;


    [Range(5, 10)]
    public int WallHeight = 5;
    [Range(5, 10)]
    public int HiddenFloorLayer = 10;
    public GameObject[] House1Nodes;
    public GameObject[] House2Nodes;
    public GameObject NPC;
    [HideInInspector] public Transform[] WayPoints;
    [HideInInspector] public StateController m_StateController;

    private bool m_Spawned = false;
    //todo: check if house1/2 blocks are needed for editor view or can be removed 
    private int[][,] m_HouseBlocks;
    private int[][,] m_House1Blocks;
    private int[][,] m_House2Blocks;
    private Block[,,] m_HouseArray;
    private Block[,,] m_House1Array;
    private Block[,,] m_House2Array;
    private int Node;
    private static bool doorRay = false;
    private bool NPCSpawned = false;
    private void Awake()
    {
        m_StateController = NPC.GetComponent<StateController>();
    }
    private void OnTriggerExit(Collider other)
    {
        //todo: add swirl effects that start/stop when enter-/leaving triggerzone
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!m_Spawned && other.tag == "Player")
        {
            Node = Random.Range(0, House1Nodes.Length);
            CreateHouse(Node, House1Nodes, 1);

            //take patrolpoints from first house
            List<Transform> waypoints = new List<Transform>(House1Nodes[Node].GetComponentsInChildren<Transform>());

            //spawn 2nd house
            Node = Random.Range(0, House2Nodes.Length);
            CreateHouse(Node, House2Nodes, 2);
            //navmesh builden

            // n[0].BuildNavMesh();
            // n[1].BuildNavMesh();
            if (!NPCSpawned)
            {
                Instantiate(NPC, m_House1Array[2, 1, 3].transform.position, Quaternion.identity);
                NPCSpawned = true;
            }

            m_StateController.SetupAI(true, waypoints);
        }
        else if (m_Spawned && other.tag == "Player")
            DeleteHouse();
    }

    private void CreateHouse(int _node, GameObject[] _housenodes, int _number)
    {


        GameObject[] HouseNodes = _housenodes;
        m_HouseBlocks = new int[WallHeight + 3][,];
        m_HouseBlocks[0] = Ground;
        for (int i = 1; i < WallHeight + 1; i++)
        {
            m_HouseBlocks[i] = Walls;
        }
        m_HouseBlocks[WallHeight + 1] = Roof1;
        m_HouseBlocks[WallHeight + 2] = Roof2;
        if (WallHeight >= HiddenFloorLayer)
            m_HouseBlocks[HiddenFloorLayer] = Ground;

        m_HouseArray = new Block[Ground.GetLength(0), m_HouseBlocks.Length, Ground.GetLength(1)];



        GameObject blockSave = new GameObject("house");
        for (int y = 0; y < m_HouseBlocks.Length; y++)
        {
            for (int x = 0; x < m_HouseBlocks[y].GetLength(0); x++)
            {
                for (int z = 0; z < m_HouseBlocks[y].GetLength(1); z++)
                {
                    //todo: Try to use Noise instead of randomrange
                    int Block = UnityEngine.Random.Range(0, 5);
                    if (m_HouseBlocks[y][x, z] == 4)
                    {
                        m_HouseArray[x, y, z] = Instantiate(wooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                            y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                    }
                    else if (m_HouseBlocks[y][x, z] == 5)
                    {
                        switch (Block)
                        {

                            case 0:
                                m_HouseArray[x, y, z] = Instantiate(wooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                            case 1:
                                m_HouseArray[x, y, z] = Instantiate(GBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                            case 2:
                                m_HouseArray[x, y, z] = Instantiate(SBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                            case 3:
                                m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                            case 4:
                                m_HouseArray[x, y, z] = Instantiate(wooBlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                            default:
                                m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                                    y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);
                                break;
                        }
                    }
                    else
                    {
                        m_HouseArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)HouseNodes[Node].transform.position.x,
                            y + (int)HouseNodes[Node].transform.position.y, z + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, blockSave.transform);

                    }

                }

            }

        }
        //check which house gets which door, first gets manual door
        if (!doorRay)
        {
            //todo: find out why the first house isn't getting spawned
            Destroy(m_HouseArray[4, 1, 2]);
            Destroy(m_HouseArray[4, 2, 2]);
            m_HouseArray[4, 2, 2] = Instantiate(topBlock, new Vector3(4 + (int)HouseNodes[Node].transform.position.x, 2 + (int)HouseNodes[Node].transform.position.y, 2 + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
            m_HouseArray[4, 1, 2] = Instantiate(bottomBlock, new Vector3(4 + (int)HouseNodes[Node].transform.position.x, 1 + (int)HouseNodes[Node].transform.position.y, 2 + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, m_HouseArray[4, 2, 2].transform);
            m_HouseArray[4, 1, 2].tag = "RayDoorBottom";
            m_HouseArray[4, 2, 2].tag = "RayDoorTop";

            doorRay = true;

        }
        else
        {
            Destroy(m_HouseArray[4, 1, 2]);
            Destroy(m_HouseArray[4, 2, 2]);
            m_HouseArray[4, 2, 2] = Instantiate(topBlock, new Vector3(4 + (int)HouseNodes[Node].transform.position.x, 2 + (int)HouseNodes[Node].transform.position.y, 2 + (int)HouseNodes[Node].transform.position.z), Quaternion.identity);
            m_HouseArray[4, 1, 2] = Instantiate(bottomBlock, new Vector3(4 + (int)HouseNodes[Node].transform.position.x, 1 + (int)HouseNodes[Node].transform.position.y, 2 + (int)HouseNodes[Node].transform.position.z), Quaternion.identity, m_HouseArray[4, 2, 2].transform);
            m_HouseArray[4, 1, 2].tag = "TriggerDoorBottom";
            m_HouseArray[4, 2, 2].tag = "TriggerDoorTop";
            m_HouseArray[4, 1, 2].GetOrAddComponent<BoxCollider>();
            m_HouseArray[4, 1, 2].GetOrAddComponent<BoxCollider>().isTrigger = true;
            m_HouseArray[4, 1, 2].GetOrAddComponent<BoxCollider>().size = new Vector3(20, 2, 4);


        }
        if (_number == 1)
        {
            m_House1Array = m_HouseArray;
            for (int x = 0; x < m_House1Array.GetLength(0); ++x)
                for (int y = 0; y < m_House1Array.GetLength(1); ++y)
                    for (int z = 0; z < m_House1Array.GetLength(2); ++z)
                        m_House1Array[x, y, z].CreateMesh(GetNeighbours(x, y, z));

            m_House1Blocks = m_HouseBlocks;

            blockSave.AddComponent<NavMeshSurface>();
            n[0] = blockSave.GetComponent<NavMeshSurface>();
        }
        else if (_number == 2)
        {
            m_House2Array = m_HouseArray;
            for (int x = 0; x < m_House2Array.GetLength(0); ++x)
                for (int y = 0; y < m_House2Array.GetLength(1); ++y)
                    for (int z = 0; z < m_House2Array.GetLength(2); ++z)
                        m_House2Array[x, y, z].CreateMesh(GetNeighbours(x, y, z));

            m_House2Blocks = m_HouseBlocks;

            blockSave.AddComponent<NavMeshSurface>();
            n[1] = blockSave.GetComponent<NavMeshSurface>();

        }
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
                if (m_HouseArray[x - 1, y, z].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x - 1, y, z].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x - 1, y, z].GetBlockType() != BlockType.DOORTOP)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Left);
            if (x >= 0 && x < m_HouseArray.GetLength(0) - 1)
                if (m_HouseArray[x + 1, y, z].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x + 1, y, z].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x + 1, y, z].GetBlockType() != BlockType.DOORTOP)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Right);
            if (y > 0 && y < m_HouseArray.GetLength(1))
                if (m_HouseArray[x, y - 1, z].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x, y - 1, z].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x, y - 1, z].GetBlockType() != BlockType.DOORTOP)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Bottom);
            if (y >= 0 && y < m_HouseArray.GetLength(1) - 1)
                if (m_HouseArray[x, y + 1, z].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x, y + 1, z].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x, y + 1, z].GetBlockType() != BlockType.DOORTOP)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Top);
            if (z > 0 && z < m_HouseArray.GetLength(2))
                if (m_HouseArray[x, y, z - 1].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x, y, z - 1].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x, y, z - 1].GetBlockType() != BlockType.DOORTOP)
                    Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Back);
            if (z >= 0 && z < m_HouseArray.GetLength(2) - 1)
                if (m_HouseArray[x, y, z + 1].GetBlockType() != BlockType.AIR
                    && m_HouseArray[x, y, z + 1].GetBlockType() != BlockType.DOORBOTTOM
                    && m_HouseArray[x, y, z + 1].GetBlockType() != BlockType.DOORTOP)
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
