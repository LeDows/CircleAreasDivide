using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
public class Circle
{
    static int idIndex = 1;
    public int ID { get; private set; }
    public Vector3 Center;
    public List<Vector3> Sides;
    public float Radius;
    public Circle(Vector3 pos, float radius)
    {
        Center = pos;
        Sides = new List<Vector3>(64);
        Radius = radius;
        for (int i = 0; i < 64; i++)
        {
            Sides.Add(WayToVec3(i / 64f * 360) * radius + Center);
        }
        ID = idIndex++;
    }


    public void Divide(Circle other)
    {
        if (Vector3.Distance(Center, other.Center) >= Radius + other.Radius)
            return;
        Vector3 dir = (other.Center - Center) * 0.5f;
        Vector3 ndir = dir.normalized;
        float length = dir.magnitude;
        Vector3 sdir;
        float dot;
        for (int i = 0; i < Sides.Count; i++)
        {
            if (Vector3.Distance(Sides[i], other.Center) < other.Radius)
            {
                sdir = Sides[i] - Center;
                dot = Vector3.Dot(sdir, ndir);
                if(dot>length)
                {
                    sdir *= length / dot;
                    Sides[i] = sdir + Center;
                }
            }
        }
        ndir = -ndir;
        for (int i = 0; i <other.Sides.Count; i++)
        {
            if (Vector3.Distance(other.Sides[i], Center) < Radius)
            {
                sdir = other.Sides[i] - other.Center;
                dot = Vector3.Dot(sdir, ndir);
                if (dot > length)
                {
                    sdir *= length / dot;
                    other.Sides[i] = sdir + other.Center;
                }
            }
        }
    }





    public void Draw()
    {
        for (int i = 0; i < Sides.Count - 1; i++)
        {
            Gizmos.DrawLine(Sides[i], Sides[i + 1]);
        }
        Gizmos.DrawLine(Sides[Sides.Count - 1], Sides[0]);
        Gizmos.DrawSphere(Center, 0.05f);
    }


    public static Vector3 WayToVec3(float way)
    {
        Vector3 dir = Vector3.zero;
        dir.x = Mathf.Cos(way * Mathf.Deg2Rad);
        dir.y = -Mathf.Sin(way * Mathf.Deg2Rad);
        return dir;
    }
}

