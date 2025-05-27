using EditorCools.Editor;
using SmallHedge.SoundManager;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuButtons : MonoBehaviour, IPointerEnterHandler
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       SoundManager.PlaySound(SoundType.BtnHover);
    }

   

}
