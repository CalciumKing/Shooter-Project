using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XP : MonoBehaviour {
    public PlayerStats ps;
    public TextMeshProUGUI text;
    public Image image;
    private int currentXP;

    [Header("XP Animations")]
    private Coroutine increaseXPCoroutine;
    private float timeToIncrease = .25f, targetXP = 1;

    private void Awake() { image = GetComponent<Image>(); }
    private void Update() {
        if (currentXP != ps.xp) {
            UpdateXP(ps.levelInterval, ps.xp);
            currentXP = ps.xp;
        }

        if (ps.xp >= ps.levelInterval) {
            ps.xp -= ps.levelInterval;
            ps.levelInterval += 50;
            ps.level++;
        }

        text.text = ps.level + "\t" + ps.xp + " / " + ps.levelInterval;
    }

    public void UpdateXP(float maxXP, float currentXP) {
        targetXP = currentXP / maxXP;
        increaseXPCoroutine = StartCoroutine(DrainHealth());
    }
    private IEnumerator DrainHealth() {
        float fillAmount = image.fillAmount;
        float elapsedTime = 0f;
        while (elapsedTime < timeToIncrease) {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(fillAmount, targetXP, (elapsedTime / timeToIncrease));
            yield return null;
        }
    }
}