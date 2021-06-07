using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Sensor
{
    Ray2D centerForward;
    Ray2D rightForward;
    Ray2D leftForward;
    Ray2D rightBack;
    Ray2D leftBack;
    Ray2D centerBack;

    public Ray2D RightForward { get => rightForward; set => rightForward = value; }
    public Ray2D LeftForward { get => leftForward; set => leftForward = value; }
    public Ray2D CenterForward { get => centerForward; set => centerForward = value; }
    public Ray2D LeftBack { get => leftBack; set => leftBack = value; }
    public Ray2D RightBack { get => rightBack; set => rightBack = value; }
    public Ray2D CenterBack { get => centerBack; set => centerBack = value; }

    public Sensor(Vector2 coordinates)
    {
        centerForward = new Ray2D(coordinates, new Vector2(0, 8f));
        leftForward = new Ray2D(coordinates, new Vector2(-8f, 8f));
        rightForward = new Ray2D(coordinates, new Vector2(8f, 8f));
        rightBack = new Ray2D(coordinates, new Vector2(8f, -8f));
        leftBack = new Ray2D(coordinates, new Vector2(-8f, -8f));
        centerBack = new Ray2D(coordinates, new Vector2(0, -8f));
    }

    //Двигаем сенсоры машины.
    public void MovementRays(Vector2 coordinates)
    {
        centerForward.origin = coordinates;
        rightForward.origin = coordinates;
        leftForward.origin = coordinates;
        rightBack.origin = coordinates;
        leftBack.origin = coordinates;
        centerBack.origin = coordinates;
    }

    //Поворачивает сенсоры, вместе с машиной.
    public void RotateRays(Transform dir)
    {
        centerForward.direction = dir.up;
        centerBack.direction = -dir.up;
        rightForward.direction = new Vector2(dir.up.x * 2 + dir.right.x / 2, dir.up.y * 2 + dir.right.y);
        leftForward.direction = new Vector2(dir.up.x * 2 - dir.right.x, dir.up.y * 2 - dir.right.y);
        rightBack.direction = new Vector2(-dir.up.x * 2 + dir.right.x / 2, -dir.up.y * 2 + dir.right.y);
        leftBack.direction = new Vector2(-dir.up.x * 2 - dir.right.x, -dir.up.y * 2 - dir.right.y);
    }


}

