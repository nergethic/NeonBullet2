
using _Config;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform playerPosition;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Player player;
    
    private Item pickableItem;
    private Controls controls;

    private void Start()
    {
        controls.Player.ShowInventory.performed += OnShowInventory;
        controls.Player.ShowInventory.Enable();
    }

    private void OnEnable() {
        controls = new Controls();
        controls.Player.Fire.performed   += OnFire;
        controls.Player.Aim.performed    += OnAim;
        controls.Player.Move.performed   += OnMove;
        controls.Player.Dash.performed   += OnDash;
        controls.Player.Select.performed += OnSelect;
        controls.Player.Back.performed   += OnBack;
        controls.Player.PickUp.performed += OnPickUp;
        
        controls.Player.Fire.Enable();
        controls.Player.Aim.Enable();
        controls.Player.Move.Enable();
        controls.Player.Dash.Enable();
        controls.Player.Select.Enable();
        controls.Player.Back.Enable();
        controls.Player.PickUp.Enable();
    }
    
    private void OnDisable() {
        controls.Player.Fire.performed   -= OnFire;
        controls.Player.Aim.performed    -= OnAim;
        controls.Player.Move.performed   -= OnMove;
        controls.Player.Dash.performed   -= OnDash;
        controls.Player.Select.performed -= OnSelect;
        controls.Player.Back.performed   -= OnBack;
        controls.Player.PickUp.performed -= OnPickUp;

        controls.Player.Fire.Disable();
        controls.Player.Aim.Disable();
        controls.Player.Move.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Select.Disable();
        controls.Player.Back.Disable();
        controls.Player.PickUp.Disable();
    }

    Vector2 lastMovementDirection;
    void FixedUpdate() {
        Vector3 newPlayerPos = playerPosition.position;
        Vector2 movementValue;

        if (player.isDashing) {
            movementValue = lastMovementDirection * Time.deltaTime * player.dashSpeed;

            newPlayerPos.x += movementValue.x;
            newPlayerPos.z += movementValue.y;
        } else {
            lastMovementDirection = controls.Player.Move.ReadValue<Vector2>();
            movementValue = lastMovementDirection * Time.fixedDeltaTime*player.playerSpeed;

            newPlayerPos.x += movementValue.x;
            newPlayerPos.z += movementValue.y;
        }
        
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
        if (!context.performed)
            return;
        
        if (player.Energy >= 1) {
            player.Energy -= 1;
            var bullet = Instantiate(bulletPrefab).GetComponent<Projectile>();
            bullet.Initialize(GetCentralizedMousePos(), true, Projectile.ProjectileType.Standard);
            bullet.transform.position = playerPosition.position;
        }
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

