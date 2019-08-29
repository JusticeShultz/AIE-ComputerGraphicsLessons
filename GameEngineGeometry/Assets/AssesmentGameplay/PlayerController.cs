using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 10f; //The speed the player can move at when not aiming.
    public float MovementSpeedAiming = 5f; //The speed the player can move at when aiming.
    public new Rigidbody rigidbody; //The players rigidbody.
    public Animator animator; //The players animator.
    public float AimSpeed = 4; //The speed the player can aim at.
    public GameObject PlaceholderArrow; //The reference to the fake arrow object.
    public GameObject Arrow; //The reference to the arrow prefab.
    public GameObject AimOffset; //The aiming offset object to rotate the root.

    private float Aim = 0.0f; //Our aim value.
    private float horizontal = 0; //The value given from pressing left or right inputs.
    private float vertical = 0; //The value given from pressing up or down inputs.

    void FixedUpdate()
    {
        //Grab the horizontal and vertical input axesis.
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //Apply force based on aim state and the horizontal and vertical values.
        if (Aim <= 0.45f)
            rigidbody.AddForce((new Vector3(horizontal, 0, 0) + new Vector3(0, 0, vertical)) * MovementSpeed, ForceMode.VelocityChange);
        else rigidbody.AddForce((new Vector3(horizontal, 0, 0) + new Vector3(0, 0, vertical)) * MovementSpeedAiming, ForceMode.VelocityChange);

        //If we are moving, update the players rotation.
        if (rigidbody.velocity.magnitude > 0.5f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity, Vector3.up), 0.4f);

        //Depending on if we're aiming or not, rotate the player by an offset.
        if (Aim >= 0.25f)
            AimOffset.transform.localRotation = Quaternion.Lerp(AimOffset.transform.localRotation, Quaternion.Euler(0, 60, 0), 0.4f);
        else AimOffset.transform.localRotation = Quaternion.Lerp(AimOffset.transform.localRotation, Quaternion.Euler(0, 0, 0), 0.4f);

        //If we are charged enough then make a dummy arrow display. (The arrow the player holds in the bow)
        if (Aim <= 0.85f)
            PlaceholderArrow.SetActive(false);
        else PlaceholderArrow.SetActive(true);
    }

    private void Update()
    {
        //Determine the level of aiming and set it.
        if (Input.GetMouseButton(0))
            Aim = Mathf.Lerp(Aim, 1.0f, Time.deltaTime * AimSpeed);
        else Aim = Mathf.Lerp(Aim, 0.0f, (Time.deltaTime * AimSpeed) * 2);

        //If the left mouse button is let go and we are aiming and the inventory is closed.
        if (Input.GetMouseButtonUp(0) && Aim >= 0.85f && !InventoryToggle.InventoryOpened)
        {
            //Spawn an arrow.
            GameObject arrow = Instantiate(Arrow, PlaceholderArrow.transform.position, PlaceholderArrow.transform.rotation);
            //Launch the arrow.
            arrow.GetComponent<Rigidbody>().AddForce(PlaceholderArrow.transform.forward * 450);
            //Tell the animator to do the fire animation.
            animator.SetTrigger("FireArrow");
        }

        //If we are or aren't moving, tell the animator.
        if (horizontal == 0 && vertical == 0)
            animator.SetBool("Moving", false);
        else animator.SetBool("Moving", true);

        //Set the aim value in the animator.
        animator.SetFloat("Aiming", Aim);
        //Set the movement value in the animator.
        animator.SetFloat("MovementSpeed", rigidbody.velocity.magnitude * 0.16f);
    }
}