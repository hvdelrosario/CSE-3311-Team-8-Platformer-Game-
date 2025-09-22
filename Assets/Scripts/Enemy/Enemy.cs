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
    private GameObject fallDetection;
    public LayerMask layers;

    void Start()
    {
        fallDetection = transform.GetChild(0).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
        action = Actions.WALKLEFT;

    }

    void Update()
    {
        if(!Physics2D.Raycast(fallDetection.transform.position, -fallDetection.transform.up, 0.01f, layers))
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
        if(action == Actions.WALKLEFT)
        {
            rigid.AddForce(new Vector2(-1, 0));
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(action == Actions.WALKRIGHT)
        {
            rigid.AddForce(new Vector2(1, 0));
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
