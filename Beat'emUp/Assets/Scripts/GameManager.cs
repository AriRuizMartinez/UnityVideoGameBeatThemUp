using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameScenes { SampleScene, GameOver }

    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    private const GameScenes InitialScene = GameScenes.SampleScene;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        ChangeScene(InitialScene);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager - OnSceneLoaded: " + scene.name);
    }

    public static void ChangeScene(GameScenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}
