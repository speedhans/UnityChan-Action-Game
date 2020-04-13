using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, InputSetting.IPlayerActions
{
    static public InputManager single;
    static public InputManager Instacne
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("InputManager");
                single = g.AddComponent<InputManager>();
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    InputSetting m_InputSetting;

    private void Awake()
    {
        m_InputSetting = new InputSetting();
        m_InputSetting.Player.SetCallbacks(this);

        for (int i = 0; i < 10; ++i)
        {
            m_NumberKeyEvent.Add(new List<Action<InputActionPhase>>());
        }
    }

    private void OnEnable()
    {
        m_InputSetting.Enable();
    }

    private void OnDisable()
    {
        m_InputSetting.Disable();
    }

    public Vector2 m_Move2D = Vector2.zero;
    public void OnMove2D(InputAction.CallbackContext context)
    {
        m_Move2D = context.ReadValue<Vector2>();
    }

    List<List<System.Action<InputActionPhase>>> m_MouseEvent = new List<List<System.Action<InputActionPhase>>>() { new List<Action<InputActionPhase>>(), new List<Action<InputActionPhase>>(), new List<Action<InputActionPhase>>()};
    public void OnMouse(InputAction.CallbackContext context)
    {
        int buttonNumber = 0;
        switch(context.control.name[0])
        {
            case 'l':
                buttonNumber = 0;
                break;
            case 'r':
                buttonNumber = 1;
                break;
            case 'm':
                buttonNumber = 2;
                break;
        }
        for (int i = 0; i < m_MouseEvent[buttonNumber].Count; ++i)
        {
            m_MouseEvent[buttonNumber][i].Invoke(context.phase);
        }
    }

    public void AddMouseEvent(int _Button, System.Action<InputActionPhase> _EventFunction) { m_MouseEvent[_Button].Add(_EventFunction); }
    public void ReleaseMouseEvent(int _Button, System.Action<InputActionPhase> _EventFunction) { m_MouseEvent[_Button].Remove(_EventFunction); }


    List<List<System.Action<InputActionPhase>>> m_NumberKeyEvent = new List<List<Action<InputActionPhase>>>();
    public void OnNumberKey(InputAction.CallbackContext context)
    {
        int number = (int)(context.control.name[0]) - 48;

        for (int i = 0; i < m_NumberKeyEvent[number].Count; ++i)
        {
            m_NumberKeyEvent[number][i].Invoke(context.phase);
        }
    }

    public void AddNumberKeyEvent(int _Button, System.Action<InputActionPhase> _EventFunction) { m_NumberKeyEvent[_Button].Add(_EventFunction); }
    public void ReleaseNumberKeyEvent(int _Button, System.Action<InputActionPhase> _EventFunction) { m_NumberKeyEvent[_Button].Remove(_EventFunction); }


    Dictionary<string, System.Action<InputActionPhase>> m_KeyEvent = new Dictionary<string, Action<InputActionPhase>>();
    public void OnKey(InputAction.CallbackContext context)
    {
        System.Action<InputActionPhase> action = null;
        if (m_KeyEvent.TryGetValue(context.control.name, out action))
        {
            action?.Invoke(context.phase);
        }
    }

    public void AddKeyEvent(string _KeyName, System.Action<InputActionPhase> _EventFunction)
    {
        System.Action<InputActionPhase> action;
        if (m_KeyEvent.TryGetValue(_KeyName, out action))
        {
            action += _EventFunction;
        }
        else
        {
            action += _EventFunction;
            m_KeyEvent.Add(_KeyName, action);
        }
    }
    public void ReleaseKeyEvent(string _KeyName, System.Action<InputActionPhase> _EventFunction) 
    {
        System.Action<InputActionPhase> action;
        if (m_KeyEvent.TryGetValue(_KeyName, out action))
        {
            action -= _EventFunction;
        }
    }

    public Vector2 m_Mouse2D = Vector3.zero;
    public void OnMouse2D(InputAction.CallbackContext context)
    {
        m_Mouse2D = context.ReadValue<Vector2>();
    }

    System.Action<float> m_MouseWheelEvent;
    public void OnMouseWheel(InputAction.CallbackContext context)
    {
        m_MouseWheelEvent?.Invoke(context.ReadValue<float>());
    }

    public void AddMouseWheelEvent(System.Action<float> _EventFunction) { m_MouseWheelEvent += _EventFunction; }
    public void ReleaseMouseWheelEvent(System.Action<float> _EventFunction) { m_MouseWheelEvent -= _EventFunction; }
}
