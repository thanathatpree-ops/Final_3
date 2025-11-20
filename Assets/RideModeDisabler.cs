using System.Collections.Generic;
using UnityEngine;

public class RideModeDisabler : MonoBehaviour
{
    // เก็บ Behaviour ที่เราปิดไว้
    private List<Behaviour> disabledBehaviours = new List<Behaviour>();

    // เก็บ CharacterController แยก (เพราะไม่ใช่ Behaviour)
    private CharacterController disabledCC;

    public void EnterRideMode()
    {
        disabledBehaviours.Clear();
        disabledCC = null;

        // ไล่ทุก Behaviour ใต้ PlayerRoot
        var behaviours = GetComponentsInChildren<Behaviour>(true);
        foreach (var b in behaviours)
        {
            if (b == null) continue;
            if (!b.enabled) continue;

            string t = b.GetType().Name;

            // ----- พวกที่ไม่ควรปิด (tracking/กล้อง) -----
            if (t.Contains("OVRCameraRig") || t.Contains("TrackedPoseDriver") || t.Contains("PoseDriver"))
                continue;
            if (t.Contains("Camera") || t.Contains("AudioListener"))
                continue;

            // ----- พวกที่ควรปิด (มักขยับ/รีเซ็ตตัวผู้เล่น) -----
            bool shouldDisable =
                t.Contains("MoveProvider") ||
                t.Contains("TurnProvider") ||
                t.Contains("Teleport") ||
                t.Contains("Locomotion") ||
                t.Contains("ControllerDriver") ||
                t.Contains("Recenter") ||
                t.Contains("Ground") ||
                t.Contains("Snap") ||
                t.Contains("NavMesh");

            string n = b.name.ToLower();
            shouldDisable |= n.Contains("locomotion") || n.Contains("teleport") || n.Contains("move") || n.Contains("turn");

            if (shouldDisable)
            {
                b.enabled = false;
                disabledBehaviours.Add(b);
            }
        }

        // ปิด CharacterController แยก
        var cc = GetComponentInChildren<CharacterController>();
        if (cc != null && cc.enabled)
        {
            cc.enabled = false;
            disabledCC = cc;
        }
    }

    public void ExitRideMode()
    {
        // เปิด Behaviour คืน
        foreach (var b in disabledBehaviours)
        {
            if (b != null) b.enabled = true;
        }
        disabledBehaviours.Clear();

        // เปิด CharacterController คืน
        if (disabledCC != null)
        {
            disabledCC.enabled = true;
            disabledCC = null;
        }
    }
}
