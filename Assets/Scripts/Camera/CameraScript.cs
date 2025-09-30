using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Actions action = Actions.ZOOMOUT;
    public enum Actions
    {
        ZOOMIN, ZOOMOUT, NOTHING
    }
    public GameObject player;
    void Start()
    {
        Camera.main.orthographicSize = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(action == Actions.ZOOMIN)
        {
            zoomIn(4.9f);
        }
        else if(action == Actions.ZOOMOUT)
        {
            zoomOut(5);
        }
    }

    //Use lateupdate for camera
    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -5);
    }

    public void setMode(Actions action)
    {
        this.action = action;
    }


    //Note to only zoom in if not in a cutscene or something similar
    void zoomIn(float size)
    {
        if(Camera.main.orthographicSize > size)
        {
            Camera.main.orthographicSize -= 2 * Time.deltaTime;
        }
    }

    void zoomOut(float size)
    {
        if(Camera.main.orthographicSize < size)
        {
            Camera.main.orthographicSize += 2 * Time.deltaTime;
        }
    }
}
