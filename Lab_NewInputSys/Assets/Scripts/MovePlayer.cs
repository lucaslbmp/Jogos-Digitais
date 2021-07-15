using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator anim;
    public bool isWalking = false;
    public bool isRunning = false;

    PlayerControls input;
    Vector2 Movimento = new Vector2();
    bool movimentoPressionado;
    bool runPressionado;

    private void Awake()
    {
        input = new PlayerControls();
        input.Player.Move.performed += ctx =>
        {
            Movimento = ctx.ReadValue<Vector2>();
            movimentoPressionado = (Movimento.SqrMagnitude() > float.Epsilon);
        };
        input.Player.Run.performed += ctx => runPressionado = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isRunning);
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Rotacionar();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    void Mover()
    {
        if(movimentoPressionado && !isWalking)
        {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
        }
        if(!movimentoPressionado && isWalking)
        {
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }

        if ((movimentoPressionado && runPressionado) && !isRunning)
        {
            isRunning = true;
            anim.SetBool("isRunning", isRunning);
        }
        if (!(movimentoPressionado && runPressionado) && isRunning)
        {
            isRunning = false;
            anim.SetBool("isRunning", isRunning);
        }
    }

    void Rotacionar()
    {
        Vector3 atualPoisition = transform.position;
        Vector3 novaPosicao = new Vector3(Movimento.x,0f,Movimento.y);
        Vector3 positionToLookAt = atualPoisition + novaPosicao;
        transform.LookAt(positionToLookAt);
    }
}
