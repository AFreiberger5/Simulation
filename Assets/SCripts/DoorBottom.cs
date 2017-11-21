using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBottom : Block
{

    public void OnEnable()
    {
        m_BlockType = BlockType.DOORBOTTOM;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.DOORBOTTOM;

        RenderFront(171);
        RenderBack(171);
        RenderLeft(244);
        RenderRight(244);

        GenerateMesh();
    }
}
