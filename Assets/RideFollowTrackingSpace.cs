using UnityEngine;

public class RideFollowTrackingSpace : MonoBehaviour
{
    public Transform seatPoint;
    public Transform trackingSpace;
    public Transform hmdCamera;
    public bool yawOnly = true;

    private bool riding = false;
    private Vector3 tsLocalOffsetXZ;
    private float baseY;

    void OnEnable()  => Application.onBeforeRender += BeforeRender;
    void OnDisable() => Application.onBeforeRender -= BeforeRender;

    public void BeginRide()
    {
        riding = true;
        if (seatPoint == null || trackingSpace == null || hmdCamera == null) return;

        baseY = trackingSpace.position.y;

        Vector3 headLocal = seatPoint.InverseTransformPoint(hmdCamera.position);
        Vector3 tsLocal   = seatPoint.InverseTransformPoint(trackingSpace.position);

        headLocal.y = 0f;
        tsLocal.y   = 0f;

        tsLocalOffsetXZ = tsLocal - headLocal;
    }

    public void EndRide()
    {
        riding = false;
    }

    void LateUpdate()   => Follow();
    void BeforeRender() => Follow();

    void Follow()
    {
        if (!riding || seatPoint == null || trackingSpace == null) return;

        Vector3 targetPos = seatPoint.TransformPoint(tsLocalOffsetXZ);
        targetPos.y = baseY;                 // ล็อกความสูงกันดีด

        trackingSpace.position = targetPos;

        if (yawOnly)
        {
            Vector3 fwd = seatPoint.forward;
            fwd.y = 0;
            if (fwd.sqrMagnitude > 0.001f)
                trackingSpace.rotation = Quaternion.LookRotation(fwd);
        }
        else
        {
            trackingSpace.rotation = seatPoint.rotation;
        }
    }
}
