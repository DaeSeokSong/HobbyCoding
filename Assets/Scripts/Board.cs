using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // public
    public int m_Width = 16;
    public int m_Height = 16;

    // private
    // 이차원 배열
    private Tile[,] m_TileArray = new Tile[16, 16];
    private GameObject m_TilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Prefab = 제작틀(거푸집), 게임 오브젝트를 동일하게 다수 생성 가능
        m_TilePrefab = Resources.Load("Prefabs/CandyPurple") as GameObject;

        CreateTiles();
    }

    /// <summary>
    /// Prefab 이용하여 새로운 Tile들 생성
    /// </summary>
    private void CreateTiles()
    {
        for(int x=0; x<m_TileArray.GetLength(0); x++)
        {
            for(int y=0; y<m_TileArray.GetLength(1); y++)
            {
                Tile tile = Instantiate<Tile>(m_TilePrefab.transform.GetComponent<Tile>());

                // 부모 설정
                tile.transform.SetParent(this.transform);
                // 위치 설정
                tile.transform.position = new Vector3(x, y, 0f);

                m_TileArray[x, y] = tile;
            }
        }
    }
}
