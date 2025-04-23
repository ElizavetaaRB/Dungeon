using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerS0 : ScriptableObject
{
    PlayerInputActions misControles;
    public event Action OnJump;
    public event Action<Vector2> OnMove;
    public event Action OnPause;

    private void OnEnable()
    {
        if (misControles == null)
        {
            misControles = new PlayerInputActions();
        }
        misControles.Player.Enable(); //activar las acciones del jugador (mover,saltar)

        //suscribimos
        misControles.Player.Jump.performed += Jump;
        misControles.Player.Move.performed += Move;
        misControles.Player.UI.performed += Pause;


        misControles.Player.Move.canceled += Move;
        misControles.Player.Move.canceled += Jump;
        misControles.Player.UI.canceled += Pause;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        //   Debug.Log(ctx.ReadValue<Vector2>()); ver el valor de ctx(contexto)
        OnMove?.Invoke(ctx.ReadValue<Vector2>());

    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }


    private void Pause(InputAction.CallbackContext ctx)
    {
        OnPause?.Invoke();
    }

    private void OnDisable()
    {
        misControles.Player.Jump.performed -= Jump;
        misControles.Player.Move.performed -= Move;
        misControles.Player.UI.performed -= Pause;

        if (misControles != null)
        {
            misControles.Player.Disable();
        }
    }


    private void OnDestroy()
    {
        misControles.Player.Jump.performed -= Jump;
        misControles.Player.Move.performed -= Move;
        misControles.Player.UI.performed -= Pause;

        if (misControles != null)
        {
            misControles.Player.Disable();
        }
        misControles?.Dispose(); // Liberar recursos
    }
}
