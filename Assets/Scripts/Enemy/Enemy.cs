using UnityEngine;
public enum Enemies
{
    ZOMBIE, TEST
}
public enum Actions
{
    WALKLEFT, WALKRIGHT, JUMP, RUNLEFT, RUNRIGHT
}
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Enemies enemyType;
    public Actions action;
    private GameObject fallDetectionLeft;
    private GameObject fallDetectionRight;
    public LayerMask layers;
    
    void Start()
    {
        fallDetectionLeft = transform.GetChild(0).gameObject;
        fallDetectionRight = transform.GetChild(1).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
        action = Actions.WALKLEFT;

    }

    void Update()
    {
        if(foundLedge(fallDetectionLeft) || foundLedge(fallDetectionRight))
        {
            //Only switch direction if there is a platform to go back to
            //Note that the sprite is flipped depending on walk direction, so foundledge is called on left side both cases
            if(!foundLedge(fallDetectionLeft))
            {
                if(action == Actions.WALKLEFT)
                {
                    action = Actions.WALKRIGHT;
                }
                else if(action == Actions.WALKRIGHT)
                {
                    action = Actions.WALKLEFT;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if(action == Actions.WALKLEFT)
        {
            rigid.AddForce(new Vector2(-15, 0));
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(action == Actions.WALKRIGHT)
        {
            rigid.AddForce(new Vector2(15, 0));
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public bool foundLedge(GameObject foot)
    {
        return !Physics2D.Raycast(foot.transform.position, -foot.transform.up, 0.1f, layers);
    }
}
