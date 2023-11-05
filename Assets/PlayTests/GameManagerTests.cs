using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameManagerTests
{
    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator SpawnPositionChecking()
    {
        string sceneName = "Main"; // Replace with your actual scene name
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return null;
        var gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
        var gameManager = gameManagerObject.GetComponent<GameManager>();

        Assert.AreEqual(gameManager.m_Tanks[0].m_SpawnPoint.position, gameManager.m_Tanks[0].m_Instance.transform.position);
    }


    [UnityTest]
    public IEnumerator TanksAreInGame()
    {
        string sceneName = "Main"; // Replace with your actual scene name
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return null;
        var tanks = GameObject.FindGameObjectsWithTag("Tank");
        Assert.AreEqual(tanks.Length, 2);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
    }
}
