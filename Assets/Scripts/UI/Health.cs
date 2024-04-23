using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [Header("References")]
    public PlayerStats ps;
    public Screens s;
    private Image image;
    private int currentHealth;
    public TextMeshProUGUI text;

    [Header("Health Animations")]
    public Gradient healthGradient;
    private Color targetColor;
    private float timeToDrain = .25f, targetHealth = 1;

    private void Awake() { image = GetComponent<Image>(); }
    private void Start() {
        image.color = healthGradient.Evaluate(targetHealth);
        targetColor = healthGradient.Evaluate(targetHealth);
    }
    private void Update() {
        if (currentHealth != ps.currentHealth) {
            UpdateHealthBar(ps.maxHealth, ps.currentHealth);
            currentHealth = ps.currentHealth;
        }
        if (ps.currentHealth > ps.maxHealth)
            ps.currentHealth = ps.maxHealth;
        else if (ps.currentHealth <= 0) {
            ps.currentHealth = 0;
            s.Death();
        }

        text.text = ps.currentHealth + " / " + ps.maxHealth;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth) {
        targetHealth = currentHealth / maxHealth;
        StartCoroutine(DrainHealth());
        targetColor = healthGradient.Evaluate(targetHealth);
    }
    private IEnumerator DrainHealth() {
        float fillAmount = image.fillAmount;
        Color currentColor = image.color;
        float elapsedTime = 0f;
        while (elapsedTime < timeToDrain) {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(fillAmount, targetHealth, (elapsedTime / timeToDrain));
            image.color = Color.Lerp(currentColor, targetColor, (elapsedTime / timeToDrain));
            yield return null;
        }
    }
}