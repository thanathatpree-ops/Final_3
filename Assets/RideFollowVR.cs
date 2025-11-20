using UnityEngine;

public class RideFollowVR : MonoBehaviour
{
    public Transform seatPoint;
    public GameObject playerRoot;
    public Transform yawOffset;
    public Transform hmdCamera;

    public bool yawOnly = true;

    private bool riding = false;
    private Vector3 rootLocalOffset;

    void OnEnable() => Application.onBeforeRender += BeforeRender;
    void OnDisable() => Application.onBeforeRender -= BeforeRender;

    public void BeginRide()
    {
        riding = true;
        if (seatPoint == null || playerRoot == null || hmdCamera == null) return;

        Vector3 headLocal = seatPoint.InverseTransformPoint(hmdCamera.position);
        Vector3 rootLocal = seatPoint.InverseTransformPoint(playerRoot.transform.position);
        rootLocalOffset = rootLocal - headLocal;
    }

    public void EndRide(Transform exitPoint)
    {
        riding = false;
        if (yawOffset != null) yawOffset.localRotation = Quaternion.identity;

        if (exitPoint != null)
        {
            playerRoot.transform.position = exitPoint.position;
            playerRoot.transform.rotation = exitPoint.rotation;
        }
    }

    void LateUpdate() => FollowSeat();
    void BeforeRender() => FollowSeat();

    void FollowSeat()
    {
        if (!riding || seatPoint == null || playerRoot == null) return;

        playerRoot.transform.position = seatPoint.TransformPoint(rootLocalOffset);

        if (yawOffset != null && hmdCamera != null)
        {
            float headYaw = hmdCamera.eulerAngles.y;
            float seatYaw = seatPoint.eulerAngles.y;
            yawOffset.localRotation = Quaternion.Euler(0, seatYaw - headYaw, 0);
        }

        if (yawOnly)
        {
            Vector3 fwd = seatPoint.forward; fwd.y = 0;
            if (fwd.sqrMagnitude > 0.001f)
                playerRoot.transform.rotation = Quaternion.LookRotation(fwd);
        }
        else playerRoot.transform.rotation = seatPoint.rotation;
    }
}
