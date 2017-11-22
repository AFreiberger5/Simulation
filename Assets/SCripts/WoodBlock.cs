using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : Block
{
    
  
    public void OnEnable()
    {
        m_BlockType = BlockType.WOOD;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.WOOD;
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Front))
        {
            RenderFront(244);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Back))
        {
            RenderBack(244);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Left))
        {
            RenderLeft(244);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Right))
        {
            RenderRight(244);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Top))
        {
            RenderTop(244);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Bottom))
        {
            RenderBottom(244);
        }

        GenerateMesh();
    }

}
