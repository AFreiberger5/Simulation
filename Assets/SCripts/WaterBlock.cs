using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlock : Block
{
   

    public void OnEnable()
    {        
        m_BlockType = BlockType.WATER;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {

        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.WATER;
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Front))
        {
            RenderFront(254);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Back))
        {
            RenderBack(254);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Left))
        {
            RenderLeft(254);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Right))
        {
            RenderRight(254);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Top))
        {
            RenderTop(254);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Bottom))
        {
            RenderBottom(254);
        }

        GenerateMesh();
    }

}
