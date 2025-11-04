using UnityEngine;

public class Gate : MonoBehaviour
{
    private bool activated;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void activateGate()
    {
        if(!activated)
        {
            anim.Play("GateOpen");
        }
        activated = true;
    }
}
