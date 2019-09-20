using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : MonoBehaviour
{
    public float MovementSpeed = 0.1f;
    public float JumpHeight = 25f;
    public float FallMultiplier = 2.5f;

    public GameObject forward;
    public new GameObject camera;
    public GameObject shotPoint;
    public GameObject bullet;
    public new Rigidbody rigidbody;
    public Animator animator;

    public float GunShotCooldown = 1.0f;
    private float ShotCooldown = 0f;
    private float distToGround;
    private float JumpTime = 0f;

    private void Start()
    {
        distToGround = GetComponent<CapsuleCollider>().height * 0.7f;
    }

    void Update()
    {
        Application.targetFrameRate = 120;
        
        ShotCooldown += Time.deltaTime;

        forward.transform.position = camera.transform.position;
        forward.transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y,0);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rigidbody.AddForce(Vector3.Normalize((h * forward.transform.right) + (v * forward.transform.forward)) * MovementSpeed);

        animator.SetFloat("MoveSpeed", rigidbody.velocity.magnitude * 0.09f);
        animator.SetBool("Moving", rigidbody.velocity.magnitude > 0.25);

        if (Input.GetMouseButton(0) && GunShotCooldown <= ShotCooldown)
        {
            ShotCooldown = 0;
            animator.SetTrigger("FireGun");
            Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation);

            //Do damage raycheck here
        }

        if(Input.GetButton("Jump") && JumpTime <= 0.09f)
        {
            JumpTime += Time.deltaTime;
            rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
        }

        if (rigidbody.velocity.y < 0)
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier * 1.7f) * Time.deltaTime;

        if (IsGrounded()) JumpTime = 0;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }
}