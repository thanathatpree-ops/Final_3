using UnityEngine;
using UnityEngine.Splines;

public class CoasterSeat : MonoBehaviour
{
    public Transform seatPoint;
    public Transform headTarget;          // <-- ใส่ HeadTarget ตรงนี้
    public Transform exitPoint;
    public SplineAnimate splineAnimate;

    public Transform cameraRoot;          // TrackingSpace

    public Vector3 rideLocalEuler = new Vector3(0,180,0); // หันหน้ารถ
    public bool autoCenter = true;        
    public bool lockY = true;             // ล็อกสูงต่ำไว้ไม่ให้ดีด

    private bool riding;
    private Transform originalParent;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    public Transform hmdCamera;           // CenterEyeAnchor

    public void Sit()
    {
        if (riding) return;
        riding = true;

        // save state
        originalParent = cameraRoot.parent;
        originalLocalPos = cameraRoot.localPosition;
        originalLocalRot = cameraRoot.localRotation;

        // parent to seat
        cameraRoot.SetParent(seatPoint, false);
        cameraRoot.localRotation = Quaternion.Euler(rideLocalEuler);

        if (autoCenter && headTarget != null && hmdCamera != null)
        {
            // คำนวณระยะหัวจริงเทียบกับจุดหัวที่ต้องการ (ใน space ของ seat)
            Vector3 hmdLocal = seatPoint.InverseTransformPoint(hmdCamera.position);
            Vector3 targetLocal = seatPoint.InverseTransformPoint(headTarget.position);

            Vector3 delta = targetLocal - hmdLocal;

            if (lockY) delta.y = 0; // อย่าให้สูงต่ำดีด

            // ขยับ TrackingSpace ชดเชยให้หัวไปอยู่ตรง HeadTarget พอดี
            cameraRoot.localPosition += delta;
        }

        if (splineAnimate != null)
            splineAnimate.Restart(true);
    }

    public void Exit()
    {
        if (!riding) return;
        riding = false;

        cameraRoot.SetParent(originalParent, false);
        cameraRoot.localPosition = originalLocalPos;
        cameraRoot.localRotation = originalLocalRot;

        if (exitPoint != null)
        {
            cameraRoot.position = exitPoint.position;
            cameraRoot.rotation = exitPoint.rotation;
        }
    }
}
