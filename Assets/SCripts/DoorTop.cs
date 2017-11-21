using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTop : Block
{

    public void OnEnable()
    {
        m_BlockType = BlockType.DOORTOP;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.DOORTOP;

        RenderFront(187);
        RenderBack(187);
        RenderLeft(244);
        RenderRight(244);

        GenerateMesh();
    }
}
