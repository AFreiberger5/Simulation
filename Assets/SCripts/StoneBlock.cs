using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : Block
{
    public void OnEnable()
    {
        m_BlockType = BlockType.STONE;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.STONE;
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Front))
        {
            RenderFront(224);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Back))
        {
            RenderBack(224);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Left))
        {
            RenderLeft(224);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Right))
        {
            RenderRight(224);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Top))
        {
            RenderTop(224);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Bottom))
        {
            RenderBottom(224);
        }

        GenerateMesh();
    }

}
