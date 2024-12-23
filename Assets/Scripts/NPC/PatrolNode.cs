using UnityEngine;

namespace NPC
{
    public class PatrolNode : MonoBehaviour
    {
        public float waitAtNode = 1;
        public PatrolRoute route;
        public GameObject PatrolNodePrefab;

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere( transform.position,0.1f);
        }
    
        /// <summary>
        /// Configure the patrol node on instantiation in the PatrolNodeEditor script
        /// </summary>
        /// <param name="prevNode"></param>
        public void ConfigureNode(PatrolNode node)
        {
            route = node.route;
            transform.SetParent(route.transform);
            route.Initialize();
        }

        public void DeleteNode()
        {
            route.DeleteNode(this);
        }
    }
}