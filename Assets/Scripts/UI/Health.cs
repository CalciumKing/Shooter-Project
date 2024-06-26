using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [Header("References")]
    private PlayerStats ps;
    [SerializeField] Screens s;
    private Image image;
    private int currentHealth;
    [SerializeField] TextMeshProUGUI text;
    private AudioSource healSound;
    private AudioSource damageSound;

    [Header("Health Animations")]
    public Image[] screens;
    public Gradient healthGradient;
    private Color targetColor;
    private float timeToDrain = .25f, targetHealth = 1;

    private void Awake() { image = GetComponent<Image>(); }
    private void Start() {
        ps = GameManager.i.ps;
        healSound = SoundManager.i.heal;
        damageSound = SoundManager.i.damage;
        image.color = healthGradient.Evaluate(targetHealth);
        targetColor = healthGradient.Evaluate(targetHealth);
    }
    private void Update() {
        if (currentHealth != ps.currentHealth) {
            if (currentHealth > ps.currentHealth) {
                damageSound.Play();
                screens[0].gameObject.SetActive(true);
            } else if (currentHealth < ps.currentHealth) {
                healSound.Play();
                screens[1].gameObject.SetActive(true);
            }

            UpdateHealthBar(ps.maxHealth, ps.currentHealth);
            currentHealth = ps.currentHealth;
        }
        if (ps.currentHealth > ps.maxHealth)
            ps.currentHealth = ps.maxHealth;
        else if (ps.currentHealth <= 0) {
            ps.currentHealth = 0;
            s.Death();
        }

        text.text = $"{ps.currentHealth} / {ps.maxHealth}";
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

        foreach (Image screen in screens)
            screen.gameObject.SetActive(false);
    }
}