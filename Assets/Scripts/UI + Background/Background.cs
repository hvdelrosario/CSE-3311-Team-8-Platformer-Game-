using UnityEngine;

public class Background : MonoBehaviour
{
    private float initialX;
    private float finalX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialX = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Backgrounds should slightly follow the camera to a fraction of the distance away
        finalX = initialX + (Camera.main.transform.position.x * 0.75f);
        transform.position = new Vector3(finalX, Camera.main.transform.position.y, 0);
        //1920 * 2 screens away, need to shift 1920 * 3 screens away hwoever
        if((transform.position.x - Camera.main.transform.position.x) > 38.40f)
        {
            initialX -= 57.60f;
        }
        else if((transform.position.x - Camera.main.transform.position.x) < -38.40f)
        {
            initialX += 57.60f;
        }
    }
}
