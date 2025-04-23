using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidadMov;
    [SerializeField] private float gravityFactor;
    [SerializeField] private Transform camara; //enfocar cam con la dirección de mov del player
    [SerializeField] private InputManagerS0 inputManager;
    [SerializeField] private float jumpHeight;

    [Header("Floor Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float radiusDetection;
    [SerializeField] private LayerMask whatisFloor;


    private CharacterController controller;
    private Animator anim;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;
    private Vector3 verticalVelocity;
    private AudioSource audioSteps;


    private void OnEnable()
    {
        inputManager.OnJump += Jump;
        inputManager.OnMove += Move;
    }



    // solo cuando se actualize el input
    private void Move(Vector2 ctx)
    {
        audioSteps.Play();
        direccionInput = new Vector3(ctx.x,0, ctx.y);
    }

    private void Jump()
    {
        //para movimientos cinemáticos hacia arriba
        if (OnFloor())
        {
            verticalVelocity.y = Mathf.Sqrt(-2 * gravityFactor * jumpHeight);
            anim.SetTrigger("jump");
        }

    }



    void Start()
    {
      //  Cursor.lockState = CursorLockMode.Locked; // que no se vea el ratón 
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSteps = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //movimiento de cámara e input
        direccionMovimiento = camara.forward*direccionInput.z + camara.right*direccionInput.x;
        direccionMovimiento.y = 0;
        controller.Move(direccionMovimiento * velocidadMov * Time.deltaTime);



        anim.SetFloat("velocidad",controller.velocity.magnitude); // esto es para pasar la velocidad que llevo al momento (para que no sea 0 o 5 solo) EN TECLADO NO SE VE 

        if (direccionMovimiento.sqrMagnitude > 0) // si mi velocidad es superior a 0
        {
            RotarHaciaDestino();
        }
        else
        {
            audioSteps.Stop();
        }

        //si hemos aterrizado
        if (OnFloor() && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0;
            anim.ResetTrigger("jump"); //reseta para no tener "lag" al pulsar muchas veces un botón y que despues de dejar de pulsar siga haciendose la animación 
        }


            ApplyGrav();
      
        
    }

    private void ApplyGrav()
    {
        verticalVelocity.y += gravityFactor * Time.deltaTime;
        controller.Move(verticalVelocity* Time.deltaTime); //gravedad segundos^2
    }


    private bool OnFloor()
    {
        return Physics.CheckSphere(feet.position, radiusDetection, whatisFloor);
    }

    private void RotarHaciaDestino() 
    {
        Quaternion rotacionObj = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = rotacionObj;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feet.position, radiusDetection);
    }
}
