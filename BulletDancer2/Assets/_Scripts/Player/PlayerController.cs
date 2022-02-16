using _Config;
using Assets._Scripts.Player.Inventory;
using Assets._Scripts.Player.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] MainCameraController mainCameraController;
    [SerializeField] Transform playerPosition;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Player player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform throwableSpawn;
    [SerializeField] ProjectileManager projectileManager;
    [SerializeField] float val;
    UIManager UIManager;

    public Camera GetCamera() => mainCameraController.GetCamera();
    public MainCameraController GetMainCameraController() => mainCameraController;
    public Vector2 Acceleration => ddPlayer;
    public SpriteRenderer weapon;
    public event Action FootstepEvent;
    public event Action DashEvent;
    public event Action ShootingEvent;
    public event Action ChargeEvent;
    public event Action StopChargeEvent;
    public event Action PickUpEvent;
    public ThrowableItem ThrowableItem { get; set; }
    public Weapon activeWeapon;
 
    Controls controls;
    bool isPause;
    
    Vector2 dPlayer = Vector2.zero; // velocity
    Vector2 ddPlayer = Vector2.zero; // acceleration
    Vector3 lastPosition = Vector3.zero;
    Vector2 lastMovementDirection;
    float currentSpeed;

    public void InitializeUIManager(UIManager UIManager) => this.UIManager = UIManager;

    void Start() {
        controls.Player.ShowInventory.performed += OnShowInventory;
        controls.Player.ShowInventory.Enable();
        controls.Player.ShowCrafting.performed += OnShowCraftingPanel;
        controls.Player.ShowCrafting.Enable();
        controls.Player.ShowMenu.performed += OnShowMenu;
        controls.Player.ShowMenu.Enable();
        lastPosition = playerPosition.position;
        currentSpeed = player.playerSpeed;

        // check why item system didn't init item
        if (activeWeapon != null)
            HandleActiveWeapon();
    }

    void OnEnable() {
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
        controls.Player.Teleport.performed     += OnTeleport;

        EnableControls();
    }

    private void OnTeleport(InputAction.CallbackContext obj)
    {
        gameObject.transform.position = new Vector2(0, 0);
    }

    void OnDisable() {
        controls.Player.Fire.performed         -= OnFire;
        controls.Player.FireReleased.performed -= OnFireReleased;
        controls.Player.Block.performed        -= OnBlock;
        controls.Player.Aim.performed          -= OnAim;
        controls.Player.Move.performed         -= OnMove;
        controls.Player.Dash.performed         -= OnDash;
        controls.Player.Select.performed       -= OnSelect;
        controls.Player.Back.performed         -= OnBack;
        controls.Player.PickUp.performed       -= OnPickUp;
        controls.Player.Teleport.performed     -= OnTeleport;

        DisableControls();
    }

    void EnableControls() {
        controls.Player.Fire.Enable();
        controls.Player.FireReleased.Enable();
        controls.Player.Block.Enable();
        controls.Player.Aim.Enable();
        controls.Player.Move.Enable();
        controls.Player.Dash.Enable();
        controls.Player.Select.Enable();
        controls.Player.Back.Enable();
        controls.Player.PickUp.Enable();
        controls.Player.Teleport.Enable();
    }

    void DisableControls() {
        controls.Player.Fire.Disable();
        controls.Player.FireReleased.Disable();
        controls.Player.Block.Disable();
        controls.Player.Aim.Disable();
        controls.Player.Move.Disable();
        controls.Player.Dash.Disable();
        controls.Player.Select.Disable();
        controls.Player.Back.Disable();
        controls.Player.PickUp.Disable();
        controls.Player.Teleport.Disable();
    }

    private void OnDestroy() {
        controls.Player.ShowInventory.performed -= OnShowInventory;
        controls.Player.ShowInventory.Disable();
        controls.Player.ShowCrafting.performed -= OnShowCraftingPanel;
        controls.Player.ShowMenu.performed -= OnShowMenu;
        controls.Player.ShowMenu.Disable();
    }
    
    Vector2 velBooster = Vector2.zero;
    void Update() {
        SetPlayerRotation();
        if (isPause)
            return;

        HandleMouseInput();
        HandleKeyboardInput();
    }

    void HandleKeyboardInput() {
        var keyboard = Keyboard.current;
        if (keyboard.qKey.isPressed && ThrowableItem != null) {
            loadingItemAction += 1f * Time.deltaTime;
        } else if (keyboard.qKey.wasReleasedThisFrame && ThrowableItem != null) {
            ThrowableItem.Throw(loadingItemAction, GetCentralizedMousePos().normalized, throwableSpawn.position);
            ThrowableItem = null;
            loadingItemAction = 0f;
        }

        if (keyboard.qKey.isPressed) {
            //var x = FindObjectOfType<BulletBoss>();
            //if (x != null) {
                //playerPosition.position = x.transform.position + (x.transform.right*2f);
            //}
            
            var masterSystem = FindObjectOfType<MasterSystem>(); // TODO: spawn player from master system
            if (masterSystem != null)
                masterSystem.LoadNextLevel();
        }
    }

    void HandleMouseInput() {
        var mouse = Mouse.current;
        if (mouse.leftButton.isPressed && player.Energy >= 1 && activeWeapon != null) {
            if (Mathf.Approximately(loadingShot, 0))
                ChargeEvent?.Invoke();
            loadingShot += 1f * Time.deltaTime;
            currentSpeed = 2f;
        } else if (mouse.leftButton.wasReleasedThisFrame) {
            if (loadingShot > 0.474f && player.Energy >= 1) {
                player.Energy -= 1;
                {
                    var bulletDirection = GetCentralizedMousePos().normalized;
                    activeWeapon.Shoot(bulletDirection);
                    ShootingEvent?.Invoke();
                    velBooster = -bulletDirection * val;
                }
            }
            else
                StopChargeEvent?.Invoke();

            currentSpeed = player.playerSpeed;
            loadingShot = 0f;
        }
    }

    void SetPlayerRotation() {
        Vector2 mousePos = mainCameraController.GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPos = playerPosition.position;
        Vector2 lookDir = mousePos - playerPos;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        rb.rotation = angle;
    }
    
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

        playerPosition.position = new Vector3(newPlayerPos.x, newPlayerPos.y, 0f);

        var diff = (lastPosition - newPlayerPos).magnitude;
        if (diff > 0.3f) {
            lastPosition = newPlayerPos;
            FootstepEvent();
        }

        var camera = mainCameraController.GetCamera();
        Vector3 newCameraPos = camera.transform.position;
        newCameraPos.x = newPlayerPos.x;
        newCameraPos.y = newPlayerPos.y;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var result = GetCentralizedMousePos();
        
        newCameraPos.x += result.x * 0.4f;
        newCameraPos.y += result.y * 0.4f;
        camera.transform.position = newCameraPos;
    }

    Vector2 GetCentralizedMousePos() {
        var camera = mainCameraController.GetCamera();
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPoint = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane+Mathf.Epsilon));
        Vector3 cameraPos = camera.transform.position;
        
        return mouseWorldPoint - cameraPos;
    }

    float loadingShot = 0f;
    float loadingItemAction = 0f;
    void OnFire(InputAction.CallbackContext context) {
        //if (!context.performed)
            //return;

    }

    void OnFireReleased(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
    }

    void OnBlock(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
        player.PerformBlock();
    }
    
    void OnAim(InputAction.CallbackContext context) {
        Debug.Log("Aim");
    }
    
    void OnMove(InputAction.CallbackContext context) {
        Vector2 value = context.ReadValue<Vector2>();
        // Debug.Log(value);
    }
    
    void OnDash(InputAction.CallbackContext context) {
        Debug.Log("Dash");
        if (player.PerformDash())
            DashEvent();
    }
    
    void OnSelect(InputAction.CallbackContext context) {
        Debug.Log("Select");
    }
    
    void OnBack(InputAction.CallbackContext context) {
        Debug.Log("Back");
    }
    
    void OnPickUp(InputAction.CallbackContext context) {
        if (player.PickUp())
            PickUpEvent();
    }

    void OnShowInventory(InputAction.CallbackContext context) {
        HandleUIAction(PanelType.Inventory);
    }

    void OnShowCraftingPanel(InputAction.CallbackContext context) {
        HandleUIAction(PanelType.Crafting);
    }

    void OnShowMenu(InputAction.CallbackContext context)
    {
        HandleUIAction(PanelType.Options);
    }

    void OnItemAction(InputAction.CallbackContext context) {}


    private void HandleUIAction(PanelType type)
    {
        if (UIManager == null)
            return;
        if (UIManager.IsPanelActive)
        {
            if (UIManager.ActivePanel.panelType == type)
            {
                HidePanel();
            }
            else
            {
                UIManager.HidePanel();
                ShowPanel(type);
            }
        }
        else
        {
            ShowPanel(type);
        }
    }

    private void HidePanel()
    {
        EnableControls();
        UIManager.HidePanel();
        isPause = false;
    }

    private void ShowPanel(PanelType type)
    {
        UIManager.ShowPanel(type);
        DisableControls();
        isPause = true;
    }
    
    void HandleActiveWeapon() {
        activeWeapon.Initialize(player, this, projectileManager);
        activeWeapon.SetCollider(false);
        player.Inventory.AddItem(activeWeapon);
        weapon.sprite = activeWeapon.topView;
        activeWeapon.SetButtonStatus(activeWeapon, true);
        activeWeapon.SpriteRenderer.sprite = null;
    }
}