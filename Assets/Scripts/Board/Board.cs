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
    // Tiles
    private GameObject m_Tile1;
    private GameObject m_Tile2;
    private GameObject m_Tile3;
    private GameObject m_Tile4;
    private GameObject m_Tile5;
    private GameObject[] m_TileTypes;
    // Tile's background
    private GameObject m_TileBackground;

    // Start is called before the first frame update
    void Start()
    {
        // Init Tiles
        m_Tile1 = Resources.Load("Prefabs/Tile1") as GameObject;
        m_Tile2 = Resources.Load("Prefabs/Tile2") as GameObject;
        m_Tile3 = Resources.Load("Prefabs/Tile3") as GameObject;
        m_Tile4 = Resources.Load("Prefabs/Tile4") as GameObject;
        m_Tile5 = Resources.Load("Prefabs/Tile5") as GameObject;
        m_TileTypes = new GameObject[] { m_Tile1, m_Tile2, m_Tile3, m_Tile4, m_Tile5 };

        // Init Tile's background
        m_TileBackground = Resources.Load("Prefabs/Ingredient") as GameObject;

        CreateTiles();
        
    }

    void Update()
    {
        if (MOVEDOWN)
        {
            ArrangeReplaceTiles();
            MOVEDOWN = false;
        }
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
                // Tile
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());
                tile.name = prefab.name;

                // Background
                GameObject background = Instantiate<GameObject>(m_TileBackground.transform.gameObject);
                background.name = "Background";

                // Set Coordination
                tile.SetX(x);
                tile.SetY(y);

                // Set parent about transform
                tile.transform.SetParent(this.transform);
                background.transform.SetParent(this.transform);

                // Set Position
                // Set Scale
                tile.transform.localScale = new Vector3(60, 60, 0);
                background.transform.localScale = new Vector3(100, 100, 0);
                // Compute Position
                float initX = -310 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2)));
                float initY = -730 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2)));
                // Set Position
                tile.transform.localPosition = new Vector3(initX, initY, 100);
                background.transform.localPosition = new Vector3(initX, initY, 200);

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
                // Compute Position
                float initX = -310 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2)));
                float initY = -730 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2)));
                tile.transform.localPosition = new Vector3(initX, initY, 100);

                TileArray[x, y] = tile;
            }
        }
    }

    private bool OverlapTile(int x, int y, int a)
    {
        int count = 0;
        GameObject Base = GameObject.Find("Base").transform.GetChild(3).gameObject;
        string name = Base.transform.GetChild(8 - y + (8 * x) - 1 + a).name;
        //x는 0부터 계산 y는 0부터 계산

        if (y > 0 && name == Base.transform.GetChild(8 - y + 1 + (8 * x) - 1 + a).name) count++;//위
        if (y < 7 && name == Base.transform.GetChild(8 - y - 1 + (8 * x) - 1 + a).name) count++;//아래
        if (x < 7 && name == Base.transform.GetChild(8 - y + (8 * (x + 1)) - 1 + a).name) count++;//오른쪽
        if (x > 0 && name == Base.transform.GetChild(8 - y + (8 * (x - 1)) - 1 + a).name) count++;//왼쪽
        
        if (count > 2) return true;
        else return false;
    }
}
