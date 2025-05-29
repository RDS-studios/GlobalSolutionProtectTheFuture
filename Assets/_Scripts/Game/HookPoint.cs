using UnityEngine;

public class HookPoint : MonoBehaviour
{
    public bool isAimed = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAimed)
        {
            GetComponent<SpriteRenderer>().color = Color.red; // Change color to red when aimed 
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white; // Change color back to white when not aimed
        }
    }
}
