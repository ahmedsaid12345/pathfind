using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    public  Transform Seeker;
    public Transform Target;



    public void FindPath(Vector3 startpos,Vector3 targetpos)
    {
        Node StartNode = grid.NodeFromWorldPoint(startpos);
        Node TargetNode = grid.NodeFromWorldPoint(targetpos);
        List<Node> OpenSet = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();
        OpenSet.Add(StartNode);
        while (OpenSet.Count > 0)
        {
            Node CurrentNode=OpenSet[0];
            for (int i=1;i<OpenSet.Count;i++)
            {
                if (CurrentNode.F_Cost>OpenSet[i].F_Cost || (CurrentNode.F_Cost == OpenSet[i].F_Cost && CurrentNode.H_Cost > OpenSet[i].H_Cost))
                {
                    CurrentNode = OpenSet[i];
                }
            }
            OpenSet.Remove(CurrentNode);
            ClosedSet.Add(CurrentNode);
            if (CurrentNode == TargetNode)
            {
                TracePath(StartNode,TargetNode);
                return;
            }
            foreach( Node neighbour in grid.GetNeighbours(CurrentNode))
            {
                if (!(neighbour.walkable) || ClosedSet.Contains(neighbour)) continue;
                int NewMovementcosttoNeighbour = CurrentNode.G_Cost + GetDistance(CurrentNode, neighbour);
                if (NewMovementcosttoNeighbour < neighbour.G_Cost || !OpenSet.Contains(neighbour))
                {
                    neighbour.G_Cost =NewMovementcosttoNeighbour;
                    neighbour.H_Cost = GetDistance(neighbour,TargetNode);
                    neighbour.Parent = CurrentNode;
                    if (!OpenSet.Contains(neighbour))
                    {
                        OpenSet.Add(neighbour);
                    }

                }
            }
        }

    }

    void TracePath(Node StartNode,Node EndNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = EndNode;
        while (currentNode != StartNode)
        {
           path.Add(currentNode);
           currentNode= currentNode.Parent;
        }
        path.Reverse();
        grid.path = path;
    }
    private void Awake()
    {
        grid = this.GetComponent<Grid>();
    }

    int GetDistance(Node nodeA,Node nodeB)
    {
        int distX = Mathf.Abs( nodeA.gridX-nodeB.gridX) ;
        int distY = Mathf.Abs(nodeA.gridY-nodeB.gridY);
        if (distX > distY)
            return 14 * distY + (distX - distY) * 10;
        return 14 * distX + (distY-distX)*10;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(Seeker.position, Target.position);
         
    }
}
