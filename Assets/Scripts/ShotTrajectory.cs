using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShotTrajectory : MonoBehaviour {


    LineRenderer lineRenderer;

    [SerializeField] private int accurency = 30;
    [SerializeField] private float simulationTime = 1f;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector3 startVector, Vector3 startVelocity)
    {
        Vector3[] positions = new Vector3[accurency];
        for(int i = 0; i < accurency; i++)
        {
            positions[i] = PlotTrajectoryAtTime(startVector, startVelocity, i *  (simulationTime/accurency));
        }
        lineRenderer.positionCount = accurency;
        lineRenderer.SetPositions(positions);
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    public void ClearTrajectoryLine()
    {
        lineRenderer.positionCount = 0;
    }
}
