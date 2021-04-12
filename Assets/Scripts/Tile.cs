using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(�ݷ�) = ���
{
    // Public
    public int DIRECTION = 0;
    public const int RIGHT = 1;
    public const int LEFT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
    public bool MATCH = false;

    // Private
    private TileBehaivour m_tileBehaivour;
    private Vector2 m_clickedVec;
    private Vector2 m_movedVec;
    private Vector3 m_movedTo;
    private float m_duration;
    private int x;
    private int y;

    void Start()
    {
        m_tileBehaivour = new TileBehaivour(this.transform);
        m_movedTo = Vector3.zero;

        // �� �̵� �ð�
        m_duration = 0.5f;
    }

    void Update()
    {
        if (m_movedTo != Vector3.zero)
        {
            m_tileBehaivour.StartCoroutine(m_tileBehaivour.MoveTo(this, m_movedTo, m_duration));
        }
    }

    public void SetMovedTo(Vector3 to)
    {
        this.m_movedTo = to;
    }

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
        // Ÿ�� Ÿ�� ����
        SetTarget();
    }

    public void SetX(int x){this.x = x;}
    public int GetX(){return this.x;}
    public void SetY(int y){ this.y = y;}
    public int GetY(){return this.y;}

    /// <summary>
    /// Ÿ�� Ÿ�� ����
    /// </summary>
    private void SetTarget()
    {
        // X��ǥ �̵����� �� ū�� Y��ǥ �̵����� �� ū�� ��
        if (Mathf.Abs(m_movedVec.x - m_clickedVec.x) > Mathf.Abs(m_movedVec.y - m_clickedVec.y))
        {
            // X��ǥ +
            if (m_movedVec.x - m_clickedVec.x > 0) DIRECTION = RIGHT;
            // X��ǥ -
            if (m_movedVec.x - m_clickedVec.x < 0) DIRECTION = LEFT;
        }
        else
        {
            // Y��ǥ +
            if (m_movedVec.y - m_clickedVec.y > 0) DIRECTION = UP;
            // Y��ǥ -
            if (m_movedVec.y - m_clickedVec.y < 0) DIRECTION = DOWN;
        }
    }
}
