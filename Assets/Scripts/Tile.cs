using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Public
    public int DIRECTION = 0;
    public bool MATCH = false;
    public bool DESTROY = false;

    // Private
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

    void Start()
    {
        // Ÿ�� �ൿ ��ü
        m_TileBehaivour = new TileBehaivour(this.transform);
        m_TargetTile = new Tile();
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
        // TileBehaivour execute
        if (m_TargetTile != null) StartBehaivor();
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
                DIRECTION = TileStatus.RIGHT;
                SetTarget(this.x + 1, this.y);
            }
            // X��ǥ -
            if (m_MovedVec.x - m_ClickedVec.x < 0)
            {
                DIRECTION = TileStatus.LEFT;
                SetTarget(this.x - 1, this.y);
            }
        }
        else
        {
            // Y��ǥ +
            if (m_MovedVec.y - m_ClickedVec.y > 0)
            {
                DIRECTION = TileStatus.UP;
                SetTarget(this.x, this.y + 1);
            }
            // Y��ǥ -
            if (m_MovedVec.y - m_ClickedVec.y < 0)
            {
                DIRECTION = TileStatus.DOWN;
                SetTarget(this.x, this.y - 1);
            }
        }
    }

    public void SetTarget(int x, int y)
    {
        m_TargetTile = Board.GetTile(x, y);
        if (this.DIRECTION % 2 == 0) m_TargetTile.DIRECTION = this.DIRECTION + 1;
        else m_TargetTile.DIRECTION = this.DIRECTION - 1;
    }

    public void StartBehaivor()
    {
        // ��ġ ��ȯ �� ���� ���� �ε��� ��ȯ
        SwapPosition();
        // Ÿ�� MatchUp
        Match();
    }

    private void SwapPosition()
    {
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.CoStartMove(this, m_TargetTile));
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.CoStartMove(m_TargetTile, this));

        Board.SwapIdx(this, m_TargetTile);
    }

    private void Match()
    {
        int beforeCnt = 0;

        // X Axis MathUp
        this.MatchUpX();
        // Y Axis MathUp
        this.MatchUpY();
        // MatchUp put in movedTile
        if (Board.MatchList.Count != 0)
        {
            Board.MatchList.Add(this);
            beforeCnt = Board.MatchList.Count;
        }

        // TargetTile Axis MathUp
        if (m_TargetTile.DIRECTION == TileStatus.RIGHT || m_TargetTile.DIRECTION == TileStatus.LEFT) m_TargetTile.MatchUpY();
        else m_TargetTile.MatchUpX();
        if (!Board.MatchList.Contains(m_TargetTile) && (Board.MatchList.Count - beforeCnt) >= 2) Board.MatchList.Add(m_TargetTile);

        // MatchUped Tile is exist?
        if (UpdateMatchStateOfTiles())
        {
            // MatchUped Tile set up for Destroy
            SetUpDestroy();
        }
        else
        {
            // Reset Position and Index
            SwapPosition();
            m_TileBehaivour.InitRunningMove();
        }
    }

    public void MatchUpX()
    {
        // �ӽ� ���� ����Ʈ
        List<Tile> tmpMatchList = new List<Tile>();
        // �� Ÿ��
        Tile comparedTile = new Tile();

        // "����" MatchUp �˻�
        for (int x = GetX() - 1; x >= 0; x--)
        {
            comparedTile = Board.GetTile(x, this.GetY());
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        // "������" MatchUp �˻�
        for (int x = GetX() + 1; x < Board.Width; x++)
        {
            comparedTile = Board.GetTile(x, this.GetY());
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        if (tmpMatchList.Count < 2) tmpMatchList.Clear();
        else
        {
            for (int idx = 0; idx < tmpMatchList.Count; idx++)
                if (!Board.MatchList.Contains(tmpMatchList[idx])) Board.MatchList.Add(tmpMatchList[idx]);
        }
    }

    public void MatchUpY()
    {
        // �ӽ� ���� ����Ʈ
        List<Tile> tmpMatchList = new List<Tile>();
        // �� Ÿ��
        Tile comparedTile = new Tile();

        // "�Ʒ���" MatchUp �˻�
        for (int y = GetY() - 1; y >= 0; y--)
        {
            comparedTile = Board.GetTile(this.GetX(), y);
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        // "����" MatchUp �˻�
        for (int y = GetY() + 1; y < Board.Height; y++)
        {
            comparedTile = Board.GetTile(this.GetX(), y);
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        if (tmpMatchList.Count < 2) tmpMatchList.Clear();
        else
        {
            for (int idx = 0; idx < tmpMatchList.Count; idx++)
                if (!Board.MatchList.Contains(tmpMatchList[idx])) Board.MatchList.Add(tmpMatchList[idx]);
        }
    }

    private bool UpdateMatchStateOfTiles()
    {
        bool result = false;

        if (Board.MatchList.Count > 2)
        {
            for (int idx = 0; idx < Board.MatchList.Count; idx++) Board.MatchList[idx].MATCH = true;
            result = true;
        }

        return result;
    }

    private void SetUpDestroy()
    {
        for (int idx = 0; idx < Board.MatchList.Count; idx++) Board.MatchList[idx].Destroy();
        Board.MatchList.Clear();
    }

    public void Destroy()
    {
        m_TileBehaivour.StartCoroutine(CoDoDestroy());
    }

    private IEnumerator CoDoDestroy()
    {
        return m_TileBehaivour.CoStartDestroy(this);
    }
}