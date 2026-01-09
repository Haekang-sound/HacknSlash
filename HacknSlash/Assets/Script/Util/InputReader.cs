using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static InputSystem_Actions;

public class InputReader : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 moveComposite;
    public Vector2 mousePos;

    public event Action onMove;
    public event Action offMove;

    public event Action onJump;
    public event Action offJump;

    public event Action onCrouch;
    public event Action offCrouch;

    public event Action onAttack;
    public event Action offAttack;

    public event Action onMagic;



    static InputReader instance;

    private InputSystem_Actions Inputs;

    public void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (Inputs != null)
        {
            Inputs.Player.Enable();
            return;
        }
        Inputs = new InputSystem_Actions();
        Inputs.Player.SetCallbacks(this); // InputReader는 IPlayerActions를 상속받았다.
                                          // Actions을 세팅한다.
        Inputs.Player.Enable();           // 사용가능한 형태로 만든다.
    }

    public void OnDisable()
    {

    }
    public bool IsMovehPressed()
    {
        return Mathf.Abs(Inputs.Player.Move.ReadValue<Vector2>().x) > float.Epsilon;
    }
    public bool IsJumpPressed()
    {
        return Mathf.Abs(Inputs.Player.Jump.ReadValue<float>()) > float.Epsilon;
    }

    public bool IsCrouchPressed()
    {
        return Inputs.Player.Crouch.ReadValue<float>() > float.Epsilon;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) { onAttack?.Invoke(); }
        //if(context.performed) { }
        if (context.canceled) { offAttack?.Invoke(); }

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started) { onCrouch?.Invoke(); }
        //if (context.performed) { onCrouch?.Invoke(); }
        if (context.canceled) { offCrouch?.Invoke(); }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) { onJump?.Invoke(); }
        //if(context.performed) { }
        if (context.canceled) { offJump?.Invoke(); }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveComposite = context.ReadValue<Vector2>();

        if (context.started) { onMove?.Invoke(); }
        //if(context.performed) { }
        if (context.canceled) { offMove?.Invoke(); }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnBulletTime(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnShootMagic(InputAction.CallbackContext context)
    {
        if (context.started) { onMagic?.Invoke(); }
        //if(context.performed) { }
        //if (context.canceled) { onMagic?.Invoke(); }
    }
}
