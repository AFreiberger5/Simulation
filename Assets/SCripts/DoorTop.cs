using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTop : Block
{
    private bool m_doorOpen = false;

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

    public void OpenClose()
    {
        if (m_doorOpen)
        {
            transform.Rotate(0, -90, 0);
            transform.position += new Vector3(0, 0, -0.4f);
            m_doorOpen = false;
        }
        else if (!m_doorOpen)
        {
            transform.Rotate(0, 90, 0);
            transform.position += new Vector3(0, 0, 0.4f);

            m_doorOpen = true;
        }
    }
}
