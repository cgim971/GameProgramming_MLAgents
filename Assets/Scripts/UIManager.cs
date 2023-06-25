using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance => _instance;
    private static UIManager _instance;

    public RectTransform Panel;
    public Button LobbyBtn;
    public TMP_Text Text;


    private void Awake() {
        if (_instance == null)
            _instance = this;
    }

    private void Start() {
        LobbyBtn.onClick.AddListener(() => ToLobbyBtn());

        HidePanel();
    }

    public void ShowPanel(bool isDie) {
        Panel.gameObject.SetActive(true);

        if (isDie) {
            Text.SetText("Á×¾ú´Ù ¤»");
        }
        else {
            Text.SetText("ÀÌ°å³ß ¤»");
        }
    }

    public void HidePanel() {
        Panel.gameObject.SetActive(false);
    }

    public void ToLobbyBtn() {
        SceneManager.LoadScene("Lobby");
    }
}
