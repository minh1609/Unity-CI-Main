using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamTrigger : MonoBehaviour
{
    public GameObject triggerCam;
    public Room currentRoom;
    public Collider2D boundary;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        triggerCam.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            currentRoom.camTrigger = true;
            triggerCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            currentRoom.camTrigger = false;
            triggerCam.SetActive(false);
        }
    }
}
