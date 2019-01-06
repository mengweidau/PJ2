using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour {
    TargetTemp parentscript;
    public Animator anim;

    private void Awake()
    {
        //i fetch the parent script (can be enemy)
        parentscript = transform.parent.GetComponent<TargetTemp>();
    }

    // Update is called once per frame
    void Update () {

        //when parent's health is below 3, i set the lowHealthBool from this animation controller's parameter (lowhealthbool) to true
         if (parentscript.GetHealth() < 3)
            anim.SetBool("LowHealthBool", true);
    }
}
