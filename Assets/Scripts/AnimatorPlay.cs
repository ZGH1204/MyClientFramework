using UnityEngine;

public class AnimatorPlay : MonoBehaviour
{
    private Animator ani;

    // Use this for initialization
    private void Start()
    {
        ani = this.GetComponent<Animator>();

        ani.Play("");
    }

    public void Jump()
    {
        ani.Play("Jump");
    }

    public void Walk_Crouch_Rilfe()
    {
        ani.SetInteger("move", 0);
    }

    public void Run_Strafe_Left()
    {
        ani.SetInteger("move", 1);
    }

    public void Run_Strafe_Right()
    {
        ani.SetInteger("move", 2);
    }

    public void Run_End()
    {
        ani.SetInteger("move", -1);
    }
}