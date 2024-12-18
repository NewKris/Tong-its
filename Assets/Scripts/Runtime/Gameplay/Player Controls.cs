//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Player Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace NordicBibo.Runtime.Gameplay
{
    public partial class @PlayerControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""Mouse Controls"",
            ""id"": ""a6daf618-8bc1-4e31-8c0b-b0f9bc35ee7c"",
            ""actions"": [
                {
                    ""name"": ""Hold"",
                    ""type"": ""Button"",
                    ""id"": ""9eb47df7-1db9-4432-9549-79f0f18ddb46"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""0bbcef9d-f3bf-4850-a167-a03cd0f7b9cf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e7de7381-ea5b-43a1-98e6-82d7e4a6aac9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1d5e933-b9d2-49db-a5e3-da2b4bb54ea5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Mouse Controls
            m_MouseControls = asset.FindActionMap("Mouse Controls", throwIfNotFound: true);
            m_MouseControls_Hold = m_MouseControls.FindAction("Hold", throwIfNotFound: true);
            m_MouseControls_Click = m_MouseControls.FindAction("Click", throwIfNotFound: true);
        }

        ~@PlayerControls()
        {
            UnityEngine.Debug.Assert(!m_MouseControls.enabled, "This will cause a leak and performance issues, PlayerControls.MouseControls.Disable() has not been called.");
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

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Mouse Controls
        private readonly InputActionMap m_MouseControls;
        private List<IMouseControlsActions> m_MouseControlsActionsCallbackInterfaces = new List<IMouseControlsActions>();
        private readonly InputAction m_MouseControls_Hold;
        private readonly InputAction m_MouseControls_Click;
        public struct MouseControlsActions
        {
            private @PlayerControls m_Wrapper;
            public MouseControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Hold => m_Wrapper.m_MouseControls_Hold;
            public InputAction @Click => m_Wrapper.m_MouseControls_Click;
            public InputActionMap Get() { return m_Wrapper.m_MouseControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MouseControlsActions set) { return set.Get(); }
            public void AddCallbacks(IMouseControlsActions instance)
            {
                if (instance == null || m_Wrapper.m_MouseControlsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MouseControlsActionsCallbackInterfaces.Add(instance);
                @Hold.started += instance.OnHold;
                @Hold.performed += instance.OnHold;
                @Hold.canceled += instance.OnHold;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }

            private void UnregisterCallbacks(IMouseControlsActions instance)
            {
                @Hold.started -= instance.OnHold;
                @Hold.performed -= instance.OnHold;
                @Hold.canceled -= instance.OnHold;
                @Click.started -= instance.OnClick;
                @Click.performed -= instance.OnClick;
                @Click.canceled -= instance.OnClick;
            }

            public void RemoveCallbacks(IMouseControlsActions instance)
            {
                if (m_Wrapper.m_MouseControlsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMouseControlsActions instance)
            {
                foreach (var item in m_Wrapper.m_MouseControlsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MouseControlsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MouseControlsActions @MouseControls => new MouseControlsActions(this);
        public interface IMouseControlsActions
        {
            void OnHold(InputAction.CallbackContext context);
            void OnClick(InputAction.CallbackContext context);
        }
    }
}