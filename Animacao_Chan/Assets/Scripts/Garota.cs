using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garota : MonoBehaviour
{
    public Animator anim;
    public Rigidbody cr;
    public CharacterController charController;

    float vel_frente = 3.0f;
    float vel_lado = 5.0f;
    float vel_giro = 80f;
    float vel_angular;
    float gravidade = 16f;
    float velocidade;
    Vector3 vel = Vector3.zero;
    private bool correr = false;
    private bool pular = false;
    private bool pulando = false;
    private bool descansar = true;
    Camera mainCamera;

    void Start()
    {
        anim = GetComponent<Animator>();
        cr = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void FixedUpdate()
    {
        var impulse = (vel_angular * Mathf.Deg2Rad) * Vector3.up;
        cr.AddTorque(impulse,ForceMode.Impulse);
    }

    void GetInputs()
    { 
        for(int i = 0; i <= 4; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                anim.Play("WAIT0" + i.ToString());
            }
        }

        if (Input.GetKeyDown("space"))
        {
            anim.Play("JUMP00");
            pular = true;
            pulando = true;
        }
        //else if(charController.isGrounded)
        else if (cr.velocity.y < float.Epsilon)
        {
            pulando = false;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.Play("LOSE00");
        }
        if (Input.GetKeyDown(KeyCode.C) && Input.GetAxis("Vertical") > float.Epsilon)
        //if (Input.GetKeyDown(KeyCode.C))
        {
            anim.Play("SLIDE00");
        }

        if (Input.GetMouseButtonDown(0))
        {
            int n = Random.Range(0, 2);
            if (n == 0)
            {
                anim.Play("DAMAGED00");
            }
            else
            {
                anim.Play("DAMAGED01");
            }
            
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
        anim.SetFloat("entradaH", entradaH);
        anim.SetFloat("entradaV", entradaV);
        anim.SetBool("correr", correr);

        float mouseXInput = Input.GetAxis("Mouse X");
        vel_angular = vel_giro * Mathf.Clamp(mouseXInput, -1, 1);

        Vector3 moveZ = transform.forward * entradaV;
        Vector3 moveX = transform.right * entradaH;
        if (moveZ.sqrMagnitude <= float.Epsilon)
        {
            moveX = Vector3.zero;
        }
        else if (correr)
        {
            moveX *= 3f;
            moveZ *= 3f;
            //anim.speed = 2f;
            velocidade = 0.5f;
            descansar = false;
        }
        else if (!correr)
        {
            //anim.speed = 1f;
            velocidade = 0.2f;
            descansar = false;
        }
        Vector3 moveHor = moveX + moveZ;
        

        vel = moveHor * vel_frente + vel.y * Vector3.up;
        //Vector3 vel_pulo = Vector3.up * 2f * (pular ? 1 : 0);

        descansar = moveHor.sqrMagnitude < float.Epsilon;
        velocidade = descansar ? 0f : velocidade;
        //print(vel.sqrMagnitude);

        if (pular && charController.isGrounded)
        {
            vel.y = 10f;
            pular = false;
        }

        if (!charController.isGrounded)
        {
            //vel.y += Physics.gravity.y * Time.deltaTime;
            vel.y -= gravidade*Time.deltaTime;
        }
        else
        {
            if (vel.y < float.Epsilon)
            {
                vel.y = 0f;
            }
        }
        charController.Move(vel*Time.deltaTime);

        anim.SetBool("descansar", descansar);
        anim.SetFloat("velocidade", velocidade);

        //float moveX = entradaH * vel_side * Time.deltaTime;
        //float moveZ = entradaV * vel_forward * Time.deltaTime;
        //if(Mathf.Abs(moveZ) <= float.Epsilon)
        //{
        //    moveX = 0f;
        //}
        //else if (correr)
        //{
        //    moveX *= 3f;
        //    moveZ *= 3f;
        //}
        //cr.AddForce(10f*(new Vector3(moveX, 0f, moveZ)), ForceMode.VelocityChange);
        //cr.velocity = new Vector3(moveX, cr.velocity.y, moveZ);
    }
}
