using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지!");

            transform.root.SendMessage(
                "SetTarget",
                other.transform,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 놓침");

            transform.root.SendMessage(
                "ClearTarget",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }
}