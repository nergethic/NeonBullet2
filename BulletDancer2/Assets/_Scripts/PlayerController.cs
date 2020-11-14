using System;
using System.Collections;
using System.Collections.Generic;
using _Config;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform playerPosition;
    [SerializeField] float playerSpeed = 1.0f;
    [SerializeField] GameObject bulletPrefab;
    private Controls controls;

    private void OnEnable() {
        controls = new Controls();
        controls.Player.Fire.performed   += OnFire;
        controls.Player.Aim.performed    += OnAim;
        controls.Player.Move.performed   += OnMove;
        controls.Player.Dash.performed   += OnDash;
        controls.Player.Select.performed += OnSelect;
        controls.Player.Back.performed   += OnBack;
        
        controls.Player.Fire.Enable();
        controls.Player.Aim.Enable();
        controls.Player.Move.Enable();
        controls.Player.Dash.Enable();
        controls.Player.Select.Enable();
        controls.Player.Back.Enable();
    }
    
    private void OnDisable() {
        controls.Player.Fire.performed   -= OnFire;
        controls.Player.Aim.performed    -= OnAim;
        controls.Player.Move.performed   -= OnMove;
        controls.Player.Dash.performed   -= OnDash;
        controls.Player.Select.performed -= OnSelect;
        controls.Player.Back.performed   -= OnBack;
        
        controls.Player.Fire.Disable();
        controls.Player.Aim.Disable();
        controls.Player.Move.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Select.Disable();
        controls.Player.Back.Disable();
    }

    // Update is called once per frame
    void Update() {
        Vector3 newPlayerPos = playerPosition.position;
        Vector2 moveValue = controls.Player.Move.ReadValue<Vector2>();

        newPlayerPos.x += Time.deltaTime * moveValue.x * playerSpeed;
        newPlayerPos.z += Time.deltaTime * moveValue.y * playerSpeed;
        
        playerPosition.position = newPlayerPos;
        
        Vector3 newCameraPos = mainCamera.transform.position;
        newCameraPos.x = newPlayerPos.x;
        newCameraPos.z = newPlayerPos.z;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        var result = GetCentralizedMousePos();
        
        newCameraPos.x += result.x * 0.4f;
        newCameraPos.z += result.y * 0.4f;
        mainCamera.transform.position = newCameraPos;
    }

    Vector2 GetCentralizedMousePos() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var result = mainCamera.ScreenToViewportPoint(mousePosition);
        
        // TODO: clamp vector
        if (result.x < 0.0f)
            result.x = 0.0f;
        if (result.x > 1.0f)
            result.x = 1.0f;
        if (result.y < 0.0f)
            result.y = 0.0f;
        if (result.y > 1.0f)
            result.y = 1.0f;
        
        result.x -= 0.5f;
        result.y -= 0.5f;

        return result;
    }

    private void OnFire(InputAction.CallbackContext context) {
        Debug.Log("Fire");
        var bullet = Instantiate(bulletPrefab).GetComponent<Projectile>();
        bullet.Initialize(GetCentralizedMousePos());
        bullet.transform.position = playerPosition.position;
    }
    
    private void OnAim(InputAction.CallbackContext context) {
        Debug.Log("Aim");
    }
    
    private void OnMove(InputAction.CallbackContext context) {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log(value);
    }
    
    private void OnDash(InputAction.CallbackContext context) {
        Debug.Log("Dash");
    }
    
    private void OnSelect(InputAction.CallbackContext context) {
        Debug.Log("Select");
    }
    
    private void OnBack(InputAction.CallbackContext context) {
        Debug.Log("Back");
    }
}
