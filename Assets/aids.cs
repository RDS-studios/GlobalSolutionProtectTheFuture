using SmallHedge.SoundManager;
using System.Collections;
using UnityEngine;

public class aids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(enumerator());
    }

    // Update is called once per frame
   IEnumerator enumerator()
    {
        yield return new WaitForSeconds(4f);
        SoundManager.PlaySound(SoundType.Gancho);
    }
}
