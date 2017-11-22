using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum BlockType
{
    GRASS,
    STONE,
    AIR,
    WOOD,
    RANDOM,
    DOORBOTTOM,
    DOORTOP,
    WATER
}

[Flags]
public enum NeighboursField : int
{
    Left = 1,
    Right = 2,
    Front = 4,
    Back = 8,
    Top = 16,
    Bottom = 32
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
abstract public class Block : MonoBehaviour
{
    protected BlockType m_BlockType;

    protected List<Vector3> m_Vertices;
    protected List<int> m_Indices;
    protected List<Vector2> m_UVs;

    protected List<Vector3> BaseVertices;

    abstract public void CreateMesh(NeighboursField Neighbours);

    // Use this for initialization
    protected void Awake()
    {
        m_Vertices = new List<Vector3>();
        m_Indices = new List<int>();
        m_UVs = new List<Vector2>();
        BaseVertices = new List<Vector3>();

        //v1
        BaseVertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        //v2
        BaseVertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        //v3
        BaseVertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        //v4
        BaseVertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        //v5
        BaseVertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        //v6
        BaseVertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        //v7
        BaseVertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        //v8
        BaseVertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
    }

    protected void GenerateMesh()
    {
        Mesh M = GetComponent<MeshFilter>().mesh;

        M.vertices = m_Vertices.ToArray();
        M.triangles = m_Indices.ToArray();
        M.uv = m_UVs.ToArray();

        M.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = M;

    }

    protected void RenderFront(int TextureIndex)
    {
        // front
        m_Vertices.Add(BaseVertices[0]);
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[2]);
        m_Vertices.Add(BaseVertices[3]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void RenderLeft(int TextureIndex)
    {
        // left
        m_Vertices.Add(BaseVertices[4]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[0]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void RenderBack(int TextureIndex)
    {
        // back
        m_Vertices.Add(BaseVertices[7]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[4]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void RenderRight(int TextureIndex)
    {
        // right
        m_Vertices.Add(BaseVertices[3]);
        m_Vertices.Add(BaseVertices[2]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[7]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void RenderTop(int TextureIndex)
    {
        // top
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[2]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void RenderBottom(int TextureIndex)
    {
        // bottom
        m_Vertices.Add(BaseVertices[4]);
        m_Vertices.Add(BaseVertices[0]);
        m_Vertices.Add(BaseVertices[3]);
        m_Vertices.Add(BaseVertices[7]);

        CalculateIndices();
        GenerateUVSForIndex(TextureIndex);
    }

    protected void CalculateIndices()
    {
        int VertexAdd = m_Vertices.Count-4;

        m_Indices.Add(0 + VertexAdd);
        m_Indices.Add(3 + VertexAdd);
        m_Indices.Add(1 + VertexAdd);
        m_Indices.Add(1 + VertexAdd);
        m_Indices.Add(3 + VertexAdd);
        m_Indices.Add(2 + VertexAdd);
    }

    protected void GenerateUVSForIndex(int Index)
    {
        float XScale = 1.0f / World.Instance.m_AtlasCols;
        float YScale = 1.0f / World.Instance.m_AtlasRows;

        //NumTextures = World.Instance.m_AtlasRows * World.Instance.m_AtlasCols;

        float XOffset = XScale * (Index % World.Instance.m_AtlasCols);
        float YOffset = YScale * (Index / World.Instance.m_AtlasCols);

        m_UVs.Add(new Vector2(XOffset, YOffset));
        m_UVs.Add(new Vector2(XOffset, YScale + YOffset));
        m_UVs.Add(new Vector2(XScale + XOffset, YScale + YOffset));
        m_UVs.Add(new Vector2(XScale + XOffset, YOffset));
    }

    public BlockType GetBlockType()
    {
        return m_BlockType;
    }
}
