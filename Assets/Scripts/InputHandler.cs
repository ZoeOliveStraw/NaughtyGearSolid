using UnityEngine;
using UnityEngine.InputSystem;

public static class InputHandler
{
    private static Controls _controls;
    public static Controls Controls {
        get
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Enable();
                AssignActions();
            }
            return _controls;
        }
    }
    
    public static InputAction Action;
    public static InputAction Stealth;

    public static Vector2 MoveDir()
    {
        return Controls.Gameplay.Move.ReadValue<Vector2>();
    }

    private static void AssignActions()
    {
        Action = Controls.Gameplay.Action;
        Stealth = Controls.Gameplay.Stealth;
    }
}
