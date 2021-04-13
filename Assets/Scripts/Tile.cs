using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(콜론) = 상속
{
    // Public
    public int DIRECTION = 0;
    public const int RIGHT = 1;
    public const int LEFT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
    public bool MATCH = false;
    public TileBehaivour TileBehaivour = new TileBehaivour();

    // Private
    private Vector2 m_clickedVec;
    private Vector2 m_movedVec;
    private int x;
    private int y;

    private void OnMouseDown()
    {
        m_clickedVec = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        m_movedVec = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // 타겟 타일 세팅
        SetTarget();
    }

    public void SetX(int x){this.x = x;}
    public int GetX(){return this.x;}
    public void SetY(int y){ this.y = y;}
    public int GetY(){return this.y;}

    /// <summary>
    /// 타겟 타일 세팅
    /// </summary>
    private void SetTarget()
    {
        // X좌표 이동값이 더 큰지 Y좌표 이동값이 더 큰지 비교
        if (Mathf.Abs(m_movedVec.x - m_clickedVec.x) > Mathf.Abs(m_movedVec.y - m_clickedVec.y))
        {
            // X좌표 +
            if (m_movedVec.x - m_clickedVec.x > 0) DIRECTION = RIGHT;
            // X좌표 -
            if (m_movedVec.x - m_clickedVec.x < 0) DIRECTION = LEFT;
        }
        else
        {
            // Y좌표 +
            if (m_movedVec.y - m_clickedVec.y > 0) DIRECTION = UP;
            // Y좌표 -
            if (m_movedVec.y - m_clickedVec.y < 0) DIRECTION = DOWN;
        }
    }
}
