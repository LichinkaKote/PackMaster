using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class GameManager
{
    private static Level levelToLoad = Prefabs.levels[0];
    private static int levelIndex;
    public static int money;
    public static Level LevelToLoad { get => levelToLoad; }
    public static int LevelIndex { get => levelIndex; }

    public static void NewGame()
    {
        levelIndex = 0;
        levelToLoad = Prefabs.levels[levelIndex];
        SceneManager.LoadScene(1);
    }
    public static void LoadNextLevel()
    {
        levelIndex++;
        if (levelIndex >= Prefabs.levels.Length)
        {
            levelIndex = 0;
        }
        levelToLoad = Prefabs.levels[levelIndex];
        SceneManager.LoadScene(1);
    }
}
