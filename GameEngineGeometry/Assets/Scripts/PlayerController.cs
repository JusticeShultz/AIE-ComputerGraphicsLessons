using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static GameObject player;
    public static PlayerController controller;

    public float Health = 10f;
    public float MovementSpeed = 10f; //The speed the player can move at when not aiming.
    public float MovementSpeedAiming = 5f; //The speed the player can move at when aiming.
    public float RunSpeed = 15f; //The speed the player can move at when running.
    public float Stamina = 5f; //The stamina the player can use to run.
    public new Rigidbody rigidbody; //The players rigidbody.
    public Animator animator; //The players animator.
    public Animator animator_damage; //The players animator.
    public GameObject PlaceholderArrow; //The reference to the fake arrow object.
    public GameObject Arrow; //The reference to the arrow prefab.
    public GameObject AimOffset; //The aiming offset object to rotate the root.
    public float AimDrawDuration = 0.5f; //The speed at which the player draws an arrow.
    public Image HealthRadial;
    public Image StaminaRadial;
    public Animator DeathScreen;
    public GameObject Button;

    [SerializeField] private AnimationCurve AimParameterCurve; //The curve at which we fire.

    private float Aim = 0.0f; //Our aim value.
    private float AimNormalizedTime { get { return Mathf.Clamp(Aim / AimDrawDuration, 0, 1); } } //Grab the normalized value of Aim.
    private float Horizontal = 0; //The value given from pressing left or right inputs.
    private float Vertical = 0; //The value given from pressing up or down inputs.
    private bool doDeathOnce = false;
    private float MaxHealth = 0f;
    private float MaxStamina = 0f;
    private bool HitNoStamina = false;
    private bool Sprinting = false;

    private void Awake()
    {
        MaxHealth = Health;
        MaxStamina = Stamina;

        player = gameObject;
        controller = this;
    }

    void FixedUpdate()
    {
        if (Health <= 0 && !doDeathOnce)
        {
            doDeathOnce = true;
            animator.SetTrigger("Death");
            DeathScreen.SetTrigger("Die");
            Button.SetActive(true);
            rigidbody.isKinematic = true;
            transform.position = new Vector3(transform.position.x, -0.33f, transform.position.z);
        }

        if (doDeathOnce) return;

        //Grab the horizontal and vertical input axesis.
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        //Apply force based on AimNormalizedTime state and the horizontal and vertical values.
        if (Sprinting)
            rigidbody.AddForce(Vector3.Normalize((new Vector3(Horizontal, 0, 0) + new Vector3(0, 0, Vertical))) * RunSpeed, ForceMode.VelocityChange);
        else if (AimNormalizedTime <= 0.45f)
            rigidbody.AddForce(Vector3.Normalize((new Vector3(Horizontal, 0, 0) + new Vector3(0, 0, Vertical))) * MovementSpeed, ForceMode.VelocityChange);
        else rigidbody.AddForce(Vector3.Normalize((new Vector3(Horizontal, 0, 0) + new Vector3(0, 0, Vertical))) * MovementSpeedAiming, ForceMode.VelocityChange);

        //If we are moving, update the players rotation.
        if (rigidbody.velocity.magnitude > 0.5f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity, Vector3.up), 0.4f);

        //Depending on if we're aiming or not, rotate the player by an offset.
        if (AimNormalizedTime >= 0.25f)
            AimOffset.transform.localRotation = Quaternion.Lerp(AimOffset.transform.localRotation, Quaternion.Euler(0, 60, 0), 0.2f);
        else AimOffset.transform.localRotation = Quaternion.Lerp(AimOffset.transform.localRotation, Quaternion.Euler(0, 0, 0), 0.2f);

        //If we are charged enough then make a dummy arrow display. (The arrow the player holds in the bow)
        if (AimNormalizedTime <= 0.85f)
            PlaceholderArrow.SetActive(false);
        else PlaceholderArrow.SetActive(true);
    }

    private void Update()
    {
        HealthRadial.fillAmount = Mathf.Lerp(HealthRadial.fillAmount, Health / MaxHealth, 0.1f);
        StaminaRadial.fillAmount = Mathf.Lerp(StaminaRadial.fillAmount, Stamina / MaxStamina, 0.1f);

        if (doDeathOnce) return;

        if(Stamina < MaxStamina && !Sprinting)
        {
            Stamina += Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);

            if (Stamina == MaxStamina) HitNoStamina = false;
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Stamina > 0 && !HitNoStamina && AimNormalizedTime <= 0.45f && rigidbody.velocity.magnitude > 0.3f)
        {
            Stamina -= Time.deltaTime;
            Sprinting = true;
        }
        else Sprinting = false;

        if(Stamina <= 0)
        {
            HitNoStamina = true;
        }

        //Determine the level of aiming and set it.
        //0 to 1, where 0 represents the beginning of the aim, and 1 is the end.
        if (Input.GetMouseButton(0)) //|| Input.GetAxis("PrimaryAttack") > 0.1f)
            Aim = Mathf.Min(AimDrawDuration, Aim + Time.deltaTime);
        else Aim = Mathf.Max(0.0f, Aim - Time.deltaTime);

        //If the left mouse button is let go and we are aiming and the inventory is closed.
        if (Input.GetMouseButtonUp(0) && AimNormalizedTime >= 0.85f && !InventoryToggle.InventoryOpened)
        {
            //Set the time we aim for to 0 so we cannot refire instantly.
            Aim = 0;
            //Spawn an arrow.
            GameObject arrow = Instantiate(Arrow, PlaceholderArrow.transform.position, PlaceholderArrow.transform.rotation);
            //Launch the arrow.
            arrow.GetComponent<Rigidbody>().AddForce(PlaceholderArrow.transform.forward * 450);

            //I decided after the change due to the frame rate bug this animation just doesn't work in this game anymore(smoothly). This is mostly mixamos fault and how
            //it touches the base root called "Hips" with Unity's humanoid structure.
            //Tell the animator to do the fire animation.
            //animator.SetTrigger("FireArrow");
        }

        //If we are or aren't moving, tell the animator.
        if (Horizontal == 0 && Vertical == 0)
            animator.SetBool("Moving", false);
        else animator.SetBool("Moving", true);

        //Set the aim value in the animator.
        animator.SetFloat("Aiming", AimParameterCurve.Evaluate(AimNormalizedTime));
        //Set the movement value in the animator.
        animator.SetFloat("MovementSpeed", rigidbody.velocity.magnitude * 0.16f);
    }

    //Note to self, update to 
    //public static void TakeDamage(float damage, DamageType typeOfDamage)
    //to allow physical and magical damage resistances if revisited.
    public static void TakeDamage(float damage)
    {
        controller.animator_damage.SetTrigger("Damage");
        controller.Health -= damage;
    }
}