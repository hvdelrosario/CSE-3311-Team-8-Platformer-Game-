using UnityEngine;

public class WindParent : MonoBehaviour
{
    public float maxTimer;
    public float windStrength;
    public bool windPermanent;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = maxTimer;
        transform.GetChild(0).gameObject.GetComponent<Wind>().windStrength = windStrength;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && !windPermanent)
        {
            timer = maxTimer;
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
        else if(windPermanent)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }


}
