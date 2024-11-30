using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SearchNodeManager : MonoBehaviour
{
    [SerializeField] private List<SearchNode> SearchPoints;
    private State_Abstract myState;
    
    public static SearchNodeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public Vector3 GetRandomNodeInRange(Vector3 myPos, float maxRange)
    {
        var points = GetPointsInRange(myPos, maxRange);
        if (points.Count <= 1)
        {
            return myPos;
        }
        int randIndex = Random.Range(0, points.Count);
        return points[randIndex];
    }

    private List<Vector3> GetPointsInRange(Vector3 origin, float range)
    {
        return SearchPoints
            .Where(n => Vector3.Distance(origin, n.GetMyPosition()) <= range)
            .Select(n => n.GetMyPosition())
            .ToList();
    }
}
