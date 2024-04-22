using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunInfo : MonoBehaviour {
    [Header("References")]
    private PlayerStats ps;
    public GunStats m, s, p;
    private Transform player;
    private WeaponSwitching w;
    private Image image;
    private int currentAmmo;
    public TextMeshProUGUI typeText, ammoText;

    [Header("Ammo Animations")]
    private Coroutine drainAmmoCoroutine;
    public Gradient ammoGradient;
    private Color targetColor;
    private float timeToDrain = .2f, targetAmmo = 1;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        image = GetComponent<Image>();
        w = player.GetComponent<WeaponSwitching>();
    }
    private void Start() {
        ps = GameManager.i.ps;
        image.color = ammoGradient.Evaluate(targetAmmo);
        targetColor = ammoGradient.Evaluate(targetAmmo);

        UpdateAmmoBar(m.maxAmmo, m.currentAmmo);
        UpdateAmmoBar(s.maxAmmo, s.currentAmmo);
        UpdateAmmoBar(p.maxAmmo, p.currentAmmo);

    }
    private void Update() {
        switch (ps.currentGunKey) {
            case KeyCode.Alpha1:
                typeText.text = "Machine Gun";
                ammoText.text = m.currentAmmo + " / " + m.maxAmmo;
                if (currentAmmo != m.currentAmmo) {
                    UpdateAmmoBar(m.maxAmmo, m.currentAmmo);
                    currentAmmo = m.currentAmmo;
                }
                break;
            case KeyCode.Alpha2:
                typeText.text = "Shotgun";
                ammoText.text = s.currentAmmo + " / " + s.maxAmmo;
                if (currentAmmo != s.currentAmmo) {
                    UpdateAmmoBar(s.maxAmmo, s.currentAmmo);
                    currentAmmo = s.currentAmmo;
                }
                break;
            case KeyCode.Alpha3:
                typeText.text = "Pistol";
                ammoText.text = p.currentAmmo + " / " + p.maxAmmo;
                if (currentAmmo != p.currentAmmo) {
                    UpdateAmmoBar(p.maxAmmo, p.currentAmmo);
                    currentAmmo = p.currentAmmo;
                }
                break;
        }
    }

    public void UpdateAmmoBar(float maxHealth, float currentHealth) {
        targetAmmo = currentHealth / maxHealth;
        drainAmmoCoroutine = StartCoroutine(ReduceAmmo());
        targetColor = ammoGradient.Evaluate(targetAmmo);
    }
    private IEnumerator ReduceAmmo() {
        float fillAmount = image.fillAmount;
        Color currentColor = image.color;
        float elawedTime = 0f;
        while (elawedTime < timeToDrain) {
            elawedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(fillAmount, targetAmmo, (elawedTime / timeToDrain));
            image.color = Color.Lerp(currentColor, targetColor, (elawedTime / timeToDrain));
            yield return null;
        }
    }
}