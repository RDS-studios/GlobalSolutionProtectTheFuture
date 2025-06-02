using UnityEngine;

public class HookScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer hookPos;
    [SerializeField] Transform hookTransform;
    [SerializeField]Transform startPoint;
    public Vector3 targetPos;
    public Transform targetTransform;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        // mouse position to world point
        
        targetPos.z = 0;
        targetPos.x = targetTransform.position.x;
        targetPos.y = targetTransform.position.y;
        // update wire
        // update position
        hookTransform.position = targetPos;

        // update direction
        Vector3 direction = targetPos - startPoint.position;
        hookTransform.right = direction * transform.lossyScale.x;

        // update scale
        float dist = Vector2.Distance(startPoint.position, targetPos );
        hookPos.size = new Vector2(dist, hookPos.size.y);
    }
}
