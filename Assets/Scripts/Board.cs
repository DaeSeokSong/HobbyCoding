using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Public
    // Tile List
    public static Tile[,] TileArray;
    // MatchUp List
    public static List<Tile> MatchList = new List<Tile>();
    // IsMoveDown?
    public static bool MOVEDOWN = false;
    // IsAfterMatchUp?
    public static bool AFTERMATCHUP = false;
    // Width, Height
    public static int Width = 8;
    public static int Height = 8;

    // Private
    // Prefabs
    private GameObject m_TileRed;
    private GameObject m_TileBlue;
    private GameObject m_TileGreen;
    private GameObject m_TilePurple;
    private GameObject m_TileOrange;
    private GameObject m_TileYellow;
    private GameObject[] m_TileTypes;
    // Controller
    private BoardController m_BoardController;

    // Start is called before the first frame update
    void Start()
    {
        // Init Controller
        m_BoardController = new BoardController(this.transform);

        // Init Prefab
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
        if (MOVEDOWN || AFTERMATCHUP) ReplaceTiles();
    }

    /*
     * =================================================== public ===================================================
     */
    /// <summary>
    /// Get specific Tile about X, Y
    /// </summary>
    /// <param name="x">Coordination X</param>
    /// <param name="y">Coordination Y</param>
    /// <returns>Tile about (X, Y)</returns>
    public static Tile GetTile(int x, int y) { return TileArray[x, y]; }

    public static void SwapIdx(Tile moved, Tile to)
    {
        TileArray[moved.GetX(), moved.GetY()] = to;
        TileArray[to.GetX(), to.GetY()] = moved;

        int tmpX = moved.GetX();
        int tmpY = moved.GetY();
        moved.SetX(to.GetX());
        moved.SetY(to.GetY());
        to.SetX(tmpX);
        to.SetY(tmpY);
    }

    /*
     * =================================================== private ===================================================
     */
    /// <summary>
    /// Init GameObjects(Tiles)
    /// </summary>
    private void CreateTiles()
    {
        // Init list for store about tiles
        TileArray = new Tile[Width, Height * 2];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());

                tile.name = prefab.name;

                // Set Coordination
                tile.SetX(x);
                tile.SetY(y);

                // Set parent about transform
                tile.transform.SetParent(this.transform);
                // Set position
                tile.transform.localScale = new Vector3(60, 60, 0);
                tile.transform.localPosition = new Vector3(-320 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2))), -730 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2))), 200);

                TileArray[x, y] = tile;
            }
        }

        ArrangeReplaceTiles();
    }

    /// <summary>
    /// Init ArrangedTiles
    /// </summary>
    private void ArrangeReplaceTiles()
    {
        // Arranged Tiles Create (Active = false)
        for (int x = 0; x < Width; x++)
        {
            for (int y = Height; y < Height * 2; y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());

                // Set Deactivate
                tile.gameObject.SetActive(false);
                tile.name = prefab.name;

                // Set Coordination
                tile.SetX(x);
                tile.SetY(y);

                // Set parent about transform
                tile.transform.SetParent(this.transform);
                // Set position
                tile.transform.localScale = new Vector3(60, 60, 0);
                tile.transform.localPosition = new Vector3(-320 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2))), -730 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2))), 200);

                TileArray[x, y] = tile;
            }
        }
    }

    /// <summary>
    /// MoveDown arranged Tiles
    /// </summary>
    private void ReplaceTiles()
    {
        int axisX = 0;
        int startMoveDownIdxY = 0;
        for (int x = 0; x < Width; x++)
        {
            int moveDownCnt = 0;
            for (int y = 0; y < Height; y++)
            {
                if (TileArray[x, y].DESTROY)
                {
                    moveDownCnt++;
                    startMoveDownIdxY = y;
                }
            }

            if (moveDownCnt != 0)
            {
                axisX = x;
                for (int idx = startMoveDownIdxY + 1; idx < Height + moveDownCnt; idx++)
                {
                    TileArray[x, idx].MoveDown(moveDownCnt);
                    TileArray[x, idx - moveDownCnt] = TileArray[x, idx];
                    TileArray[x, idx - moveDownCnt].SetX(x);
                    TileArray[x, idx - moveDownCnt].SetY(idx - moveDownCnt);
                }
            }
        }

        MatchList.Clear();
        ArrangeReplaceTiles();
        m_BoardController.StartCoroutine(m_BoardController.EvoluteMatchUp(axisX));
    }
}
