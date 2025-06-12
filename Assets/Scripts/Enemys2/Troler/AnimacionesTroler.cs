using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesTroler : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public  void AproximateAttack()
    {
        animator.SetTrigger("AproximateAttack");
    }
}
