using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaivour : MonoBehaviour
{
    private float TILE_MOVE_DURATION = 0.5f;

    public IEnumerator MoveTo(Tile moved, Vector3 to)
    {
        Vector2 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < TILE_MOVE_DURATION)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.position = Vector2.Lerp(startPos, to, elapsed / TILE_MOVE_DURATION);

            yield return null;
        }

        moved.transform.position = to;

        yield break;
    }
}
