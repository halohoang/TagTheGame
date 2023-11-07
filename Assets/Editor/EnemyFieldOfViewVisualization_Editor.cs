using StateMashine;
using UnityEditor;
using UnityEngine;

// see YT-Tuttorial: https://youtu.be/j1-OyLo77ss?t=1095 by 'Comp-3 Interaction'

[CustomEditor(typeof(ConditionPlayerDetectionCheck))]
public class EnemyFieldOfViewVisualization_Editor : Editor
{
    private void OnSceneGUI()
    {       
        ConditionPlayerDetectionCheck condPlayerDetection = (ConditionPlayerDetectionCheck)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(condPlayerDetection.transform.position, -Vector3.forward, Vector3.right, 360.0f, condPlayerDetection.FOVRadius);

        Vector3 viewAngle01 = DirectionFromAngle(condPlayerDetection.transform.eulerAngles.z, condPlayerDetection.FOVAngle * 0.5f);
        Vector3 viewAngle02 = DirectionFromAngle(condPlayerDetection.transform.eulerAngles.z, -condPlayerDetection.FOVAngle * 0.5f);

        Handles.color = Color.yellow;
        Handles.DrawLine(condPlayerDetection.transform.position, condPlayerDetection.transform.position + viewAngle01 * condPlayerDetection.FOVRadius);
        Handles.DrawLine(condPlayerDetection.transform.position, condPlayerDetection.transform.position + viewAngle02 * condPlayerDetection.FOVRadius);

        if (condPlayerDetection.IsPlayerDetected)
        {
            Handles.color = Color.green;
            Handles.DrawLine(condPlayerDetection.transform.position, condPlayerDetection.PlayerObj.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f);
    }
}
