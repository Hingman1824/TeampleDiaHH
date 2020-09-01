using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChar : MonoBehaviour
{

    public Character character;
    Animator anim;

    MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mr = GetComponent<MeshRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        DataManager.instance.currentCharacter = character;
    }

    void OnSelect()
    {
        anim.SetBool("run", true);
        
    }
}
