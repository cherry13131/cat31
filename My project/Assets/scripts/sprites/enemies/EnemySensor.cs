using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    private EnemyAI parentAI;

    void Awake()
    {
        parentAI = GetComponentInParent<EnemyAI>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지!");
            parentAI.SetTarget(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 놓침");
            parentAI.RemoveTarget();
        }
    }
}