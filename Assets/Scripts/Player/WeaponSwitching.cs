using System;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour {
    private PlayerStats ps;
    private Keys k;
    private Screens s;
    public Transform weaponHolder;
    public GameObject[] guns;

    private void Start() {
        ps = GameManager.i.ps;
        k = GameManager.i.k;
        s = FindObjectOfType<Screens>();
        ps.currentInsta = Instantiate(ps.currentWeapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    }
    private void Update() {
        if(!s.stopped) {
            if (Input.mouseScrollDelta.y == 0) {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKey(key) && key != ps.currentGunKey && key >= k.slot1 && key <= k.slot3) {
                        switch (key) {
                            case KeyCode thisKey when thisKey == k.slot1:
                                ps.currentWeapon = guns[0];
                                break;
                            case KeyCode thisKey when thisKey == k.slot2:
                                ps.currentWeapon = guns[1];
                                break;
                            case KeyCode thisKey when thisKey == k.slot3:
                                ps.currentWeapon = guns[2];
                                break;
                        }
                        ps.currentGunKey = key;
                        Destroy(ps.currentInsta);
                        ps.currentInsta = Instantiate(ps.currentWeapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
                    }
                }
            } else if (Input.mouseScrollDelta.y < 0) {
                foreach (GameObject gun in guns) {
                    if (ps.currentWeapon == gun) {
                        switch (ps.currentWeapon) {
                            case GameObject thisGun when thisGun == guns[0]:
                                ps.currentWeapon = guns[1];
                                ps.currentGunKey = k.slot2;
                                break;
                            case GameObject thisGun when thisGun == guns[1]:
                                ps.currentWeapon = guns[2];
                                ps.currentGunKey = k.slot3;
                                break;
                            case GameObject thisGun when thisGun == guns[2]:
                                ps.currentWeapon = guns[0];
                                ps.currentGunKey = k.slot1;
                                break;
                        }
                        Destroy(ps.currentInsta);
                        ps.currentInsta = Instantiate(ps.currentWeapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
                        break;
                    }
                }
            } else {
                foreach (GameObject gun in guns) {
                    if (ps.currentWeapon == gun) {
                        switch (ps.currentWeapon) {
                            case GameObject thisGun when thisGun == guns[0]:
                                ps.currentWeapon = guns[2];
                                ps.currentGunKey = k.slot3;
                                break;
                            case GameObject thisGun when thisGun == guns[1]:
                                ps.currentWeapon = guns[0];
                                ps.currentGunKey = k.slot1;
                                break;
                            case GameObject thisGun when thisGun == guns[2]:
                                ps.currentWeapon = guns[1];
                                ps.currentGunKey = k.slot2;
                                break;
                        }
                        Destroy(ps.currentInsta);
                        ps.currentInsta = Instantiate(ps.currentWeapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
                        break;
                    }
                }
            }
        }
    }
}