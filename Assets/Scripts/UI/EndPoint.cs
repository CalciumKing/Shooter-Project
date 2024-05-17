using System;
using TMPro;
using UnityEngine;

public class EndPoint : MonoBehaviour {
    private PlayerStats ps;
    private TextMeshProUGUI text;
    private Screens s;
    private Transform player;

    [SerializeField] GameObject winScreen;
    [SerializeField] TMP_Text timeText;

    private void Start() {
        ps = GameManager.i.ps;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.gameObject.SetActive(false);
        s = FindObjectOfType<Screens>();
    }
    private void Update() {
        if (text.gameObject.activeInHierarchy) {
            text.transform.LookAt(player);
            if (Input.GetKeyDown(KeyCode.E)) {
                SoundManager.i.victory.Play();
                s.MenuSettings(true);
                winScreen.SetActive(true);
                if (s.timer < ps.bestTime)
                    ps.bestTime = s.timer;
                timeText.text = "Your Time: " + String.Format("{0:0.00}", s.timer) + "\nBest Time: " + String.Format("{0:0.00}", ps.bestTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            text.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            text.gameObject.SetActive(false);
    }
}