using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    [SerializeField] BossHand leftHandScript;
    [SerializeField] BossHand rightHandScript;

    [SerializeField] GameObject leftHandObj;
    [SerializeField] GameObject rightHandObj;


    [SerializeField] Transform playerTransform;

    [SerializeField] GameObject selectedHand;

    [SerializeField] int firstFaseMaxHits;
    [SerializeField] int firstFaseHits;
    [SerializeField] int secondFaseMaxHits;
    [SerializeField] int secondFaseHits;

    public bool hasBothHands = true;
     



    public int fase = 0;
    public bool isAttacking = false;

    public int bossHP = 3;
    public bool isVulnerable = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(BossControl());
        firstFaseMaxHits = Random.Range(1,7);
        secondFaseMaxHits = Random.Range(1, 4);
    }


    void Update()
    {



    }

    IEnumerator BossControl()
    {
        if (fase == 1 && !isAttacking && firstFaseHits<firstFaseMaxHits)
        {
            int randomHand = Random.Range(0, 2);

            if (randomHand == 0)
            {
                yield return StartCoroutine(leftHandScript.ChangeFollowRoutine());
            }
            else
            {
                yield return StartCoroutine(rightHandScript.ChangeFollowRoutine());
            }
            firstFaseHits++;
            if(firstFaseHits == firstFaseMaxHits)
            {
                fase = 2; // Change to fase 2 when max hits are reached
                ChangeFase();   
            }
        }
        else if (fase == 2 && !isAttacking)
        {
                Coroutine left = StartCoroutine(leftHandScript.SmashMiddleRoutine());
                Coroutine right = StartCoroutine(rightHandScript.SmashMiddleRoutine());

                // Wait until both finish
                yield return left;
                yield return right;
            secondFaseHits++;   
            if (secondFaseHits == secondFaseMaxHits)
            {
                fase = 3; // Change to fase 3 when max hits are reached
                ChangeFase();
            }
        }
        else if (fase == 3)
        {
            if (hasBothHands) { 
            int randomHand = Random.Range(0, 2);

            if (randomHand == 0)
            {
                yield return StartCoroutine(leftHandScript.ChangeFollowRoutine());
            }
            else
            {
                yield return StartCoroutine(rightHandScript.ChangeFollowRoutine());
            }
            }
        }
        yield return new WaitForSeconds(Random.Range(3.5f,5.6f));
        
       
            StartCoroutine(BossControl());
            print("BossControl Coroutine Started");    

    }

    void ChangeFase()
    {
        print("Changing fase");
        if (fase == 2)
        {
            print("Entrando na fase 2");
        }
        if (fase == 3)
        {
            leftHandScript.Burn();
            rightHandScript.Burn(); 
            print("Entrando na fase 3");
        }
    }

    
}