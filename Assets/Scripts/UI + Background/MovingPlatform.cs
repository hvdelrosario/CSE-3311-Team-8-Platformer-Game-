using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;
    public GameObject path;
    public int currentNode;
    private int totalNodes;
    public float x;
    public float percentageCompleted;
    public bool activatable;
    private bool activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalNodes = path.transform.childCount;
        percentageCompleted = 0f;
        x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        int nextNode = (currentNode + 1) % totalNodes;
        startPosition = path.transform.GetChild(currentNode).transform.position;
        endPosition = path.transform.GetChild(nextNode).transform.position;
        if(activated || !activatable)
        {
            x += speed * Time.deltaTime;
            //3x^2 - 2x^3 for curve easein/easeout
            //0.001f for the floating point weird stuff
            percentageCompleted = 3 * x * x - 2 * x * x * x + 0.001f;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageCompleted);
            if(percentageCompleted >= 1f)
            {
                x = 0f;
                currentNode = nextNode;
                nextNode = (currentNode + 1) % totalNodes;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(activatable)
        {
            activated = true;
        }
    }
}
