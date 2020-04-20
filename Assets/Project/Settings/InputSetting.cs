// GENERATED AUTOMATICALLY FROM 'Assets/Project/Settings/InputSetting.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputSetting : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSetting()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSetting"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""6841f0d7-f1e1-42c0-bb44-276920cb1419"",
            ""actions"": [
                {
                    ""name"": ""Move2D"",
                    ""type"": ""Value"",
                    ""id"": ""121703d0-3749-463a-a268-d0ae5d2e7d0d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse2D"",
                    ""type"": ""Value"",
                    ""id"": ""0d3acdab-63d3-4f8b-8f98-318386f0a2b1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseWheel"",
                    ""type"": ""Value"",
                    ""id"": ""9ec1af00-b5f7-4774-9062-8ad2c9be60bb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Button"",
                    ""id"": ""d2d1df3a-2f19-4380-8ff2-bfe1c1528c03"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NumberKey"",
                    ""type"": ""Button"",
                    ""id"": ""854e565e-6cc4-4213-9759-7068fe8fc631"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Key"",
                    ""type"": ""Button"",
                    ""id"": ""3b283982-14d3-4299-b03c-69eac051190e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""DirectionVector2D"",
                    ""id"": ""aaa3819b-3b33-4a2f-ba00-79b8ea9b41b9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move2D"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""41609d33-9b75-432c-b29c-932a87dd5b69"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move2D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8871f12b-9853-4184-8647-1f13ee2fca3e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move2D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1be594af-57e1-4ae2-a1c5-18bc96c3ab04"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move2D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2ee91b4c-6d9c-440a-af64-f51261b33a1e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move2D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""96797c0c-c6a9-4d2e-890f-b4f83cbb9070"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c48e9e0-ca01-4d12-8b9e-54fb2470eac6"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3706a612-e5a8-4560-b901-da6ab67d8482"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e678c0c4-8eb1-4aa2-9c9b-d2242fe7da6e"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33a813cb-f189-4aaf-9050-fedfefd8804a"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ec45d59-57d1-4b4e-b665-c91a1d7233f1"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""648b198e-7380-4177-b00e-88b4596a9dd1"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fa26142-6cf0-4f56-90ff-fa0f87b6aacc"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a0324ad-7573-4d61-a712-654fa5f1e625"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72391a06-4a55-4dcd-946a-af9338528637"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""164d096e-ce57-4771-beea-0f5024fd70a8"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8f99896-c100-4bd6-93ad-7f1fcd1e7576"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e2a97b0-2fd9-46de-a0ff-0bc5ff5766c4"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NumberKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efc18516-c4ed-486f-bdae-2ee04ac41a9f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d87c65a-6408-4826-873b-38d6efb8125c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Key"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c62c4e52-5258-452a-968a-4b66e1c3b3c3"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b215eed-bac4-4953-98ac-861ba1488720"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse2D"",
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
        m_Player_Move2D = m_Player.FindAction("Move2D", throwIfNotFound: true);
        m_Player_Mouse2D = m_Player.FindAction("Mouse2D", throwIfNotFound: true);
        m_Player_MouseWheel = m_Player.FindAction("MouseWheel", throwIfNotFound: true);
        m_Player_Mouse = m_Player.FindAction("Mouse", throwIfNotFound: true);
        m_Player_NumberKey = m_Player.FindAction("NumberKey", throwIfNotFound: true);
        m_Player_Key = m_Player.FindAction("Key", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Move2D;
    private readonly InputAction m_Player_Mouse2D;
    private readonly InputAction m_Player_MouseWheel;
    private readonly InputAction m_Player_Mouse;
    private readonly InputAction m_Player_NumberKey;
    private readonly InputAction m_Player_Key;
    public struct PlayerActions
    {
        private @InputSetting m_Wrapper;
        public PlayerActions(@InputSetting wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move2D => m_Wrapper.m_Player_Move2D;
        public InputAction @Mouse2D => m_Wrapper.m_Player_Mouse2D;
        public InputAction @MouseWheel => m_Wrapper.m_Player_MouseWheel;
        public InputAction @Mouse => m_Wrapper.m_Player_Mouse;
        public InputAction @NumberKey => m_Wrapper.m_Player_NumberKey;
        public InputAction @Key => m_Wrapper.m_Player_Key;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move2D.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove2D;
                @Move2D.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove2D;
                @Move2D.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove2D;
                @Mouse2D.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse2D;
                @Mouse2D.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse2D;
                @Mouse2D.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse2D;
                @MouseWheel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseWheel;
                @MouseWheel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseWheel;
                @Mouse.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouse;
                @NumberKey.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberKey;
                @NumberKey.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberKey;
                @NumberKey.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberKey;
                @Key.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnKey;
                @Key.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnKey;
                @Key.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnKey;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move2D.started += instance.OnMove2D;
                @Move2D.performed += instance.OnMove2D;
                @Move2D.canceled += instance.OnMove2D;
                @Mouse2D.started += instance.OnMouse2D;
                @Mouse2D.performed += instance.OnMouse2D;
                @Mouse2D.canceled += instance.OnMouse2D;
                @MouseWheel.started += instance.OnMouseWheel;
                @MouseWheel.performed += instance.OnMouseWheel;
                @MouseWheel.canceled += instance.OnMouseWheel;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @NumberKey.started += instance.OnNumberKey;
                @NumberKey.performed += instance.OnNumberKey;
                @NumberKey.canceled += instance.OnNumberKey;
                @Key.started += instance.OnKey;
                @Key.performed += instance.OnKey;
                @Key.canceled += instance.OnKey;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove2D(InputAction.CallbackContext context);
        void OnMouse2D(InputAction.CallbackContext context);
        void OnMouseWheel(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnNumberKey(InputAction.CallbackContext context);
        void OnKey(InputAction.CallbackContext context);
    }
}
