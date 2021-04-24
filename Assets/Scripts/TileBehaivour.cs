using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaivour : MonoBehaviour
{
    // Private
    // MonoBehaviour
    private MonoBehaviour m_MonoBehaviour;
    // Container
    private Transform m_Container;
    // IsRunning
    private int m_IsRunningMove = 0;
    private int m_IsRunningMoveDown = 0;

    public TileBehaivour(Transform container)
    {
        m_Container = container;
        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    /*
     * =================================================== public ===================================================
     */
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    /// <summary>
    /// Init Running state about move
    /// </summary>
    public void InitRunningMove() { this.m_IsRunningMove = 0; }

    /// <summary>
    /// Action about move
    /// </summary>
    /// <param name="moved">Moved tile</param>
    /// <param name="to">Target tile's Vector(2D) to move</param>
    /// <returns>Coroutine</returns>
    public IEnumerator CoStartMove(Tile moved, Vector3 to)
    {
        if (m_IsRunningMove > 2) yield return null;
        m_IsRunningMove++;

        Vector3 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < TileStatus.DURATION)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.localPosition = Vector3.Lerp(startPos, to, elapsed / TileStatus.DURATION);

            yield return null;
        }
        moved.transform.localPosition = to;

        m_IsRunningMove--;

        yield break;
    }

    /// <summary>
    /// Action about destroy
    /// </summary>
    /// <param name="destroyedTile">Destroied tile</param>
    /// <returns>Coroutine</returns>
    public IEnumerator CoStartDestroy(Tile destroyedTile)
    {
        // Delay about destroying for moving time
        yield return new WaitForSeconds(TileStatus.DURATION);

        // SizeDown Action : 1 -> 0.3(TileStatus.DESTROY_SCALE)
        yield return Scale(destroyedTile.transform, TileStatus.DESTROY_SCALE, TileStatus.DESTROY_SPEED);

        // Delay about destroy
        yield return new WaitForSeconds(0.1f);

        // Destroy
        destroyedTile.DESTROY = true;
        Board.MatchList.Remove(destroyedTile);
        Destroy(destroyedTile.gameObject);
    }

    /// <summary>
    /// Action about move down
    /// </summary>
    /// <param name="moved">Moved tile</param>
    /// <param name="to">Target tile to move</param>
    /// <returns>Coroutine</returns>
    public IEnumerator CoStartMoveDown(Tile moved, Vector3 to)
    {
        // Delay about Movedown for moving and destroying time
        yield return new WaitForSeconds(TileStatus.DURATION);
        m_IsRunningMoveDown++;

        Vector3 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < TileStatus.DURATION)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.position = Vector3.Lerp(startPos, to, elapsed / TileStatus.DURATION);

            yield return null;
        }
        moved.transform.position = to;

        m_IsRunningMoveDown--;
        if (m_IsRunningMoveDown == 0) Board.MOVEDOWN = false;
        yield break;
    }

    /*
     * =================================================== private ===================================================
     */
    /// <summary>
    /// Action about tile's scale size down
    /// </summary>
    /// <param name="target">Target tile to size down</param>
    /// <param name="toScale">To down size</param>
    /// <param name="speed">How fast speed</param>
    /// <returns>Coroutine</returns>
    private IEnumerator Scale(Transform target, float toScale, float speed)
    {
        float factor;

        while (true)
        {
            factor = Time.deltaTime * speed * -1;
            target.localScale = new Vector3(target.localScale.x + factor, target.localScale.y + factor, target.localScale.z);

            if (target.localScale.x <= toScale) break;

            yield return null;
        }

        yield break;
    }
}
