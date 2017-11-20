using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : Block
{
    private bool top = false;

    public void OnEnable()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        m_BlockType = BlockType.GRASS;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
        m_Vertices.Clear();
        m_Indices.Clear();
        m_UVs.Clear();
        m_BlockType = BlockType.GRASS;
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Bottom))
        {
            RenderBottom(242);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Top))
        {
            RenderTop(178);
            top = true;

        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Front))
        {
            if (top)
                RenderFront(180);
            else
                RenderFront(242);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Back))
        {
            if (top)
                RenderBack(180);
            else
                RenderBack(242);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Left))
        {
            if (top)
                RenderLeft(180);
            else
                RenderLeft(242);
        }
        if (!Utils.IsBitSet(Neighbours, (int)NeighboursField.Right))
        {
            if (top)
                RenderRight(180);
            else
                RenderRight(242);
        }

        GenerateMesh();
    }

}
