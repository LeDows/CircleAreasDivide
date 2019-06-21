using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Circle> Circles = new List<Circle>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CheckAreas(new Circle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1));
        }
    }

    void CheckAreas(Circle circle)
    {
        for (int i = 0; i < Circles.Count; i++)
        {
            Circles[i].Divide(circle);
        }
        Circles.Add(circle);
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i=0;i<Circles.Count;i++)
        {
            Circles[i].Draw();
        }        
    }
}
