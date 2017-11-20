using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MyCube3 : MonoBehaviour
{
    public int m_AtlasCols;
    public int m_AtlasRows;
    public List<int> m_TextureNum;

    private List<Vector3> m_Vertices;
    private List<int> m_Indices;
    private List<Vector2> m_UVs;

    // Use this for initialization
    void Start()
    {
        m_Vertices = new List<Vector3>();
        m_Indices = new List<int>();
        m_UVs = new List<Vector2>();

        List<Vector3> BaseVertices = new List<Vector3>();

        //v1
        BaseVertices.Add(new Vector3(-0.5f, -0.5f,  0.5f));
        //v2
        BaseVertices.Add(new Vector3(-0.5f,  0.5f,  0.5f));
        //v3
        BaseVertices.Add(new Vector3( 0.5f,  0.5f,  0.5f));
        //v4
        BaseVertices.Add(new Vector3( 0.5f, -0.5f,  0.5f));
        //v5
        BaseVertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        //v6
        BaseVertices.Add(new Vector3(-0.5f,  0.5f, -0.5f));
        //v7
        BaseVertices.Add(new Vector3( 0.5f,  0.5f, -0.5f));
        //v8
        BaseVertices.Add(new Vector3( 0.5f, -0.5f, -0.5f));

        // front
        m_Vertices.Add(BaseVertices[0]);
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[2]);
        m_Vertices.Add(BaseVertices[3]);
        // left
        m_Vertices.Add(BaseVertices[4]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[0]);
        // back
        m_Vertices.Add(BaseVertices[7]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[4]);
        // right
        m_Vertices.Add(BaseVertices[3]);
        m_Vertices.Add(BaseVertices[2]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[7]);
        // top
        m_Vertices.Add(BaseVertices[1]);
        m_Vertices.Add(BaseVertices[5]);
        m_Vertices.Add(BaseVertices[6]);
        m_Vertices.Add(BaseVertices[2]);
        // bottom
        m_Vertices.Add(BaseVertices[4]);
        m_Vertices.Add(BaseVertices[0]);
        m_Vertices.Add(BaseVertices[3]);
        m_Vertices.Add(BaseVertices[7]);

        int VertexAdd = 0;
        for (int i = 0; i < 36; i += 6)
        {
            //front
            m_Indices.Add(0 + VertexAdd);
            m_Indices.Add(3 + VertexAdd);
            m_Indices.Add(1 + VertexAdd);
            m_Indices.Add(1 + VertexAdd);
            m_Indices.Add(3 + VertexAdd);
            m_Indices.Add(2 + VertexAdd);

            VertexAdd += 4;
        }

        for (int i = 0; i < 6; ++i)
            GenerateUVSForIndex(i);

        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh M = GetComponent<MeshFilter>().mesh;

        M.vertices = m_Vertices.ToArray();
        M.triangles = m_Indices.ToArray();
        M.uv = m_UVs.ToArray();

        M.RecalculateNormals();
    }

    void GenerateUVSForIndex(int Index)
    {
        float XScale = 1.0f / m_AtlasCols;
        float YScale = 1.0f / m_AtlasRows;

        // NumTextures = m_AtlasRows * m_AtlasCols;

        float XOffset = XScale * (m_TextureNum[Index] % m_AtlasCols);
        float YOffset = YScale * (m_TextureNum[Index] / m_AtlasCols);

        m_UVs.Add(new Vector2(XOffset, YOffset));
        m_UVs.Add(new Vector2(XOffset, YScale + YOffset));
        m_UVs.Add(new Vector2(XScale + XOffset, YScale + YOffset));
        m_UVs.Add(new Vector2(XScale + XOffset, YOffset));
    }
}
