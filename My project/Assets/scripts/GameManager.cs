using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject movePrompt;
    public GameObject interactPrompt;
    public GameObject guest;
    public GameObject door;

    private bool isNearDoor = false;
    private bool doorOpened = false;

    void Start()
    {
        player.canMove = false;
        movePrompt.SetActive(false);
        interactPrompt.SetActive(false);
        guest.SetActive(false);

        Invoke("EnablePlayer", 2f); // �Ͼ �� �����̱� ���
    }

    void EnablePlayer()
    {
        player.canMove = true;
        movePrompt.SetActive(true);
    }

    void Update()
    {
        if (isNearDoor && !doorOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        doorOpened = true;
        interactPrompt.SetActive(false);
        guest.SetActive(true);
        Debug.Log("�� ����, �մ� ����");
    }

    public void SetNearDoor(bool near)
    {
        isNearDoor = near;
        interactPrompt.SetActive(near && !doorOpened);
    }
}
