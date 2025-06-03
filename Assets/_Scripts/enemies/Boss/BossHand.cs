using System.Collections;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool isFollowingPlayer = false;
    [SerializeField] Transform originalPos;
    [SerializeField] Transform playerTransform;
    [SerializeField] BossHead bossHeadScript;
    [SerializeField] bool isVunerable = false;
    [SerializeField] int vida = 3; // Health of the hand
    private bool isAttacking = false;
    [SerializeField] SpriteRenderer maoRenderer;
    [SerializeField] Sprite maoSpriteAberta;
    [SerializeField] Sprite maoSpriteFechada;

    [SerializeField] GameObject fogoAberto;
    [SerializeField] GameObject fogoFechado;
    [SerializeField] bool isLeftHand;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAttacking) return; // prevent movement during attack

        if (isFollowingPlayer)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(playerTransform.position.x, transform.position.y),
                0.1f
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos.position, 0.1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalPos.rotation, 0.1f);
        }
    }

    public IEnumerator ChangeFollowRoutine()
    {
        isFollowingPlayer = !isFollowingPlayer;

        if (isFollowingPlayer)
        {
             
            animator.SetTrigger("enterAttack");
            bossHeadScript.isAttacking = true;

            if (bossHeadScript.fase == 1)
            {
                yield return StartCoroutine(AttackPlayer1());
            }
            else if (bossHeadScript.fase == 3)
            {
                yield return StartCoroutine(AttackPlayer3());
            }
            else if (bossHeadScript.fase == 4)
            {
                int randomNum = Random.Range(0, 2);
                if (randomNum == 0)
                {
                     
                    yield return StartCoroutine(AttackPlayer3());
                    yield return new WaitForSeconds(1f); // Wait for the attack to finish
                    yield return StartCoroutine(SmashMiddleRoutine2());
                }
                else
                {
                    
                    yield return StartCoroutine(SmashMiddleRoutine2());
                    yield return new WaitForSeconds(1f); // Wait for the attack to finish
                    yield return StartCoroutine(AttackPlayer3());
                }

            }
        }
        else
        {
            transform.position = originalPos.position;
            transform.rotation = originalPos.rotation;
            bossHeadScript.isAttacking = false;
        }
    }


    public IEnumerator AttackPlayer1()
    {
       

        yield return new WaitForSeconds(4f); // Let enterAttack animation play
        isAttacking = true;
        // Step 1: Rise
        Vector3 risePos = transform.position + Vector3.up * 2f;
        while (Vector3.Distance(transform.position, risePos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, risePos, 5f * Time.deltaTime);
            yield return null; // <-- this is crucial!
        }
        maoRenderer.sprite = maoSpriteFechada; // Change to closed hand sprite
        yield return new WaitForSeconds(0.2f);

        // Step 2: Slam down
        Vector3 slamPos = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z); // or use playerTransform.position.y
        while (Vector3.Distance(transform.position, slamPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, slamPos, 20f * Time.deltaTime);
            yield return null; // <-- this is also crucial!
        }
        


        yield return new WaitForSeconds(1f);

        maoRenderer.sprite = maoSpriteAberta; // Change to open hand sprite

        // Step 4: Return to original position
        while (Vector3.Distance(transform.position, originalPos.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos.position, 5f * Time.deltaTime);
            yield return null;
        }

        isAttacking = false;
       
        StartCoroutine( ChangeFollowRoutine()); // revert to normal behavior
        animator.SetTrigger("attack");
    }
    
   public IEnumerator SmashMiddleRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("enterAttack"); // Trigger the attack animation
        bossHeadScript.isAttacking = true; // Set the boss head's attacking state   
        
        // Step 1: Slam downward
        Vector3 slamPos = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z); // Adjust Y as needed
        while (Vector3.Distance(transform.position, slamPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, slamPos, 20f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // Pause before moving to middle

        // Step 2: Move to center of arena
        Vector3 middlePos = new Vector3(0, transform.position.y, transform.position.z); // Adjust X=0 to center X
        while (Vector3.Distance(transform.position, middlePos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, middlePos, 20f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // Optional pause

        // Step 3: Return to original position
        while (Vector3.Distance(transform.position, originalPos.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos.position, 5f * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger("attack"); // Trigger the exit animation
        isAttacking = false;
        bossHeadScript.isAttacking = false; // Reset the boss head's attacking state
    }

    public IEnumerator SmashMiddleRoutine2()
    {
        isAttacking = true;
        animator.SetTrigger("enterAttack"); // Trigger the attack animation
        bossHeadScript.isAttacking = true; // Set the boss head's attacking state   

        // Step 1: Slam downward
        // Step 1: Slam downward
        maoRenderer.sprite = maoSpriteFechada;
        fogoAberto.SetActive(false);
        fogoFechado.SetActive(true);

        Vector3 slamPos = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, slamPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, slamPos, 20f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Step 2: Move to center
        int xpos = isLeftHand ? 12 : -12;
        Vector3 middlePos = new Vector3(xpos, transform.position.y, transform.position.z);
        while (Vector3.Distance(transform.position, middlePos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, middlePos, 20f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Step 3: Return to origin
        while (Vector3.Distance(transform.position, originalPos.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos.position, 5f * Time.deltaTime);
            yield return null;
        }

        // Reset visuals
        maoRenderer.sprite = maoSpriteAberta;
        fogoFechado.SetActive(false);
        fogoAberto.SetActive(true);

        animator.SetTrigger("attack");
        isAttacking = false;
        bossHeadScript.isAttacking = false;
    }


    public IEnumerator AttackPlayer3()
    {


        yield return new WaitForSeconds(4f); // Let enterAttack animation play
        isAttacking = true;
        // Step 1: Rise

        Vector3 risePos = transform.position + Vector3.up * 2f;
        while (Vector3.Distance(transform.position, risePos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, risePos, 5f * Time.deltaTime);
            yield return null; // <-- this is crucial!
        }

        yield return new WaitForSeconds(0.2f);

        // Step 2: Slam down
        fogoAberto.SetActive(false); // Disable open fire   
        fogoFechado.SetActive(true); // Enable closed fire
        maoRenderer.sprite = maoSpriteFechada; // Change to closed hand sprite
        Vector3 slamPos = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z); // or use playerTransform.position.y
        while (Vector3.Distance(transform.position, slamPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, slamPos, 20f * Time.deltaTime);
            yield return null; // <-- this is also crucial!
        }
        isVunerable = true; // Set the hand to be vulnerable after the slam
        yield return new WaitForSeconds(1.5f);  
        
       
        yield return new WaitForSeconds(1f);
        isVunerable = false; // Reset vulnerability after the attack
        // Step 4: Return to original position
        fogoFechado.SetActive(false); // Disable closed fire
        fogoAberto.SetActive(true); // Enable open fire
        maoRenderer.sprite = maoSpriteAberta; // Change to open hand sprite
        while (Vector3.Distance(transform.position, originalPos.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos.position, 5f * Time.deltaTime);
            yield return null;
        }
        
        isAttacking = false;
        StartCoroutine(ChangeFollowRoutine()); // revert to normal behavior
        animator.SetTrigger("attack");
        if(vida <= 0)
        {
            animator.SetTrigger("Break");
            Destroy(gameObject, 5f); // Destroy the hand after the break animation
            bossHeadScript.fase++;
            bossHeadScript.ChangeFase();  
            bossHeadScript.hasBothHands = false; // Set the boss to not have both hands
            if (isLeftHand)
            {
                bossHeadScript.isLeftHandActive = false; // Disable left hand
            }
            else
            {
                bossHeadScript.isRightHandActive = false; // Disable right hand
            }
        }
    }

    public void Burn()
    {
        fogoAberto.SetActive(true);
        fogoFechado.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterJet") && isVunerable)
        { 
         vida--;
        }

        
    }
}
