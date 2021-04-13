using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private ActionController m_ActionController;

    void Start()
    {
        m_ActionController = new ActionController(this.transform);
    }
    
    public IEnumerator DoCoSwapTiles(Tile moved, Tile to)
    {
        m_ActionController.StartCoroutine(moved.TileBehaivour.MoveTo(moved, to.transform.position));
        m_ActionController.StartCoroutine(moved.TileBehaivour.MoveTo(to, moved.transform.position));

        yield return new WaitForSeconds(0.5f);
    }
}
