using System.Collections.Generic;
using UnityEngine;

public static class VisionConeGenerator
{
    public static Mesh Generate(float sightHorizontalAngle, float sightHeight, float sightDistance, int stepCount)
    {
        Mesh visionMesh = new Mesh();
        
        List<Vector3> viewPoints = new List<Vector3>();
        List<int> tris = new List<int>();
        
        //TODO: Generate points at origin
        Vector3 originTop = new Vector3(0, sightHeight / 2, 0);
        viewPoints.Add(originTop);
        Vector3 originBottom = new Vector3(0, -(sightHeight / 2), 0);
        viewPoints.Add(originBottom);

        List<float> steps = VisionConeAngles(sightHorizontalAngle, stepCount);
        for (int i = 0; i < stepCount; i++)
        {
            //Generate the necessary point
            Vector3 topPoint = GetRotatedPoint(steps[i], sightHeight / 2, sightDistance);
            int topPointIndex = AddV3ToList(topPoint, viewPoints);
            Vector3 bottomPoint = GetRotatedPoint(steps[i], -(sightHeight / 2), sightDistance);
            int bottomPointIndex = AddV3ToList(bottomPoint, viewPoints);

            if (i == 0 || i == steps.Count - 1)
            {
                tris.AddRange(new int[] {0, 1, topPointIndex});
                tris.AddRange(new int[] {1, bottomPointIndex, topPointIndex});
            }

            if (i != 0)
            {
                tris.AddRange(new int[] {0, topPointIndex, topPointIndex - 2});
                tris.AddRange(new int[] {1, bottomPointIndex, bottomPointIndex - 2});
            }
        }
        
        //Set Mesh points and tris
        Vector3[] vertices = viewPoints.ToArray();
        visionMesh.vertices = vertices;
        visionMesh.triangles = tris.ToArray();
        
        //return the finished mesh :)
        visionMesh.RecalculateNormals();
        return visionMesh;
    }
    
    private static Vector3 GetRotatedPoint(float angleInDegrees, float verticalOffset, float sightDistance)
    {
        Vector3 forwardDirection = Vector3.forward;
        Vector3 rotatedDirection = Quaternion.Euler(0, angleInDegrees, 0) * forwardDirection;
        Vector3 targetPoint = rotatedDirection.normalized * sightDistance;
        targetPoint = new Vector3(targetPoint.x, targetPoint.y + verticalOffset, targetPoint.z);

        return targetPoint;
    }

    //RETURNS INDEX IN ARRAY
    private static int AddV3ToList(Vector3 v3, List<Vector3> list)
    {
        list.Add(v3);
        return list.Count - 1;
    }

    private static List<float> VisionConeAngles(float totalAngle, int numberOfSteps)
    {
        List<float> anglePoints = new List<float>();
        float halfAngle = totalAngle / 2f;
        float stepSize = totalAngle / (numberOfSteps - 1);
        
        for (int i = 0; i < numberOfSteps; i++)
        {
            float angle = -halfAngle + (i * stepSize);
            anglePoints.Add(angle);
        }
        return anglePoints;
    }
}
