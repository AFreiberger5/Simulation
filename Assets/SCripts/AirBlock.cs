using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlock : Block
{
    public void OnEnable()
    {
        m_BlockType = BlockType.AIR;
    }

    public override void CreateMesh(NeighboursField Neighbours)
    {
    }
}
