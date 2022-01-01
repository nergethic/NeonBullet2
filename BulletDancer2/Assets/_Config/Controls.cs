// GENERATED AUTOMATICALLY FROM 'Assets/_Config/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace _Config
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""75b07fed-8a77-41b9-83ae-6b63e5c71b41"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f8f84d31-df8c-4e0b-b575-d758c8361fdc"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FireReleased"",
                    ""type"": ""Button"",
                    ""id"": ""a7a93e81-5aff-41ac-816e-3c5b6cbf6136"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""2a7de4be-4875-4f05-a0f7-0436806a8fc0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""c1d4c8c2-ea62-407b-bb20-b544cdc5f014"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""1147cfda-5924-4d96-b432-1d41bbef3b39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""89b6b69f-6d61-4c99-9a88-589b06640f9d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""41a33e05-c966-402a-a5cb-4ee9b7317f0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PickUp"",
                    ""type"": ""Button"",
                    ""id"": ""c36e4aba-2ef7-4083-bb8c-a79494c65944"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowInventory"",
                    ""type"": ""Button"",
                    ""id"": ""46a8aaa0-74d8-4f99-8507-7e4a74a3de28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowCrafting"",
                    ""type"": ""Button"",
                    ""id"": ""2e5ff549-2da7-4c9c-9c95-eb30ce65bfc0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemAction"",
                    ""type"": ""Button"",
                    ""id"": ""f609d5ab-a66b-4d27-b8be-f274b19ee003"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""b531fb92-6fbd-4047-a9cf-64d0013581eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Teleport"",
                    ""type"": ""Button"",
                    ""id"": ""e39b1a45-b365-402b-957c-b65831330e25"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowMenu"",
                    ""type"": ""Button"",
                    ""id"": ""17324e12-f4da-4977-b5a3-714f472ff06e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b3ea0b68-5ad9-48db-be44-07b05b372a96"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ab4c1dc-0a47-46de-9234-23abe908c66c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a310dd8-570f-47b3-a6a4-b6dd76aa8e5a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9540c11e-1a89-4241-882c-cdf693233dd1"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce2e4a23-b9f5-42c3-887a-98b5b0a2603e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd3073f8-dd47-4171-9ccb-694d2ff64cd9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1bf7ae7d-a83c-4d06-af38-42fe6cb6ea97"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1685620f-0485-4cd1-9dee-3221875107dc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1140b61e-d109-49f9-bd7a-ca4b4ffcb628"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""57e3acf5-15e7-4021-8ae1-1883431cc29c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a467d394-a45c-4ed9-8355-b73a57e1ca6f"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0873150b-0afe-4f74-b0dd-ae34d158b4ff"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2884e48a-c075-47f1-bbb9-48537b994a90"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b588253-5a59-44f0-b8c2-76137db870e2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dff4da2a-da14-4c5c-b747-6a52653efd3d"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowCrafting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b75c1da-f6d8-4fde-bd1f-fa0ade8b0e97"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f24f713f-f799-4613-a413-4c8464ef7cf0"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a52d8f61-a390-4b84-af05-e00d9e915b30"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Teleport"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b06977dd-7551-4cf4-ad9b-7ff7307830d3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""d82ad8ac-436e-485a-b284-1d04ffa923fc"",
            ""actions"": [
                {
                    ""name"": ""ButtonClick"",
                    ""type"": ""Button"",
                    ""id"": ""7ce0d072-d653-441e-8823-43d294ee3caf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4d2db25-10a6-42b3-884d-58a91d918dfd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
            m_Player_FireReleased = m_Player.FindAction("FireReleased", throwIfNotFound: true);
            m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
            m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
            m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Back = m_Player.FindAction("Back", throwIfNotFound: true);
            m_Player_PickUp = m_Player.FindAction("PickUp", throwIfNotFound: true);
            m_Player_ShowInventory = m_Player.FindAction("ShowInventory", throwIfNotFound: true);
            m_Player_ShowCrafting = m_Player.FindAction("ShowCrafting", throwIfNotFound: true);
            m_Player_ItemAction = m_Player.FindAction("ItemAction", throwIfNotFound: true);
            m_Player_Block = m_Player.FindAction("Block", throwIfNotFound: true);
            m_Player_Teleport = m_Player.FindAction("Teleport", throwIfNotFound: true);
            m_Player_ShowMenu = m_Player.FindAction("ShowMenu", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_ButtonClick = m_UI.FindAction("ButtonClick", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Fire;
        private readonly InputAction m_Player_FireReleased;
        private readonly InputAction m_Player_Aim;
        private readonly InputAction m_Player_Dash;
        private readonly InputAction m_Player_Select;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Back;
        private readonly InputAction m_Player_PickUp;
        private readonly InputAction m_Player_ShowInventory;
        private readonly InputAction m_Player_ShowCrafting;
        private readonly InputAction m_Player_ItemAction;
        private readonly InputAction m_Player_Block;
        private readonly InputAction m_Player_Teleport;
        private readonly InputAction m_Player_ShowMenu;
        public struct PlayerActions
        {
            private @Controls m_Wrapper;
            public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Fire => m_Wrapper.m_Player_Fire;
            public InputAction @FireReleased => m_Wrapper.m_Player_FireReleased;
            public InputAction @Aim => m_Wrapper.m_Player_Aim;
            public InputAction @Dash => m_Wrapper.m_Player_Dash;
            public InputAction @Select => m_Wrapper.m_Player_Select;
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Back => m_Wrapper.m_Player_Back;
            public InputAction @PickUp => m_Wrapper.m_Player_PickUp;
            public InputAction @ShowInventory => m_Wrapper.m_Player_ShowInventory;
            public InputAction @ShowCrafting => m_Wrapper.m_Player_ShowCrafting;
            public InputAction @ItemAction => m_Wrapper.m_Player_ItemAction;
            public InputAction @Block => m_Wrapper.m_Player_Block;
            public InputAction @Teleport => m_Wrapper.m_Player_Teleport;
            public InputAction @ShowMenu => m_Wrapper.m_Player_ShowMenu;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                    @FireReleased.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireReleased;
                    @FireReleased.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireReleased;
                    @FireReleased.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireReleased;
                    @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                    @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                    @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Back.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                    @Back.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                    @Back.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBack;
                    @PickUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @PickUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @PickUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                    @ShowInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowInventory;
                    @ShowInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowInventory;
                    @ShowInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowInventory;
                    @ShowCrafting.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowCrafting;
                    @ShowCrafting.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowCrafting;
                    @ShowCrafting.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowCrafting;
                    @ItemAction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemAction;
                    @ItemAction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemAction;
                    @ItemAction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemAction;
                    @Block.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBlock;
                    @Block.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBlock;
                    @Block.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBlock;
                    @Teleport.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTeleport;
                    @Teleport.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTeleport;
                    @Teleport.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTeleport;
                    @ShowMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowMenu;
                    @ShowMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowMenu;
                    @ShowMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShowMenu;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                    @FireReleased.started += instance.OnFireReleased;
                    @FireReleased.performed += instance.OnFireReleased;
                    @FireReleased.canceled += instance.OnFireReleased;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Back.started += instance.OnBack;
                    @Back.performed += instance.OnBack;
                    @Back.canceled += instance.OnBack;
                    @PickUp.started += instance.OnPickUp;
                    @PickUp.performed += instance.OnPickUp;
                    @PickUp.canceled += instance.OnPickUp;
                    @ShowInventory.started += instance.OnShowInventory;
                    @ShowInventory.performed += instance.OnShowInventory;
                    @ShowInventory.canceled += instance.OnShowInventory;
                    @ShowCrafting.started += instance.OnShowCrafting;
                    @ShowCrafting.performed += instance.OnShowCrafting;
                    @ShowCrafting.canceled += instance.OnShowCrafting;
                    @ItemAction.started += instance.OnItemAction;
                    @ItemAction.performed += instance.OnItemAction;
                    @ItemAction.canceled += instance.OnItemAction;
                    @Block.started += instance.OnBlock;
                    @Block.performed += instance.OnBlock;
                    @Block.canceled += instance.OnBlock;
                    @Teleport.started += instance.OnTeleport;
                    @Teleport.performed += instance.OnTeleport;
                    @Teleport.canceled += instance.OnTeleport;
                    @ShowMenu.started += instance.OnShowMenu;
                    @ShowMenu.performed += instance.OnShowMenu;
                    @ShowMenu.canceled += instance.OnShowMenu;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_ButtonClick;
        public struct UIActions
        {
            private @Controls m_Wrapper;
            public UIActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @ButtonClick => m_Wrapper.m_UI_ButtonClick;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @ButtonClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonClick;
                    @ButtonClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonClick;
                    @ButtonClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonClick;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ButtonClick.started += instance.OnButtonClick;
                    @ButtonClick.performed += instance.OnButtonClick;
                    @ButtonClick.canceled += instance.OnButtonClick;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface IPlayerActions
        {
            void OnFire(InputAction.CallbackContext context);
            void OnFireReleased(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnBack(InputAction.CallbackContext context);
            void OnPickUp(InputAction.CallbackContext context);
            void OnShowInventory(InputAction.CallbackContext context);
            void OnShowCrafting(InputAction.CallbackContext context);
            void OnItemAction(InputAction.CallbackContext context);
            void OnBlock(InputAction.CallbackContext context);
            void OnTeleport(InputAction.CallbackContext context);
            void OnShowMenu(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnButtonClick(InputAction.CallbackContext context);
        }
    }
}
