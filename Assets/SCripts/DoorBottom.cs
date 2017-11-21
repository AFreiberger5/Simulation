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

        RenderFront(244);
        RenderBack(244);
        RenderLeft(145);
        RenderRight(145);
        RenderBottom(244);

        GenerateMesh();
    }
}
