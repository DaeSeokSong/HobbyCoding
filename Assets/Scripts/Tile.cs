using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Public
    // const
    public const int RIGHT = 1;
    public const int LEFT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
    // member
    public int DIRECTION = 0;
    public bool MATCH = false;

    // Private
    // �̵� �ð�
    private const float m_Duration = 0.5f;
    // �̵� ��ǥ
    private int x;
    private int y;
    // �̵� ��ǥ Ÿ��
    private Tile m_TargetTile;
    // Ÿ�� �ൿ ��ü
    private TileBehaivour m_TileBehaivour;
    // Ŭ�� �� �¿���� ��
    private Vector2 m_ClickedVec;
    private Vector2 m_MovedVec;
    // MatchUp ����Ʈ
    private List<Tile> m_MatchList = new List<Tile>();

    void Start()
    {
        // Ÿ�� �ൿ ��ü
        m_TileBehaivour = new TileBehaivour(this.transform);
        m_TargetTile = new Tile();
    }

    private void Move(Vector3 to)
    {
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.MoveTo(this, to, m_Duration));
    }

    private void OnMouseDown()
    {
        m_ClickedVec = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        m_MovedVec = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // Ÿ�� Ÿ�� ����
        SetDirection();
        // Ÿ�ٰ� ��ġ ����
        SwapPosition();
    }

    public void SetX(int x) { this.x = x; }
    public int GetX() { return this.x; }
    public void SetY(int y) { this.y = y; }
    public int GetY() { return this.y; }

    /// <summary>
    /// Ÿ�� Ÿ�� ����
    /// </summary>
    private void SetDirection()
    {
        // X��ǥ �̵����� �� ū�� Y��ǥ �̵����� �� ū�� ��
        if (Mathf.Abs(m_MovedVec.x - m_ClickedVec.x) > Mathf.Abs(m_MovedVec.y - m_ClickedVec.y))
        {
            // X��ǥ +
            if (m_MovedVec.x - m_ClickedVec.x > 0)
            {
                DIRECTION = RIGHT;
                SetTarget(this.x + 1, this.y);
            }
            // X��ǥ -
            if (m_MovedVec.x - m_ClickedVec.x < 0)
            {
                DIRECTION = LEFT;
                SetTarget(this.x - 1, this.y);
            }
        }
        else
        {
            // Y��ǥ +
            if (m_MovedVec.y - m_ClickedVec.y > 0)
            {
                DIRECTION = UP;
                SetTarget(this.x, this.y + 1);
            }
            // Y��ǥ -
            if (m_MovedVec.y - m_ClickedVec.y < 0)
            {
                DIRECTION = DOWN;
                SetTarget(this.x, this.y - 1);
            }
        }
    }

    public void SetTarget(int x, int y)
    {
        m_TargetTile = Board.GetTile(x, y);
    }

    public void SwapPosition()
    {
        m_TileBehaivour.StartCoroutine(MoveFromTo());
        m_TileBehaivour.StartCoroutine(MoveToFrom());

        Board.SwapIdx(this, m_TargetTile);
    }

    private IEnumerator MoveFromTo()
    {
        return m_TileBehaivour.MoveTo(this, m_TargetTile.transform.position, m_Duration);
    }

    private IEnumerator MoveToFrom()
    {
        return m_TileBehaivour.MoveTo(m_TargetTile, this.transform.position, m_Duration);
    }

    /*
    private void Match()
    {
        if (MatchUpX() || MatchUpY())
        {
            
        }
    }

    private bool MatchUpX()
    {
        // �� Ÿ��
        Tile comparedTile = new Tile();

        // MatchUp ����Ʈ�� �ڽ� ����
        m_MatchList.Add(this);

        // "����" MatchUp �˻�
        for (int x = GetX() - 1; x > 0; x--)
        {
            comparedTile = Board.GetTile(x, m_TargetTile.GetY());
            if (x != GetX())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        // "������" MatchUp �˻�
        for (int x = GetX() + 1; x < Board.m_Width; x++)
        {
            comparedTile = Board.GetTile(x, m_TargetTile.GetY());
            if (x != GetX())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        return UpdateMatchStateOfTiles();
    }

    private bool MatchUpY()
    {
        // �� Ÿ��
        Tile comparedTile = new Tile();

        // MatchUp ����Ʈ�� �ڽ� ����
        m_MatchList.Add(this);

        // "�Ʒ���" MatchUp �˻�
        for (int y = GetY() - 1; y > 0; y--)
        {
            comparedTile = Board.GetTile(m_TargetTile.GetX(), y);
            if (y != GetY())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        // "����" MatchUp �˻�
        for (int y = GetY() + 1; y < Board.m_Height; y++)
        {
            comparedTile = Board.GetTile(m_TargetTile.GetX(), y);
            if (y != GetY())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        return UpdateMatchStateOfTiles();
    }

    private bool UpdateMatchStateOfTiles()
    {
        bool result = false;

        if (m_MatchList.Count > 2)
        {
            for (int idx = 0; idx < m_MatchList.Count; idx++) m_MatchList[idx].MATCH = true;
            result = true;
        }

        return result;
    }
    */
}
