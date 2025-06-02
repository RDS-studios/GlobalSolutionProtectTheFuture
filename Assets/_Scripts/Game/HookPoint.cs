using UnityEngine;

public class HookPoint : MonoBehaviour
{
    public bool isAimed = false;
    [SerializeField] GameObject showTargeted;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAimed)
        {
           showTargeted.SetActive(true); // Show the targeted object when aimed
        }
        else
        {
          showTargeted.SetActive(false); // Hide the targeted object when not aimed
        }
    }
}
