using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class PatrolRoute : MonoBehaviour
    {
        [SerializeField] private List<PatrolNode> nodes = new ();
        [SerializeField] private bool circularPath;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            Debug.LogWarning($"INITIALIZE CALLED");
            nodes.Clear();
            foreach (Transform child in transform)
            {
                nodes.Add(child.GetComponent<PatrolNode>());
            }
        }

        public int NodeCount()
        {
            return nodes.Count;
        }

        public PatrolNode GetNextNode(PatrolNode currentNode, ref bool isGoingBack)
        {
        
            //RETURN FIRSRT NODE IF THERE IS NO CURRENT NODE
            if (currentNode == null && nodes.Count >= 1)
            {
                Debug.LogWarning($"RETURNING: {0}");
                return nodes[0];
            }
        
            //RETURN NO NODE IF THERE'S ONLY ONE
            if (nodes.Count == 0 || nodes.Count == 1)
            {
                Debug.LogWarning($"RETURNING: null nodes");
                return null;
            }
        
            int indexOfCurrentNode = 0;
        
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == currentNode)
                {
                    indexOfCurrentNode = i;
                    break;
                }
            }

            if (indexOfCurrentNode == 0)
            {
                isGoingBack = false;
                Debug.LogWarning($"RETURNING: {1}");
                return nodes[1];
            }

            if (indexOfCurrentNode == nodes.Count - 1 && !circularPath)
            {
                isGoingBack = true;
            }
        
            if (isGoingBack)
            {
                Debug.LogWarning($"RETURNING: {indexOfCurrentNode - 1}");
                return nodes[indexOfCurrentNode - 1];
            }
            Debug.LogWarning($"RETURNING: {(indexOfCurrentNode == (nodes.Count - 1) ? 0 : indexOfCurrentNode + 1)}");
            return indexOfCurrentNode == nodes.Count - 1 ? nodes[0] : nodes[indexOfCurrentNode + 1];
        }

        public void DeleteNode(PatrolNode nodeToDelete)
        {
            if (nodes.Count == 1) return;
            nodes.Remove(nodeToDelete);
            Initialize();
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == null)
                {
                    Initialize();
                    break;
                }

                if (i < nodes.Count - 1)
                {
                    if (nodes[i + 1] == null)
                    {
                        Initialize();
                        break;
                    }
                }
            
            
                if (i < nodes.Count - 1)
                {
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
                else if (circularPath)
                {
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[0].transform.position);
                }
            }
        }
    }
}
