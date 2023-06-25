using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake() {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    public void GameStart(int characterIndex, List<int> list) {
        StartCoroutine(GameStarting(characterIndex, list));
    }

    IEnumerator GameStarting(int characterIndex, List<int> list) {
        SceneManager.LoadScene("Main");
        yield return null;
        StageManager.Instance.Init(characterIndex, list);
    }
}
