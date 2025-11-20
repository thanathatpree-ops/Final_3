using UnityEngine;

public class RidePivotFollower : MonoBehaviour
{
    public Transform seatPoint;   // seatpointCoaster
    public Transform ridePivot;   // RidePivot
    public bool yawOnly = true;

    void LateUpdate()
    {
        if (seatPoint == null || ridePivot == null) return;

        // ตามตำแหน่งเบาะ 1:1
        ridePivot.position = seatPoint.position;

        // หมุนเฉพาะ Yaw (กันตีลังกาพากล้องหลุด)
        if (yawOnly)
        {
            Vector3 fwd = seatPoint.forward;
            fwd.y = 0;
            if (fwd.sqrMagnitude > 0.001f)
                ridePivot.rotation = Quaternion.LookRotation(fwd);
        }
        else
        {
            ridePivot.rotation = seatPoint.rotation;
        }
    }
}
