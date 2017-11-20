using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : Block
{
    
  // public int m_top;
  // public int m_bottom;
  // public int m_front;
  // public int m_back;
  // public int m_left;
  // public int m_right;
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
