using UnityEngine;

public class AlertUI : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Fade()
    {
        animator.Play("AlertUI", -1, 0);
    }
}
