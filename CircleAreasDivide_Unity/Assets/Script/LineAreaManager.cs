using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAreaManager : MonoBehaviour
{
    List<Area> Areas = new List<Area>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckLine(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void CheckLine(Vector3 point)
    {
        Area area = new Area();
        area.Center = point;

        Vector3 dir;
        Vector3 center;
        Vector3 nor;
        Line line;
        foreach (Area a in Areas)
        {
            nor = a.Center - point;
            dir = Vector3.Cross(Vector3.forward, nor).normalized;
            center = point + nor * 0.5f;
            nor = nor.normalized;
            a.Check(new Line(center, dir, nor, 1000, -1000));
            area.Check(new Line(center, dir, -nor, 1000, -1000));
        }
        Areas.Add(area);
    }


    public void OnDrawGizmos()
    {
        foreach (Area a in Areas)
        {
            a.Draw();
        }
    }
}

public class Line
{
    public Vector3 Start
    {
        get
        {
            return Center + Driction * Mint;
        }
    }
    public Vector3 End
    {
        get
        {
            return Center + Driction * Maxt;
        }
    }

    public Vector3 Center;
    public Vector3 Driction;
    public Vector3 Normal;
    public float Maxt;
    public float Mint;


    public Line(Vector3 center, Vector3 driction, Vector3 normal, float max, float min)
    {
        Center = center;
        Normal = normal;
        Driction = driction;
        Maxt = max;
        Mint = min;
    }

    public bool Intersect(Line other)//1 平行 2 相交 3 不相交
    {
        float dot = Driction.x * other.Normal.x + Driction.y * other.Normal.y + Driction.z * other.Normal.z;
        if (dot != 0)
        {
            float t = (other.Normal.x * (other.Center.x - Center.x) + other.Normal.y * (other.Center.y - Center.y) + other.Normal.z * (other.Center.z - Center.z)) / dot;
            if (t >= Mint && t <= Maxt)
            {
                Vector3 point = Center + Driction * t;
                float m = 0;
                if (other.Driction.x != 0)
                    m = (point.x - other.Center.x) / other.Driction.x;
                else if (other.Driction.y != 0)
                    m = (point.y - other.Center.y) / other.Driction.y;
                else if (other.Driction.z != 0)
                    m = (point.z - other.Center.z) / other.Driction.z;
                if (m >= other.Mint && m <= other.Maxt)
                {
                    if (Vector3.Dot(Start - other.Center, other.Normal) > 0)
                    {
                        Maxt = t;
                    }
                    else
                    {
                        Mint = t;
                    }
                    if (Vector3.Dot(other.Start - Center, Normal) > 0)
                    {
                        other.Maxt = m;
                    }
                    else
                    {
                        other.Mint = m;
                    }
                    return true;
                }
            }
        }
        return false;
    }
}

public class Area
{
    public Vector3 Center;
    public List<Line> Lines = new List<Line>();
    List<Line> remove = new List<Line>();

    public void Check(Line line)
    {
        bool canadd = false;
        bool allin = true;
        remove.Clear();
        foreach (Line l in Lines)
        {
            if(l.Intersect(line))
            {
                canadd = true;
            }
            else
            {
                if (Vector3.Dot(line.Start - l.Center, l.Normal) < 0 || Vector3.Dot(line.End - l.Center, l.Normal)<0)
                    allin = false;
            }
        }
        if (canadd||allin)
        {
            foreach (Line l in Lines)
            {
                if (Vector3.Dot(l.Start - line.Center, line.Normal) <= 0 && Vector3.Dot(l.End - line.Center, line.Normal) <= 0)
                    remove.Add(l);
            }

            foreach (Line l in remove)
            {
                Lines.Remove(l);
            }
            Lines.Add(line);
        }
    }

    public void Draw()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Center, 0.05f);
        //Gizmos.color = Color.red;
        foreach (Line l in Lines)
        {
            Gizmos.DrawLine(l.Start, l.End);
        }
    }
}





