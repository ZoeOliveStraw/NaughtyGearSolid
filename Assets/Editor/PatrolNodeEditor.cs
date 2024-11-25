using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PatrolNode))]
public class PatrolNodeEditor : Editor
{
    private PatrolNode _targetNode;
    protected void OnSceneGUI()
    {
        _targetNode = (PatrolNode) target;
        Vector3 spawnPosition = _targetNode.transform.position + new Vector3(0.5f,0.5f,0);
        Vector3 deletePosition = _targetNode.transform.position + new Vector3(-0.5f,0.5f,0);
        float size = 0.3f;
        float pickSize = size * 2f;
    
        if (Handles.Button(spawnPosition, Quaternion.identity, size, pickSize, Handles.CubeHandleCap))
        {
            SpawnNewNode();
        }
        
        if (Handles.Button(deletePosition, Quaternion.identity, size, pickSize, Handles.SphereHandleCap))
        {
            DeleteNode();
        }
    }
    
    private void SpawnNewNode()
    {
        PatrolNode newNode = Instantiate(_targetNode.PatrolNodePrefab, _targetNode.transform.position, Quaternion.identity).GetComponent<PatrolNode>();
        newNode.ConfigureNode(_targetNode); 
        Selection.activeGameObject = newNode.gameObject;
    }
    
    private void DeleteNode()
    {
        _targetNode.DeleteNode();
        DestroyImmediate(_targetNode.gameObject);
    }
}