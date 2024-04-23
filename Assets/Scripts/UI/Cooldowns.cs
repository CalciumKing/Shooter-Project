using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cooldowns : MonoBehaviour {
    [Header("Images")]
    public Image healImage;
    public Image throwableImage;

    [Header("Animations")]
    private float timeToIncrease = .25f, healCooldown = 1, throwCooldown = 1;

    private void Start() {
        healImage.fillAmount = 0;
        throwableImage.fillAmount = 0;
    }
    public void UpdateHealCooldown(float maxCooldown, float currentCoolDown) {
        healCooldown = currentCoolDown / maxCooldown;
        StartCoroutine(DrainHealCooldown());
    }
    public void UpdateThrowCooldown(float maxCooldown, float currentCoolDown) {
        throwCooldown = currentCoolDown / maxCooldown;
        StartCoroutine(DrainThrowCooldown());
    }
    private IEnumerator DrainHealCooldown() {
        float fillAmount = healImage.fillAmount;
        float elapsedTime = 0f;
        while (elapsedTime < timeToIncrease) {
            elapsedTime += Time.deltaTime;
            healImage.fillAmount = Mathf.Lerp(fillAmount, healCooldown, (elapsedTime / timeToIncrease));
            yield return null;
        }
    }
    private IEnumerator DrainThrowCooldown() {
        float fillAmount = healImage.fillAmount;
        float elapsedTime = 0f;
        while (elapsedTime < timeToIncrease) {
            elapsedTime += Time.deltaTime;
            throwableImage.fillAmount = Mathf.Lerp(fillAmount, throwCooldown, (elapsedTime / timeToIncrease));
            yield return null;
        }
    }
}