using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _gravity;

    //Ref vars
    private PlayerActions _playerActions;
    private CharacterController _controller;

    //Help vars
    private Vector3 _movement;
    private bool _jumping;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        InitializeActions();        
    }

    private void InitializeActions()
    {
        _playerActions = new PlayerActions();
        _playerActions.PlayerMap.Enable();

        _playerActions.PlayerMap.Move.started += Move_performed;
        _playerActions.PlayerMap.Move.performed += Move_performed;
        _playerActions.PlayerMap.Move.canceled += Move_performed;
        _playerActions.PlayerMap.Jump.performed += Jump_performed;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _movement.x = 0;
            _movement.z = 0;
            return;
        }

        _movement.x = context.ReadValue<Vector2>().x;
        _movement.z = context.ReadValue<Vector2>().y;
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        _jumping = true;
        _movement.y = _jumpPower;
    }

    private void FixedUpdate()
    {
        if (_controller.isGrounded)
            _movement.y = -0.005f;
        else
            _movement.y -= _gravity * Time.fixedDeltaTime;
        
        _controller.Move(Time.fixedDeltaTime * _speed * _movement);
    }

}
