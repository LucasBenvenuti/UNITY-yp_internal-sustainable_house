using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Anim : MonoBehaviour
{
    private bool menuOpen = false;
    private bool soundImg = true;
    public Animator menuAnim;
    public Animator soundAnim;

    public void AnimateMenu()
    {
        menuOpen = !menuOpen;

        if (!menuOpen)
        {
            menuAnim.SetTrigger("Close");
        }
        else
        {
            menuAnim.SetTrigger("Open");
        }
    }

    public void AnimateSound()
    {
        soundImg = !soundImg;

        if (!soundImg)
        {
            soundAnim.SetTrigger("Off");
        }
        else
        {
            soundAnim.SetTrigger("On");
        }
    }
}
