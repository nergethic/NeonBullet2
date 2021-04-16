
using _Config;
using Assets._Scripts.Player.Inventory;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform playerPosition;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Player player;
    [SerializeField] CraftingPanel craftingPanel;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform throwableSpawn;
    [SerializeField] ProjectileManager projectileManager;
    public event Action FootstepEvent;
    public event Action DashEvent;
    public event Action ShootingEvent;
    public event Action ChargeEvent;
    public event Action StopChargeEvent;
    public event Action PickUpEvent;
    public ThrowableItem ThrowableItem { get; set; }
    private Item pickableItem;
    private Resource pickableResource;
    private Controls controls;
    private bool isPause = false;

    private float currentSpeed;

    private void Start() {
        controls.Player.ShowInventory.performed += OnShowInventory;
        controls.Player.ShowInventory.Enable();
        controls.Player.ShowCrafting.performed += OnShowCraftingPanel;
        controls.Player.ShowCrafting.Enable();
        lastPosition = playerPosition.position;
        currentSpeed = player.playerSpeed;
    }

    private void OnEnable() {
        controls = new Controls();
        controls.Player.Fire.performed         += OnFire;
        controls.Player.FireReleased.performed += OnFireReleased;
        controls.Player.Block.performed        += OnBlock;
        controls.Player.Aim.performed          += OnAim;
        controls.Player.Move.performed         += OnMove;
        controls.Player.Dash.performed         += OnDash;
        controls.Player.Select.performed       += OnSelect;
        controls.Player.Back.performed         += OnBack;
        controls.Player.PickUp.performed       += OnPickUp;

        EnableControls();
    }
    
    private void OnDisable() {
        controls.Player.Fire.performed         -= OnFire;
        controls.Player.FireReleased.performed -= OnFireReleased;
        controls.Player.Block.performed        -= OnBlock;
        controls.Player.Aim.performed          -= OnAim;
        controls.Player.Move.performed         -= OnMove;
        controls.Player.Dash.performed         -= OnDash;
        controls.Player.Select.performed       -= OnSelect;
        controls.Player.Back.performed         -= OnBack;
        controls.Player.PickUp.performed       -= OnPickUp;

        DisableControls();
    }

    private void EnableControls()
    {
        controls.Player.Fire.Enable();
        controls.Player.FireReleased.Enable();
        controls.Player.Block.Enable();
        controls.Player.Aim.Enable();
        controls.Player.Move.Enable();
        controls.Player.Dash.Enable();
        controls.Player.Select.Enable();
        controls.Player.Back.Enable();
        controls.Player.PickUp.Enable();
    }

    private void DisableControls()
    {

        controls.Player.Fire.Disable();
        controls.Player.FireReleased.Disable();
        controls.Player.Block.Disable();
        controls.Player.Aim.Disable();
        controls.Player.Move.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Select.Disable();
        controls.Player.Back.Disable();
        controls.Player.PickUp.Disable();
    }

    private void OnDestroy()
    {
        controls.Player.ShowInventory.performed -= OnShowInventory;
        controls.Player.ShowInventory.Disable();
        controls.Player.ShowCrafting.performed -= OnShowCraftingPanel;
        controls.Player.ShowCrafting.Disable();
    }

    [SerializeField] float val;
    private Vector2 velBooster = Vector2.zero;
    private void Update() {
        // mouse

        SetPlayerRotation();
        if (!isPause)
        {
            var mouse = Mouse.current;
            if (mouse.leftButton.isPressed && player.Energy >= 1)
            {
                if (Mathf.Approximately(loadingShot, 0))
                    ChargeEvent();
                loadingShot += 1f * Time.deltaTime;
                currentSpeed = 2f;
            }
            else if (mouse.leftButton.wasReleasedThisFrame)
            {
                if (loadingShot > 0.474f && player.Energy >= 1)
                {
                    player.Energy -= 1;

                    var (bulletGO, bullet) = projectileManager.SpawnProjectile(playerPosition.position, ProjectileType.Standard, true, 4.2f);
                    var bulletDirection = GetCentralizedMousePos().normalized;
                    bullet.SetDirection(bulletDirection);
                    ShootingEvent();
                    velBooster = -bulletDirection * val;
                }
                else
                    StopChargeEvent();

                currentSpeed = player.playerSpeed;
                loadingShot = 0f;
            }

            var keyboard = Keyboard.current;
            if (keyboard.qKey.isPressed && ThrowableItem != null)
            {
                loadingItemAction += 1f * Time.deltaTime;

            }
            else if (keyboard.qKey.wasReleasedThisFrame && ThrowableItem != null)
            {
                ThrowableItem.Throw(loadingItemAction, GetCentralizedMousePos().normalized, throwableSpawn.position);
                ThrowableItem = null;
                loadingItemAction = 0f;
            }
        }
    }

    private void SetPlayerRotation() {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPos = playerPosition.position;
        Vector2 lookDir = mousePos - playerPos;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        rb.rotation = angle;
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
            newPlayerPos.y += 0.5f * ddPlayer.y * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.y * Time.fixedDeltaTime;
        
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
            newPlayerPos.y += 0.5f * ddPlayer.y * Time.fixedDeltaTime * Time.fixedDeltaTime + dPlayer.y * Time.fixedDeltaTime;
        
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
        newCameraPos.y = newPlayerPos.y;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var result = GetCentralizedMousePos();
        
        newCameraPos.x += result.x * 0.4f;
        newCameraPos.y += result.y * 0.4f;
        mainCamera.transform.position = newCameraPos;

    }

    Vector2 GetCentralizedMousePos() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPoint = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane+Mathf.Epsilon));
        Vector3 cameraPos = mainCamera.transform.position;
        
        return mouseWorldPoint - cameraPos;
    }

    private float loadingShot = 0f;
    private float loadingItemAction = 0f;
    private void OnFire(InputAction.CallbackContext context) {
        //if (!context.performed)
            //return;

    }

    private void OnFireReleased(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
        player.PerformBlock();
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
        player.PerformDash();
        DashEvent();
    }
    
    private void OnSelect(InputAction.CallbackContext context) {
        Debug.Log("Select");
    }
    
    private void OnBack(InputAction.CallbackContext context) {
        Debug.Log("Back");
    }
    void OnPickUp(InputAction.CallbackContext context) {
        if (pickableItem != null) {
            player.Inventory.AddItem(pickableItem);
            pickableItem.gameObject.SetActive(false);
            pickableItem = null;
            PickUpEvent();
        }

        if (pickableResource != null)
        {
            pickableResource.AddResource(player.Resources);
            Destroy(pickableResource.gameObject);
            pickableResource = null;
            PickUpEvent();
        }
    }

    void OnShowInventory(InputAction.CallbackContext context) {
        if (player.Inventory.IsInventoryActive) {
            EnableControls();
            player.Inventory.HideInventory();
            isPause = false;
        } else {
            player.Inventory.ShowInventory();

            if (craftingPanel.isActiveAndEnabled)
                craftingPanel.gameObject.SetActive(false);
            else
            {
                isPause = true;
                DisableControls();
            }
        }
    }

    void OnShowCraftingPanel(InputAction.CallbackContext context) {
        if (craftingPanel.isActiveAndEnabled) {
            EnableControls();
            craftingPanel.gameObject.SetActive(false);
            isPause = false;
        } else {
            craftingPanel.gameObject.SetActive(true);
            if (player.Inventory.IsInventoryActive)
                player.Inventory.HideInventory();
            else
            {
                isPause = true;
                DisableControls();
                craftingPanel.Display();
            }
        }
    }

    void OnItemAction(InputAction.CallbackContext context) {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Item"))
            pickableItem = other.GetComponent<Item>();
        else if (other.CompareTag("Resource"))
            pickableResource = other.GetComponent<Resource>();
        else if (other.CompareTag("Projectile")) {
            var projectile = other.GetComponent<Projectile>();
            if (projectile != null)
                if (!projectile.projectileData.ownedByPlayer) 
                    player.HandleProjectile(projectile);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Item"))
            pickableItem = null;
        if (other.CompareTag("Resource"))
            pickableResource = null;
    }
}

