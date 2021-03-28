using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // public
    public int m_Width = 16;
    public int m_Height = 16;

    // private
    // ������ �迭
    private Tile[,] m_TileArray = new Tile[16, 16];
    private GameObject m_TilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Prefab = ����Ʋ(��Ǫ��), ���� ������Ʈ�� �����ϰ� �ټ� ���� ����
        m_TilePrefab = Resources.Load("Prefabs/CandyPurple") as GameObject;

        CreateTiles();
    }

    /// <summary>
    /// Prefab �̿��Ͽ� ���ο� Tile�� ����
    /// </summary>
    private void CreateTiles()
    {
        for(int x=0; x<m_TileArray.GetLength(0); x++)
        {
            for(int y=0; y<m_TileArray.GetLength(1); y++)
            {
                Tile tile = Instantiate<Tile>(m_TilePrefab.transform.GetComponent<Tile>());

                // �θ� ����
                tile.transform.SetParent(this.transform);
                // ��ġ ����
                tile.transform.position = new Vector3(x, y, 0f);

                m_TileArray[x, y] = tile;
            }
        }
    }
}
