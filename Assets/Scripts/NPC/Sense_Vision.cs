using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Sense_Vision : MonoBehaviour
{
    
    //Serialized fields
    [Header("Component References")] 
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private Transform eyePosition;
    
    [Header("Vision cone dimensions")]
    [SerializeField] private float sightDistance;
    [SerializeField][Range(0,360)] private float sightHorizontalAngle;
    [SerializeField] private float sightHeight;
    [SerializeField] private int meshResolution = 10;

    [Header("Vision State Materials")] 
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material alertMaterial;
    
    [Header("Tracking information")]
    [SerializeField] private List<string> trackedTags = new();

    private List<Transform> _objectsInFOV = new();
    private List<Transform> _seenObjects = new();

    private void Start()
    {
        GenerateViewCone();
    }

    private void FixedUpdate()
    {
        if(_objectsInFOV.Count > 0) RaycastToTrackedObjects();
    }

    private void GenerateViewCone()
    {
        meshFilter.mesh = VisionConeGenerator.Generate(sightHorizontalAngle, sightHeight, sightDistance, meshResolution);
        meshCollider.sharedMesh = meshFilter.mesh;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (trackedTags.Contains(col.gameObject.tag))
        {
            Debug.LogWarning($"Adding {col.gameObject.name} to tracked list");
            _objectsInFOV.Add(col.transform);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (trackedTags.Contains(col.gameObject.tag))
        {
            Debug.LogWarning($"Removing {col.gameObject.name} from tracked list");
            _objectsInFOV.Remove(col.transform);

            if (_seenObjects.Contains(col.transform))
            {
                _seenObjects.Remove(col.transform);
            }
        }
    }

    private void RaycastToTrackedObjects()
    {
        foreach (Transform t in _objectsInFOV)
        {
            CheckIfObjectSeen(t);
        }
    }

    private void CheckIfObjectSeen(Transform t)
    {
        //SEND OUT A RAYCAST AND PUT THE HIT TO hit
        if (Physics.Raycast(transform.position, t.position - transform.position, out RaycastHit hit))
        {
            Debug.LogWarning($"HIT TAG: {hit.transform.tag}");
            
            if (hit.transform == t && !_seenObjects.Contains(t))
            {
                _seenObjects.Add(t);
                Debug.LogWarning($"ADDING {t.name} TO SEEN LIST, COUNT: {_seenObjects.Count}");
            }
            else if (hit.transform != t && _seenObjects.Contains(t))
            {
                _seenObjects.Remove(t);
                Debug.LogWarning($"REMOVING {t.name} FROM SEEN LIST, COUNT: {_seenObjects.Count}");
            }
        }
        else
        {
            _seenObjects.Remove(t);
            Debug.LogWarning($"REMOVING {t.name} FROM SEEN LIST, COUNT: {_seenObjects.Count}");
        }
    }

    public bool CanSeeObjectWithTag(string tag)
    {
        foreach (Transform t in _seenObjects)
        {
            if (t.CompareTag(tag)) return true;
        }
        return false;
    }
    
    public bool CanSeeObject(Transform obj)
    {
        foreach (Transform t in _seenObjects)
        {
            if (t == obj) return true;
        }
        return false;
    }

    public void SetVisionConeColor(Color c)
    {
        meshRenderer.material.color = c;
    }
}