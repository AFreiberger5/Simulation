using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : Singleton<World>
{
    protected World() { }

    public int m_AtlasCols;
    public int m_AtlasRows;

    public WoodBlock wooBlock;
    public GrassBlock GBlock;
    public StoneBlock SBlock;
    public AirBlock ABlock;
    public WaterBlock EditorWater;
    public static WaterBlock waterBlock;
    public DoorBottom bottomBlock;
    public DoorTop topBlock;

    private static Block[,,] m_WorldArray;
    private float[] m_GrassValues = { 13.2f, 0.75f, 1.2f };
    private float[] m_StoneValues = { 2.7f, 0.75f, 1.1f };


    void Start()
    {
        waterBlock = EditorWater;
        m_WorldArray = new Block[40, 8, 40];
        GenerateTerrain();
    }


    void GenerateTerrain()
    {
        List<Block> Blocks = new List<Block>(GameObject.FindObjectsOfType<Block>());

        foreach (Block B in Blocks)
        {
            if (B == SBlock)
                continue;
            if (B == GBlock)
                continue;
            if (B == ABlock)
                continue;
            if (B == wooBlock)
                continue;
            if (B == waterBlock)
                continue;
            if (B == bottomBlock)
                continue;
            if (B == topBlock)
                continue;
            Destroy(B.gameObject);
        }
        GameObject blockSave = new GameObject("world");
        for (int x = 0; x < m_WorldArray.GetLength(0); ++x)
            for (int z = 0; z < m_WorldArray.GetLength(2); ++z)
            {
                int GrassNoise = FBM(x, z, m_GrassValues[0], m_GrassValues[1], m_GrassValues[2], 3);
                int StoneNoise = FBM(x, z, m_StoneValues[0], m_StoneValues[1], m_StoneValues[2], 5);
                for (int y = 0; y < m_WorldArray.GetLength(1); ++y)
                {
                    if (y < 5)
                        if (StoneNoise >= y)
                            m_WorldArray[x, y, z] = Instantiate(SBlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity, blockSave.transform);
                        else
                            m_WorldArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity, blockSave.transform);
                    else
                        if (GrassNoise >= y - 4)
                        m_WorldArray[x, y, z] = Instantiate(GBlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity, blockSave.transform);
                    else
                        m_WorldArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity, blockSave.transform);
                }
            }
        for (int x = 0; x < m_WorldArray.GetLength(0); ++x)
            for (int y = 0; y < m_WorldArray.GetLength(1); ++y)
                for (int z = 0; z < m_WorldArray.GetLength(2); ++z)
                    m_WorldArray[x, y, z].CreateMesh(GetNeighbours(x, y, z));

    }

    private static NeighboursField GetNeighbours(int x, int y, int z)
    {
        NeighboursField Field = 0;

        if (x > 0 && x < m_WorldArray.GetLength(0))
            if (m_WorldArray[x - 1, y, z].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Left);
        if (x >= 0 && x < m_WorldArray.GetLength(0) - 1)
            if (m_WorldArray[x + 1, y, z].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Right);
        if (y > 0 && y < m_WorldArray.GetLength(1))
            if (m_WorldArray[x, y - 1, z].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Bottom);
        if (y >= 0 && y < m_WorldArray.GetLength(1) - 1)
            if (m_WorldArray[x, y + 1, z].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Top);
        if (z > 0 && z < m_WorldArray.GetLength(2))
            if (m_WorldArray[x, y, z - 1].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Back);
        if (z >= 0 && z < m_WorldArray.GetLength(2) - 1)
            if (m_WorldArray[x, y, z + 1].GetBlockType() != BlockType.AIR)
                Field = (NeighboursField)Utils.SetBit(Field, (int)NeighboursField.Front);

        return Field;
    }

    private int FBM(int x, int z, float Scale, float Persistance, float Lacunarity, int Factor)
    {
        float Amplitude = 1.0f;
        float Frequency = 1.0f;
        float NoiseHeight = 0.0f;

        for (int i = 0; i < 4; ++i)
        {
            float SampleX = x / Scale * Frequency;
            float SampleZ = z / Scale * Frequency;

            float CurrentNoiseValue = Mathf.PerlinNoise(SampleX, SampleZ);

            NoiseHeight += CurrentNoiseValue * Amplitude;

            Amplitude *= Persistance;
            Frequency *= Lacunarity;
        }
        return Mathf.FloorToInt(map(NoiseHeight, 0.7f, 1.5f, 0.0f, Factor));
    }

    public static void BrewCoffee(RaycastHit _RHit, Transform _world)
    {
        int x = (int)_RHit.point.x - (int)_world.transform.position.x;
        int y = (int)_RHit.point.y - (int)_world.transform.position.y;
        int z = (int)_RHit.point.z - (int)_world.transform.position.z;
        if (x > m_WorldArray.GetLength(0) - 1 || y > m_WorldArray.GetLength(1) - 1 || z > m_WorldArray.GetLength(2) - 1
            || x < 0 || y < 0 || z < 0)
        {

        }
        else
        {


            if (m_WorldArray[x, y + 1, z].tag == "ABlock" && y + 1 < m_WorldArray.GetLength(1))
            {
                //not deleting this code to remember/think about changes easier
                //Destroy(m_WorldArray[x, y, z]);
                //m_WorldArray[x, y + 1, z] = Instantiate(waterBlock, new Vector3(_RHit.transform.position.x,
                //     _RHit.transform.position.y + 1, _RHit.transform.position.z), Quaternion.identity);
                //m_WorldArray[x, y + 1, z].CreateMesh(GetNeighbours(x, y + 1, z));
                PourCoffe(_RHit, x, y, z, 0, 1, 0);

            }
            if (m_WorldArray[x, y + 1, z].tag == "wetBlock" && x + 1 < m_WorldArray.GetLength(0) && m_WorldArray[x + 1, y, z].tag == "ABlock")
            {
                RaycastHit RHit = _RHit;
                RHit.point += new Vector3(1, 0, 0);
                PourCoffe(RHit, x, y, z, 0, 1, 0);
                BrewCoffee(RHit, _world);

            }
            // if (m_WorldArray[x, y + 1, z].tag == "wetBlock" && x - 1 >= 0 && m_WorldArray[x - 1, y, z].tag == "ABlock")
            // {
            //     RaycastHit RHit = _RHit;
            //     RHit.point += new Vector3(-1, 0, 0);
            //     PourCoffe(RHit, x, y, z, 0, 1, 0);
            //     BrewCoffee(RHit, _world);
            //
            // }
            // if (m_WorldArray[x, y + 1, z].tag == "wetBlock" && z + 1 < m_WorldArray.GetLength(2) && m_WorldArray[x, y, z + 1].tag == "ABlock")
            // {
            //     RaycastHit RHit = _RHit;
            //     RHit.point += new Vector3(0, 0, 1);
            //     PourCoffe(RHit, x, y, z, 0, 1, 0);
            //     BrewCoffee(RHit, _world);
            //
            // }
            // if (m_WorldArray[x, y + 1, z].tag == "wetBlock" && z-1 >= 0 && m_WorldArray[x , y, z - 1].tag == "ABlock")
            // {
            //     RaycastHit RHit = _RHit;
            //     RHit.point += new Vector3(1, 0, -1);
            //     PourCoffe(RHit, x, y, z, 0, 1, 0);
            //     BrewCoffee(RHit, _world);
            //
            // }

        }
    }

    public static void PourCoffe(RaycastHit _RHit, int _x, int _y, int _z, int _px, int _py, int _pz)
    {
        int x = _x + _px;
        int y = _y + _py;
        int z = _z + _pz;

        //Destroy(m_WorldArray[x, y, z]);

        m_WorldArray[x, y, z] = Instantiate(waterBlock, new Vector3(_RHit.point.x + _px,
             _RHit.point.y + _py, _RHit.point.z + _pz), Quaternion.identity);

        m_WorldArray[x, y, z].CreateMesh(GetNeighbours(x, y, z));
    }

    private float map(float value,
                              float istart,
                              float istop,
                              float ostart,
                              float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }



}
