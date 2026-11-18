using UnityEngine;

public class Viewer : MonoBehaviour
{
    
    public void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("viewer_idle", -1, Random.Range(0f, 1f));
        animator.SetFloat("idleSpeedMultiplier", Random.Range(0.5f, 2f));
    }
}
