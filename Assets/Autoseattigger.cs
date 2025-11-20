using UnityEngine;

public class AutoSeatTrigger : MonoBehaviour
{
    public CoasterSeat seat;
    public bool startOnce = true;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (startOnce && used) return;

        // หา CoasterSeat/PlayerRoot จาก parent ขึ้นไป
        // ถ้าชนด้วย collider ลูก ก็ยังหาเจอ
        var player = other.GetComponentInParent<CharacterController>();
        var rigRoot = other.GetComponentInParent<Transform>();

        Debug.Log("Trigger hit by: " + other.name);

        // ถ้าชนด้วย CharacterController หรือชนด้วย object ที่เป็นผู้เล่น
        if (player != null || other.CompareTag("Player"))
        {
            used = true;
            Debug.Log("Auto Sit!");
            seat.Sit();
        }
    }
}






