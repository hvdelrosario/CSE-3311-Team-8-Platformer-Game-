using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Use lateupdate for camera
    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
