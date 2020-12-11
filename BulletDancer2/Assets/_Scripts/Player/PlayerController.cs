
using _Config;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform playerPosition;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Player player;
    public event Action FootstepEvent;
    public event Action DashEvent;

    private Item pickableItem;
    private Controls controls;

    private float currentSpeed;

    private void Start() {
        controls.Player.ShowInventory.performed += OnShowInventory;
        controls.Player.ShowInventory.Enable();
        lastPosition = playerPosition.position;
        currentSpeed = player.playerSpeed;
    }

    private void OnEnable() {
        controls = new Controls();
        controls.Player.Fire.performed         += OnFire;
        controls.Player.FireReleased.performed += OnFireReleased;
        controls.Player.Aim.performed          += OnAim;
        controls.Player.Move.performed         += OnMove;
        controls.Player.Dash.performed         += OnDash;
        controls.Player.Select.performed       += OnSelect;
        controls.Player.Back.performed         += OnBack;
        controls.Player.PickUp.performed       += OnPickUp;
        
        controls.Player.Fire.Enable();
        controls.Player.FireReleased.Enable();
        controls.Player.Aim.Enable();
        controls.Player.Move.Enable();
        controls.Player.Dash.Enable();
        controls.Player.Select.Enable();
        controls.Player.Back.Enable();
        controls.Player.PickUp.Enable();
    }
    
    private void OnDisable() {
        controls.Player.Fire.performed         -= OnFire;
        controls.Player.FireReleased.performed -= OnFireReleased;
        controls.Player.Aim.performed          -= OnAim;
        controls.Player.Move.performed         -= OnMove;
        controls.Player.Dash.performed         -= OnDash;
        controls.Player.Select.performed       -= OnSelect;
        controls.Player.Back.performed         -= OnBack;
        controls.Player.PickUp.performed       -= OnPickUp;

        controls.Player.Fire.Disable();
        controls.Player.FireReleased.Disable();
        controls.Player.Aim.Disable();
        controls.Player.Move.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Select.Disable();
        controls.Player.Back.Disable();
        controls.Player.PickUp.Disable();
    }

    [SerializeField] float val;
    private Vector2 velBooster = Vector2.zero;
    private void Update() {
        // mouse
        var mouse = Mouse.current;
        if (mouse.leftButton.isPressed && player.Energy >= 1) {
            loadingShot += 1f*Time.deltaTime;
            currentSpeed = 2f;
        } else if (mouse.leftButton.wasReleasedThisFrame) {
            if (loadingShot > 1f && player.Energy >= 1) {
                player.Energy -= 1;
                var bullet = Instantiate(bulletPrefab).GetComponent<Projectile>();
                var bulletDirection = GetCentralizedMousePos();
                bullet.Initialize(bulletDirection, true, Projectile.ProjectileType.Standard);
                bullet.transform.position = playerPosition.position;

                velBooster = -bulletDirection * val;
            }

            currentSpeed = player.playerSpeed;
            loadingShot = 0f;
        }
    }

    Vector2 dPlayer = Vector2.zero; // velocity
    Vector2 ddPlayer = Vector2.zero; // acceleration
    Vector3 lastPosition = Vector3.zero;

    Vector2 lastMovementDirection;
    void FixedUpdate() {
        Vector3 newPlayerPos = playerPosition.position;
        
        lastMovementDirection = controls.Player.Move.ReadValue<Vector2>();
        if (player.isDashing) {
            ddPlayer = lastMovementDirection * player.dashSpeed;

            newPlayerPos.x += 0.5f * ddPlayer.x * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.x * Time.fixedDeltaTime;
            newPlayerPos.z += 0.5f * ddPlayer.y * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.y * Time.fixedDeltaTime;
        
            dPlayer += 10f*ddPlayer * Time.fixedDeltaTime;
            dPlayer = Vector2.ClampMagnitude(dPlayer, 3f);
        } else {
            ddPlayer = lastMovementDirection * currentSpeed;

            ddPlayer += velBooster;
            
            if (velBooster != Vector2.zero) {
                velBooster -= 40.0f*velBooster*Time.fixedDeltaTime;
                if (velBooster.sqrMagnitude < 0.01f)
                    velBooster = Vector2.zero;
            }

            ddPlayer += -4.5f * dPlayer;
            
            newPlayerPos.x += 0.5f * ddPlayer.x * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.x * Time.fixedDeltaTime;
            newPlayerPos.z += 0.5f * ddPlayer.y * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.y * Time.fixedDeltaTime;
        
            dPlayer += ddPlayer * Time.fixedDeltaTime;
            dPlayer = Vector2.ClampMagnitude(dPlayer, 1f);
        }
        
        
        //dPlayer.x -= Time.fixedDeltaTime * 1.5f;
        //dPlayer.y -= Time.fixedDeltaTime * 1.5f;
        //if (dPlayer.magnitude < 0f)
            //dPlayer = Vector2.zero;
        
        playerPosition.position = newPlayerPos;

        var diff = (lastPosition - newPlayerPos).magnitude;
        if (diff > 0.3f) {
            lastPosition = newPlayerPos;
            FootstepEvent();
        }

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

    private float loadingShot = 0f;
    private void OnFire(InputAction.CallbackContext context) {
        //if (!context.performed)
            //return;

    }

    private void OnFireReleased(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
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
        player.Dash();
        DashEvent();
    }
    
    private void OnSelect(InputAction.CallbackContext context) {
        Debug.Log("Select");
    }
    
    private void OnBack(InputAction.CallbackContext context) {
        Debug.Log("Back");
    }
    void OnPickUp(InputAction.CallbackContext context)
    {
        if (pickableItem != null)
        {
            player.Inventory.AddItem(pickableItem);
            pickableItem.gameObject.SetActive(false);
            pickableItem = null;
        }

    }

    void OnShowInventory(InputAction.CallbackContext context)
    {
        if (player.Inventory.IsInventoryActive)
        {
            OnEnable();
            player.Inventory.HideInventory();
        }
        else
        {
            OnDisable();
            player.Inventory.ShowInventory();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) {
            pickableItem = other.GetComponent<Item>();
        } else if (other.CompareTag("Projectile")) {
            var projectile = other.GetComponent<Projectile>();
            if (projectile != null) {
                var projectileData = projectile.projectileData;
                if (!projectileData.ownedByPlayer) {
                    if (player.isAbsorbingEnergy && projectileData.typeMask == (int)Projectile.ProjectileType.Energy) {
                        int newEnergy = player.Energy + 1;
                        if (newEnergy <= player.MaxEnergy)
                            player.Energy = newEnergy;
                    } else if (!player.isImmuneToDamage) {
                        player.PlayerHitByProjectileAction(ref projectileData);
                    }
                    Destroy(projectile.gameObject);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            pickableItem = null;
        }
    }
}

