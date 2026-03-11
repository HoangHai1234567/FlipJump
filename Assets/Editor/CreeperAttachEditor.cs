using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreeperAttach))]
public class CreeperAttachEditor : Editor
{
    private void OnSceneGUI()
    {
        CreeperAttach creeper = (CreeperAttach)target;
        if (creeper.pivot == null || creeper.attachPoint == null) return;

        Vector3 pivotPos = creeper.pivot.position;
        Vector3 offset = creeper.attachPoint.localPosition - creeper.pivot.localPosition;

        // Handle A (green) — position of AttachPoint at angleA
        Vector3 posA = pivotPos + (Vector3)(Quaternion.Euler(0, 0, creeper.angleA) * offset);
        Handles.color = Color.green;
        float handleSize = HandleUtility.GetHandleSize(posA) * 0.15f;
        Vector3 newPosA = Handles.FreeMoveHandle(posA, handleSize, Vector3.zero, Handles.SphereHandleCap);
        if (newPosA != posA)
        {
            Undo.RecordObject(creeper, "Move Point A");
            Vector3 dir = newPosA - pivotPos;
            float rawAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float baseAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            creeper.angleA = rawAngle - baseAngle;
            EditorUtility.SetDirty(creeper);
        }

        // Handle B (red) — position of AttachPoint at angleB
        Vector3 posB = pivotPos + (Vector3)(Quaternion.Euler(0, 0, creeper.angleB) * offset);
        Handles.color = Color.red;
        handleSize = HandleUtility.GetHandleSize(posB) * 0.15f;
        Vector3 newPosB = Handles.FreeMoveHandle(posB, handleSize, Vector3.zero, Handles.SphereHandleCap);
        if (newPosB != posB)
        {
            Undo.RecordObject(creeper, "Move Point B");
            Vector3 dir = newPosB - pivotPos;
            float rawAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float baseAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            creeper.angleB = rawAngle - baseAngle;
            EditorUtility.SetDirty(creeper);
        }

        // Labels
        Handles.color = Color.green;
        Handles.Label(posA + Vector3.up * 0.2f, "A");
        Handles.color = Color.red;
        Handles.Label(posB + Vector3.up * 0.2f, "B");
    }
}
