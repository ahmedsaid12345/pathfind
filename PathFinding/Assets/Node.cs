using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector3 worldposition;
    public int G_Cost;
    public int H_Cost;

    public int gridX;
    public int gridY;

    public Node Parent;
    public Node(bool _walkable,Vector3 _worldpos,int _gridX,int _gridY) {
        this.walkable = _walkable;
        this.worldposition = _worldpos;
        this.gridX = _gridX;
        this.gridY = _gridY;

    }
    public int F_Cost
    {
        get
        {
            return G_Cost + H_Cost;
        }
    }


}
