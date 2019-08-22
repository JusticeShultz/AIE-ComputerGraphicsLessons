using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public float MovementSpeedAiming = 5f;
    public new Rigidbody rigidbody;
    public Animator animator;
    public float AimSpeed = 4;
    public GameObject PlaceholderArrow;
    public GameObject Arrow;

    private float Aim = 0.0f;
    private float horizontal = 0;
    private float vertical = 0;

    void Start ()
    {
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Aim <= 0.45f)
            rigidbody.AddForce((new Vector3(horizontal, 0, 0) + new Vector3(0, 0, vertical)) * MovementSpeed, ForceMode.VelocityChange);
        else rigidbody.AddForce((new Vector3(horizontal, 0, 0) + new Vector3(0, 0, vertical)) * MovementSpeedAiming, ForceMode.VelocityChange);

        if (rigidbody.velocity.magnitude > 0.5f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity, Vector3.up), 0.4f);

        if (Aim <= 0.85f)
            PlaceholderArrow.SetActive(false);
        else PlaceholderArrow.SetActive(true);
    }

    private void Update()
    {
        //Determine the level of aiming.
        if (Input.GetMouseButton(0))
            Aim = Mathf.Lerp(Aim, 1.0f, Time.deltaTime * AimSpeed);
        else Aim = Mathf.Lerp(Aim, 0.0f, (Time.deltaTime * AimSpeed) * 2);

        if (Input.GetMouseButtonUp(0) && Aim >= 0.85f)
        {
            GameObject arrow = Instantiate(Arrow, PlaceholderArrow.transform.position, PlaceholderArrow.transform.rotation);
            arrow.GetComponent<Rigidbody>().AddForce(PlaceholderArrow.transform.forward * 450);

            animator.SetTrigger("FireArrow");
        }

        if (horizontal == 0 && vertical == 0)
            animator.SetBool("Moving", false);
        else animator.SetBool("Moving", true);

        //Animator junk.
        animator.SetFloat("Aiming", Aim);
        animator.SetFloat("MovementSpeed", rigidbody.velocity.magnitude * 0.16f);
    }
}