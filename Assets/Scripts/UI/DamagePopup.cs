using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour {
    private TextMeshProUGUI damageText;
    private void Awake() { damageText = GetComponentInChildren<TextMeshProUGUI>(); }
    public void SetDamageText(float amount) {
        try {
            damageText.text = amount.ToString();
        } catch { }
    }
    public void DestroyText() { Destroy(gameObject); }
}