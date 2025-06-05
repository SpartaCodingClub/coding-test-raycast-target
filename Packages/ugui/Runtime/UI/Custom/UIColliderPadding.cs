using System;
using UnityEngine;

[Serializable]
public struct UIColliderPadding
{
    public float Left;
    public float Bottom;
    public float Right;
    public float Top;

    public UIColliderPadding(float left, float bottom, float right, float top)
    {
        Left = left;
        Bottom = bottom;
        Right = right;
        Top = top;
    }

    public UIColliderPadding(Vector4 v)
    {
        Left = v.x;
        Bottom = v.y;
        Right = v.z;
        Top = v.w;
    }

    public readonly Vector4 ToVector4()
    {
        return new(Left, Bottom, Right, Top);
    }
}