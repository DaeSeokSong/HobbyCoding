using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Public
    public static int m_Width = 8;
    public static int m_Height = 8;

    // Private
    // ���� ������Ʈ ���� �Ķ����
    private GameObject m_TileRed;
    private GameObject m_TileBlue;
    private GameObject m_TileGreen;
    private GameObject m_TilePurple;
    private GameObject m_TileOrange;
    private GameObject m_TileYellow;
    private GameObject[] m_TileTypes;
    private Tile[,] m_TileArray;
    // ��ǥ ���� �Ķ����
    private int m_movedX, m_movedY;
    private int m_targetX, m_targetY;

    // Start is called before the first frame update
    void Start()
    {
        // Prefab = ����Ʋ(��Ǫ��), ���� ������Ʈ�� �����ϰ� �ټ� ���� ����
        // ���� ������Ʈ ���� �ʱ�ȭ
        m_TileRed = Resources.Load("Prefabs/CandyRed") as GameObject;
        m_TileBlue = Resources.Load("Prefabs/CandyBlue") as GameObject;
        m_TileGreen = Resources.Load("Prefabs/CandyGreen") as GameObject;
        m_TilePurple = Resources.Load("Prefabs/CandyPurple") as GameObject;
        m_TileOrange = Resources.Load("Prefabs/CandyOrange") as GameObject;
        m_TileYellow = Resources.Load("Prefabs/CandyYellow") as GameObject;
        m_TileTypes = new GameObject[] { m_TileRed, m_TileBlue, m_TileGreen, m_TilePurple, m_TileOrange, m_TileYellow };

        CreateTiles();
    }

    void Update()
    {
        // ������ Ÿ���� ������ ��
        if (IsExistMovedTile())
        {
            // Ÿ�� �̵�
            MoveTile();

            if (IsMatchUpTile())
            {
                // MatchUp�� Ÿ���� ���� ��
            }
            else
            {
                // MatchUp�� Ÿ���� ���� �� (Ÿ�� ���ڸ�)
                SwapPosition();
                SwapIdx();
            }

            InitDirectionZero();
        }
    }

    /// <summary>
    /// Prefab �̿��Ͽ� ���ο� Tile ����
    /// </summary>
    private void CreateTiles()
    {
        // ���� Ÿ�� �迭 �ʱ�ȭ
        m_TileArray = new Tile[m_Width, m_Height];

        for (int x = 0; x < m_TileArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_TileArray.GetLength(1); y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());
                tile.name = prefab.name;

                // ��ǥ ����
                tile.SetX(x);
                tile.SetY(y);

                // �θ� ����
                tile.transform.SetParent(this.transform);
                // ��ġ ����
                tile.transform.position = new Vector2(x + (x * tile.transform.localScale.x) / 2, y + (y * tile.transform.localScale.y) / 2);

                m_TileArray[x, y] = tile;
            }
        }
    }

    private void InitDirectionZero()
    {
        m_TileArray[m_movedX, m_movedY].DIRECTION = 0;
        m_TileArray[m_targetX, m_targetY].DIRECTION = 0;
    }

    private void SetMoved(int x, int y)
    {
        this.m_movedX = x;
        this.m_movedY = y;
    }

    private void SetTarget(int x, int y)
    {
        this.m_targetX = x;
        this.m_targetY = y;
        if (m_TileArray[m_movedX, m_movedY].DIRECTION % 2 == 1) m_TileArray[m_targetX, m_targetY].DIRECTION = m_TileArray[m_movedX, m_movedY].DIRECTION + 1;
        else m_TileArray[m_targetX, m_targetY].DIRECTION = m_TileArray[m_movedX, m_movedY].DIRECTION - 1;
    }

    /// <summary>
    /// �̵���ų Ÿ���� �ִ��� Ȯ���Ѵ�.
    /// </summary>
    /// <returns>movedTile is exist then return true / or return false</returns>
    private bool IsExistMovedTile()
    {
        bool result = false;

        for (int x = 0; x < m_Width; x++)
            for (int y = 0; y < m_Height; y++)
            {
                if (m_TileArray[x, y].DIRECTION != 0)
                {
                    SetMoved(x, y);
                    result = true;
                }
            }

        return result;
    }

    private void MoveTile()
    {
        switch (m_TileArray[m_movedX, m_movedY].DIRECTION)
        {
            case Tile.RIGHT:
                if (m_movedX + 1 < m_Width)
                {
                    SetTarget(m_movedX + 1, m_movedY);
                    SwapPosition();
                    SwapIdx();
                }
                else break;

                break;

            case Tile.LEFT:
                if (m_movedX - 1 > 0)
                {
                    SetTarget(m_movedX - 1, m_movedY);
                    SwapPosition();
                    SwapIdx();
                }
                else break;

                break;

            case Tile.UP:
                if (m_movedY + 1 < m_Height)
                {
                    SetTarget(m_movedX, m_movedY + 1);
                    SwapPosition();
                    SwapIdx();
                }
                else break;

                break;

            case Tile.DOWN:
                if (m_movedY - 1 > 0)
                {
                    SetTarget(m_movedX, m_movedY - 1);
                    SwapPosition();
                    SwapIdx();
                }
                else break;

                break;

            default:
                break;
        }
    }

    private void SwapPosition()
    {
        Tile movedTile = m_TileArray[m_movedX, m_movedY];
        Tile targetTile = m_TileArray[m_targetX, m_targetY];

        movedTile.SetMovedTo(targetTile.transform.position);
        //targetTile.SetMovedTo(movedTile.transform.position);
    }

    private void SwapIdx()
    {
        Tile tmpTile = m_TileArray[m_movedX, m_movedY];
        m_TileArray[m_movedX, m_movedY] = m_TileArray[m_targetX, m_targetY];
        m_TileArray[m_targetX, m_targetY] = tmpTile;
    }

    private bool IsMatchUpTile()
    {
        List<Tile> matchList = new List<Tile>();
        matchList.Add(m_TileArray[m_targetX, m_targetY]);

        MatchUpX(matchList);
        MatchUpY(matchList);

        // �������� Ÿ��(target)�� MatchUp �˻�
        if (m_TileArray[m_movedX, m_movedY].DIRECTION > 0 && m_TileArray[m_movedX, m_movedY].DIRECTION <= 2) MatchUpY(matchList);
        else if (m_TileArray[m_movedX, m_movedY].DIRECTION > 2 && m_TileArray[m_movedX, m_movedY].DIRECTION <= 4) MatchUpX(matchList);

        // Ÿ���� Match ���� ����
        return UpdateMatchStateOfTiles(matchList);
    }

    private void MatchUpX(List<Tile> matchList)
    {
        // "����" MatchUp �˻�
        for (int x = m_targetX - 1; x > 0; x--)
        {
            if (x != m_targetX)
            {
                if (m_TileArray[m_targetX, m_targetY].name.Equals(m_TileArray[x, m_targetY].name) && !matchList.Contains(m_TileArray[x, m_targetY])) matchList.Add(m_TileArray[x, m_targetY]);
                else break;
            }
        }

        // "������" MatchUp �˻�
        for (int x = m_targetX + 1; x < m_Width; x++)
        {
            if (x != m_targetX)
            {
                if (m_TileArray[m_targetX, m_targetY].name.Equals(m_TileArray[x, m_targetY].name) && !matchList.Contains(m_TileArray[x, m_targetY])) matchList.Add(m_TileArray[x, m_targetY]);
                else break;
            }
        }
    }

    private void MatchUpY(List<Tile> matchList)
    {
        // "�Ʒ���" MatchUp �˻�
        for (int y = m_targetY - 1; y > 0; y--)
        {
            if (y != m_targetY)
            {
                if (m_TileArray[m_targetX, m_targetY].name.Equals(m_TileArray[m_targetX, y].name) && !matchList.Contains(m_TileArray[m_targetX, y])) matchList.Add(m_TileArray[m_targetX, y]);
                else break;
            }
        }

        // "����" MatchUp �˻�
        for (int y = m_targetY + 1; y < m_Height; y++)
        {
            if (y != m_targetY)
            {
                if (m_TileArray[m_targetX, m_targetY].name.Equals(m_TileArray[m_targetX, y].name) && !matchList.Contains(m_TileArray[m_targetX, y])) matchList.Add(m_TileArray[m_targetX, y]);
                else break;
            }
        }
    }

    private bool UpdateMatchStateOfTiles(List<Tile> matchList)
    {
        bool result = false;

        if (matchList.Count > 2)
        {
            for (int idx = 0; idx < matchList.Count; idx++) matchList[idx].MATCH = true;
            result = true;
        }

        return result;
    }

    private bool IsExistMatchUpOthers()
    {
        bool result = false;

        for (int x = 0; x < m_Width; x++)
            for (int y = 0; y < m_Height; y++)
            {

            }

        return result;
    }
}
