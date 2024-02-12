using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleConstructionMenu : MonoBehaviour
{
    public Animator menuAnimator;
    private bool isOpen = false;

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuAnimator.SetBool("IsOpen", isOpen);
    }
}
