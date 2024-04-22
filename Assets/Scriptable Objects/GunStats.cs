using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Scriptable Objects/GunStats")]
public class GunStats : ScriptableObject {
    [SerializeField] public int maxAmmo;
    [SerializeField] public int currentAmmo;
}