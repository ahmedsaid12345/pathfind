using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unWalkableMask;
    public float NodeRaduis;
    public Vector2 grideWorldSize;
    Node[,] grid;

    float Nodediameter;
    int grideSizeX;
     int   grideSizeY;
    private void Start()
    {
        Nodediameter = NodeRaduis * 2;
        grideSizeX = Mathf.RoundToInt( grideWorldSize.x / Nodediameter);
        grideSizeY = Mathf.RoundToInt(grideWorldSize.y/Nodediameter);
        CreateGrid();

    }
    void CreateGrid() {

        grid = new Node[grideSizeX,grideSizeY];
        Vector3 worldbottomleft =transform.position-(Vector3.right*grideSizeX/2)-(Vector3.forward*grideSizeY/2) ;
       // Debug.Log("gri");
        for (int x=0;x<grideSizeX;x++)
        {
            for (int y=0;y<grideSizeY;y++)
            {
                Vector3 WorldPoint = worldbottomleft + Vector3.right * (x * Nodediameter + NodeRaduis) + Vector3.forward * (y * Nodediameter+ NodeRaduis);
                bool walkable = !(Physics.CheckSphere(WorldPoint,NodeRaduis,unWalkableMask));
                grid[x, y] = new Node(walkable,WorldPoint,x,y);
            }
        }



    }


    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x=-1;x<=1;x++) {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX+x;
                int checkY = node.gridY +y;
                if (checkX>=0&&checkX<grideSizeX && checkY>=0 && checkY<grideSizeY)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// //// take player position and back node where player stand
    /// </summary>
    public Node NodeFromWorldPoint(Vector3 playerpos)
    {
        float percentX =(playerpos.x+grideWorldSize.x/2)/grideWorldSize.x;
        float percentY = (playerpos.z + grideWorldSize.y / 2) /grideWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x =Mathf.RoundToInt((grideSizeX-1)*percentX) ;
        int y = Mathf.RoundToInt((grideSizeY - 1) * percentY);
        return grid[x,y];
    }


    public List<Node> path = new List<Node>();
    //visualize result
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(grideWorldSize.x,1,grideWorldSize.y));
       
        if (grid !=null) {
            Node PlayerNode = NodeFromWorldPoint(player.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
               if (PlayerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;

                Gizmos.DrawCube(n.worldposition,Vector3.one* (Nodediameter-.1f));
            }
        }
    }

}
