using UnityEngine;

[CreateAssetMenu(fileName = "Keys", menuName = "Scriptable Objects/Keys")]
public class Keys : ScriptableObject {
    [Header("Gun Slots")]
    public KeyCode slot1 = KeyCode.Alpha1;
    public KeyCode slot2 = KeyCode.Alpha2;
    public KeyCode slot3 = KeyCode.Alpha3;

    [Header("Main Keys")]
    public KeyCode reload = KeyCode.R;
    public KeyCode jump = KeyCode.Space;
    public KeyCode crouch = KeyCode.LeftControl;
    public KeyCode throwable = KeyCode.Q;
    public KeyCode heal = KeyCode.X;

    [Header("Mouse/Look Sens")]
    public KeyCode fire = KeyCode.Mouse0;
    public KeyCode aim = KeyCode.Mouse1;
    [Range(1, 10)]
    public int xSense;
    [Range(1, 10)]
    public int ySense;
    [Range(1, 10)]
    public int scopeXSense;
    [Range(1, 10)]
    public int scopeYSense;
}