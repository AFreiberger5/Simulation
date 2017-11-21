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

        RenderFront(244);
        RenderBack(244);
        RenderLeft(161);
        RenderRight(161);
        RenderTop(244);

        GenerateMesh();
    }
}
