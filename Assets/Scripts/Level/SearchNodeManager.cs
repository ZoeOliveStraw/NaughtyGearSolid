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
            return GetPositionWithoutCollision(myPos, maxRange);
        }
        int randIndex = Random.Range(0, points.Count);
        return points[randIndex];
    }

    private List<Vector3> GetPointsInRange(Vector3 origin, float range)
    {
        List<Vector3> point = SearchPoints
            .Where(n => Vector3.Distance(origin, n.GetMyPosition()) <= range)
            .Select(n => n.GetMyPosition())
            .ToList();
        return point;
    }
    
    public static Vector3 GetPositionWithoutCollision(Vector3 origin, float radius, float checkRadius = 0.5f, int maxAttempts = 100)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0;
            Vector3 candidatePosition = origin + randomOffset;
            
            if (!Physics.CheckSphere(candidatePosition, checkRadius))
            {
                return candidatePosition;
            }
        }
        Debug.LogWarning("No valid position found after max attempts.");
        return origin;
    }
}
