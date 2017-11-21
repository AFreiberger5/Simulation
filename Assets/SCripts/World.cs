using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class World : Singleton<World>
{
    protected  World() { }

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

        for (int x = 0; x < m_WorldArray.GetLength(0); ++x)
            for (int z = 0; z < m_WorldArray.GetLength(2); ++z)
            {
                int GrassNoise = FBM(x, z, m_GrassValues[0], m_GrassValues[1], m_GrassValues[2], 3);
                int StoneNoise = FBM(x, z, m_StoneValues[0], m_StoneValues[1], m_StoneValues[2], 5);
                for (int y = 0; y < m_WorldArray.GetLength(1); ++y)
                {
                    if (y < 5)
                        if (StoneNoise >= y)
                            m_WorldArray[x, y, z] = Instantiate(SBlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity);
                        else
                            m_WorldArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity);
                    else
                        if (GrassNoise >= y - 4)
                        m_WorldArray[x, y, z] = Instantiate(GBlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity);
                    else
                        m_WorldArray[x, y, z] = Instantiate(ABlock, new Vector3(x + (int)transform.position.x, y + (int)transform.position.y, z + (int)transform.position.z), Quaternion.identity);
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

    public static void MakeCoffe(RaycastHit _RHit, Transform _world)
    {
        int x = (int)_RHit.transform.position.x - (int)_world.transform.position.x;
        int y = (int)_RHit.transform.position.y - (int)_world.transform.position.y;
        int z = (int)_RHit.transform.position.z - (int)_world.transform.position.z;

        if (m_WorldArray[x, y + 1, z].tag == "ABlock")
        {
            Destroy(m_WorldArray[x, y, z]);
            m_WorldArray[x, y, z] = Instantiate(waterBlock, new Vector3(x , y , z), Quaternion.identity);
            m_WorldArray[x, y, z].CreateMesh(GetNeighbours(x, y, z));
        }

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
