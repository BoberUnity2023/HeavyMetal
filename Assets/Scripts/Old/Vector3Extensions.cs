using UnityEngine;
using UnityEngine.UIElements;

public static class Vector3Extensions
{
    public static Vector3 WithX(this Vector3 value, float x)
    {
        value.x = x;
        return value;
    }

    public static Vector3 WithY(this Vector3 value, float y)
    {
        value.y = y;
        return value;
    }

    public static Vector3 WithZ(this Vector3 value, float z)
    {
        value.z = z;
        return value;
    }

    public static Vector3 AddX(this Vector3 value, float x)
    {
        value.x += x;
        return value;
    }

    public static Vector3 AddY(this Vector3 value, float y)
    {
        value.y += y;
        return value;
    }

    public static Vector3 AddZ(this Vector3 value, float z)
    {
        value.z += z;
        return value;
    }

    public static float Distance(Transform from, Transform to)
    {        
        return Vector3.Distance(from.position, to.position);
    }

    public static float Distance(Component from, Component to)
    {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }

    public static float Distance(GameObject from, GameObject to)
    {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }
}
