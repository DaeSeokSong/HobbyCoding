using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour
{
    // Scale
    public const float DIAMETER = 105f;

    // Move
    // Move Direction
    public const int RIGHT = 1;
    public const int LEFT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
    // Moving Time
    public const float DURATION = 0.3f;
    public const float DESTROY_DURATION = DURATION * 3f;

    // Destroy
    public const float DESTROY_SCALE = 30f;
    public const float DESTROY_SPEED = 60f;
}
