using UnityEngine;

[CreateAssetMenu(fileName = "Keys", menuName = "Scriptable Objects/Keys")]
public class Keys : ScriptableObject {
    [Header("Gun Slots")]
    [SerializeField] public KeyCode slot1 = KeyCode.Alpha1;
    [SerializeField] public KeyCode slot2 = KeyCode.Alpha2;
    [SerializeField] public KeyCode slot3 = KeyCode.Alpha3;

    [Header("Main Keys")]
    [SerializeField] public KeyCode reload = KeyCode.R;
    [SerializeField] public KeyCode jump = KeyCode.Space;
    [SerializeField] public KeyCode crouch = KeyCode.LeftControl;

    [Header("Mouse/Look Sens")]
    [SerializeField] public KeyCode fire = KeyCode.Mouse0;
    [SerializeField] public KeyCode aim = KeyCode.Mouse1;
    [Range(1, 10)]
    [SerializeField] public int xSense;
    [Range(1, 10)]
    [SerializeField] public int ySense;
}