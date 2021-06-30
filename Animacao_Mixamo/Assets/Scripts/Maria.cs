using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maria : MonoBehaviour
{
    public Animator anim;
    public Rigidbody cr;
    public CharacterController charController;

    float vel_horiz = 3.0f;
    float vel_giro = 80f;
    float vel_pulo = 5f;
    float vel_angular;
    float gravidade = 9.8f;
    float velocidade;
    Vector3 vel = Vector3.zero;
    private bool correr = false;
    private bool pular = false;
    private bool pulando = false;
    private bool descansar = true;
    private bool atacando = false;
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
        GetMovementInputs();
        GetAttackInputs();
    }

    private void FixedUpdate()
    {
        var impulse = (vel_angular * Mathf.Deg2Rad) * Vector3.up;
        cr.AddTorque(impulse, ForceMode.Impulse);
    }

    void GetMovementInputs()
    {
        if (Input.GetKeyDown("space"))
        {
            anim.Play("Maria_jump");
            pular = true;
            pulando = true;
        }
        //else if(charController.isGrounded)
        else if (cr.velocity.y < float.Epsilon)
        {
            pulando = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            correr = true;
        }
        else
        {
            correr = false;
        }

        float entradaH = Input.GetAxis("Horizontal");
        float entradaV = Input.GetAxis("Vertical");

        float mouseXInput = Input.GetAxis("Mouse X");
        vel_angular = vel_giro * Mathf.Clamp(mouseXInput, -1, 1);

        Vector3 moveZ = transform.forward * entradaV;
        Vector3 moveX = transform.right * entradaH;

        if (correr)
        {
            moveX *= 3f;
            moveZ *= 3f;
            //anim.speed = 2f;
            velocidade = 0.5f;
            descansar = false;
        }
        else
        {
            //anim.speed = 1f;
            velocidade = 0.2f;
            descansar = false;
        }
        Vector3 moveHor = moveX + moveZ;


        vel = moveHor * vel_horiz + vel.y * Vector3.up;
        //Vector3 vel_pulo = Vector3.up * 2f * (pular ? 1 : 0);
        //vel += Vector3.up * vel_pulo * (pular ? 1 : 0);

        descansar = moveHor.sqrMagnitude < float.Epsilon;
        velocidade = descansar ? 0f : velocidade;

        if (pular && charController.isGrounded)
        {
            vel.y = vel_pulo;
            //cr.AddForce(100f * Vector3.up, ForceMode.Force);
            pular = false;
        }

        if (!charController.isGrounded)
        {
            //vel.y += Physics.gravity.y * Time.deltaTime;
            vel.y -= gravidade * Time.deltaTime;
        }
        //charController.Move((vel + vel_pulo) * Time.deltaTime);
        charController.Move(vel * Time.deltaTime);

        anim.SetFloat("entradaH", entradaH);
        anim.SetFloat("entradaV", entradaV);
        anim.SetBool("descansar", descansar);
        anim.SetFloat("velocidade", velocidade);
        if (charController.isGrounded)
        {
            anim.SetBool("correr", correr);
            anim.SetFloat("velocidade", velocidade);
        }
        else
        {
            anim.SetBool("correr", false);
            anim.SetFloat("velocidade", 0f);
        }
    }
    void GetAttackInputs()
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
                if (atacando)
                {
                    atacando = anim.GetCurrentAnimatorStateInfo(0).IsName(Combo1[attack_num]) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
                    //print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    attack_num = atacando ? attack_num : (++attack_num % Combo1.Length);
                }
                else
                {
                    //anim.Play("Maria_slash_" + attack_num);
                    anim.Play(Combo1[attack_num]);
                    atacando = true;
                }
            }
            else
            {
                if (atacando)
                    atacando = anim.GetCurrentAnimatorStateInfo(0).IsName("Maria_jump_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
                else
                {
                    anim.Play("Maria_jump_attack");
                    atacando = true;
                }
            }
        }
    }
}
