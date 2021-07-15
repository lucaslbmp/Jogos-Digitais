using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maria : MonoBehaviour
{
    public Animator anim;
    public Rigidbody cr;
    public CharacterController charController;

    //constantes
    float walk_speed = 3f;
    float run_speed = 6f;
    float vel_turn_max = 80f;
    float vel_pulo = 5f;
    float gravity = 9.8f;

    float entradaH = 0f, entradaV = 0f;
    float vel_turn;
    float velAnimation;
    float moveSpeed;

    Vector3 vel = Vector3.zero;

    private bool run = false;
    private bool jump = false;
    private bool jumping = false;
    private bool rest = true;
    private bool attacking = false;
    int attack_num = 0;
    
    Camera mainCamera;

    string[] Combo1 = { "Maria_slash_0", "Maria_slash_2" };
    

    void Start()
    {
        anim = GetComponent<Animator>();
        cr = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMovementStates();
        UpdateAttackStates();
    }

    private void FixedUpdate()
    {
        UpdateAnimations();
        var impulse = (vel_turn * Mathf.Deg2Rad) * Vector3.up;
        cr.AddTorque(impulse, ForceMode.Impulse);
    }

    void UpdateMovementStates()
    {
        UpdateJumpState();
        UpdateRunState();

        entradaH = Input.GetAxis("Horizontal");
        entradaV = Input.GetAxis("Vertical");
        float mouseXInput = Input.GetAxis("Mouse X");
        
        vel_turn = vel_turn_max * Mathf.Clamp(mouseXInput, -1, 1);
        Vector3 moveHor = transform.right * entradaH + transform.forward * entradaV;
        vel = moveHor * moveSpeed + vel.y * Vector3.up;

        rest = moveHor.sqrMagnitude < float.Epsilon;
        velAnimation = rest ? 0f : velAnimation;

        charController.Move(vel * Time.deltaTime);
    }

    void UpdateJumpState()
    {
        //print(jump);
        if (Input.GetKeyDown("space"))
        {
            //anim.Play("Maria_jump");
            jump = true;
            jumping = true;
        }
        //else if(charController.isGrounded)
        else if (cr.velocity.y < .01 && charController.isGrounded)
        {
            //vel.y = 0f;
            jumping = false;
        }
        else if (jump && charController.isGrounded)
        {
            vel.y = vel_pulo;
            jump = false;
        }

        if (!charController.isGrounded)
        {
            vel.y -= gravity * Time.deltaTime;
        }
    }

    void UpdateRunState()
    {
        run = Input.GetKey(KeyCode.LeftShift);
        if (run)
        {
            moveSpeed = run_speed;
            velAnimation = 0.5f;
            rest = false;
        }
        else
        {
            moveSpeed = walk_speed;
            velAnimation = 0.2f;
            rest = false;
        }
    }

    void UpdateAttackStates()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (charController.isGrounded && !atacando)
            // {
            //     anim.Play("Maria_slash_"+attack_num);
            //     atacando = true;
            // }
            // else if (atacando)
            // {
            //     atacando = anim.GetCurrentAnimatorStateInfo(0).IsName("Maria_slash_" + attack_num) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
            //     //print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //     attack_num = atacando?attack_num:(++attack_num % 3);
            // }
            // else if(!charController.isGrounded && !atacando)
            // {
            //     anim.Play("Maria_slash_4");
            //     atacando = true;
            // }
            if (charController.isGrounded)
            {
                if (attacking)
                {
                    attacking = anim.GetCurrentAnimatorStateInfo(0).IsName(Combo1[attack_num]) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
                    //print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    attack_num = attacking ? attack_num : (++attack_num % Combo1.Length);
                }
                else
                {
                    //anim.Play("Maria_slash_" + attack_num);
                    anim.Play(Combo1[attack_num]);
                    attacking = true;
                }
            }
            else
            {
                if (attacking)
                    attacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Maria_jump_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
                else
                {
                    anim.Play("Maria_jump_attack");
                    attacking = true;
                }
            }
        }
    }

    void UpdateAnimations()
    {
        anim.SetFloat("entradaH", entradaH);
        anim.SetFloat("entradaV", entradaV);
        anim.SetBool("descansar", rest);
        anim.SetFloat("velocidade", velAnimation);
        if (jump && charController.isGrounded)
        {
            anim.Play("Maria_jump");
        }
        //if (jump && charController.isGrounded)
        //{
        //    anim.SetTrigger("pular");
        //}
        if (charController.isGrounded)
        {
            anim.SetBool("correr", run);
            anim.SetFloat("velocidade", velAnimation);
        }
        else
        {
            anim.SetBool("correr", false);
            anim.SetFloat("velocidade", 0f);
        }
    }
}
