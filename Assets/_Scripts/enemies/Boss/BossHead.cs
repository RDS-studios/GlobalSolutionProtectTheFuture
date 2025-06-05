using SmallHedge.SoundManager;
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
    [SerializeField] Animator waterAnimator;



    [SerializeField] GameObject GlassSprite;

    public bool hasBothHands = true;
    public bool isLeftHandActive = true;
    public bool isRightHandActive = true;




    public int fase = 0;
    public bool isAttacking = false;

    public int bossHP = 3;
    public bool isVulnerable = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        firstFaseMaxHits = Random.Range(1, 7);
        secondFaseMaxHits = Random.Range(1, 4);



        StartCoroutine(BossControl());
    }


    void Update()
    {



    }

    IEnumerator BossControl()
    {
        if (fase == 1 && !isAttacking && firstFaseHits < firstFaseMaxHits)
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
            if (firstFaseHits == firstFaseMaxHits)
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
        else if (fase == 4)
        {
            isVulnerable = true;
            if (isLeftHandActive)
            {
                print("mao esquerda ativa");


                yield return StartCoroutine(leftHandScript.ChangeFollowRoutine());



            }
            else if (isRightHandActive)
            {
                print("mao direita ativa");
                yield return StartCoroutine(rightHandScript.ChangeFollowRoutine());

            }
        }

        yield return new WaitForSeconds(Random.Range(3.5f, 5.6f));


        StartCoroutine(BossControl());
        print("BossControl Coroutine Started");

    }

   public void ChangeFase()
    {
        print("Changing fase");
        if (fase == 2)
        {
            print("Entrando na fase 2");
        }
        if (fase == 3)
        {
            waterAnimator.SetTrigger("enterScene");
            SoundManager.PlaySound(SoundType.tichoFase3Frase);
            leftHandScript.Burn();
            rightHandScript.Burn();
            print("Entrando na fase 3");
        }
        if (fase == 4)
        {

            print("Entrando na fase 4");
            isVulnerable = true;


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isVulnerable && collision.gameObject.CompareTag("WaterJet"))
        {
           
            
                bossHP--;
            GlassSprite.SetActive(false);
            print("Boss HP: " + bossHP);
               
                if (bossHP <= 0)
                {
                    // Boss defeated logic here
                    print("Boss defeated!");
                    // You can add more logic here, like playing an animation or transitioning to a new scene.
                }
            
        }
    }
}


   
 