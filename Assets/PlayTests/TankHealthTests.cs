using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TankHealthTests
{
    // A Test behaves as an ordinary method

    [Test]
    public void TakeDamage()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(100f, 42f, 56f), Quaternion.identity);
        var health = tankObject.GetComponent<TankHealth>();
        health.TakeDamage(10f);
        Assert.AreEqual(health.m_CurrentHealth, 90f);
    }

[UnityTest]
public IEnumerator CheckTanksSpawnAtSetPositions()
{
    string sceneName = "Main";
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    yield return null;
    yield return new WaitForSeconds(3);
    GameObject spawnPoint1 = GameObject.Find("SpawnPoint1");
    GameObject spawnPoint2 = GameObject.Find("SpawnPoint2");
    Assert.IsNotNull(spawnPoint1, "Spawn Point 1 was not found in the scene.");
    Assert.IsNotNull(spawnPoint2, "Spawn Point 2 was not found in the scene.");
    GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
    Assert.AreEqual(2, tanks.Length, "The number of tanks spawned is not correct.");
    bool foundTank1 = false, foundTank2 = false;
    foreach (var tank in tanks)
    {
        if (Vector3.Distance(tank.transform.position, spawnPoint1.transform.position) < 0.1f)
        {
            foundTank1 = true;
        }
        else if (Vector3.Distance(tank.transform.position, spawnPoint2.transform.position) < 0.1f)
        {
            foundTank2 = true;
        }
    }
    Assert.IsTrue(foundTank1, "Tank 1 did not spawn at the expected spawn point.");
    Assert.IsTrue(foundTank2, "Tank 2 did not spawn at the expected spawn point.");
    SceneManager.UnloadSceneAsync(sceneName);
    yield return null;
}

    [UnityTest]
public IEnumerator CheckBackgroundMusicIsPlaying()
{
    string sceneName = "Main"; 
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    yield return null;
    yield return new WaitForSeconds(1); 
    GameObject gameManagerObject = GameObject.Find("GameManager");
    Assert.IsNotNull(gameManagerObject, "GameManager object not found in the scene.");
    AudioSource backgroundMusicSource = gameManagerObject.GetComponent<AudioSource>();
    Assert.IsNotNull(backgroundMusicSource, "AudioSource for background music not found on GameManager.");
    Assert.IsTrue(backgroundMusicSource.isPlaying, "Background music is not playing.");
    AudioClip expectedClip = backgroundMusicSource.clip;
    Assert.IsNotNull(expectedClip, "The background music clip is not assigned.");
    SceneManager.UnloadSceneAsync(sceneName);
    yield return null;
}

[UnityTest]
    public IEnumerator CheckIfOnlyOneTankLeft()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        yield return null;
        yield return new WaitForSeconds(1);
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        Assert.IsTrue(!gameManager.OneTankLeft(), "There should more tanks than one.");
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < tanks.Length - 1; i++)
        {
            tanks[i].SetActive(false);
        }
        yield return null;
        Assert.IsTrue(gameManager.OneTankLeft(), "There should be only one tank left in the game.");
        SceneManager.UnloadSceneAsync("Main");
    }

     [UnityTest]
    public IEnumerator CheckGetRoundWinnerWithOnlyOneTank()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        yield return null;
        yield return new WaitForSeconds(1);
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < tanks.Length - 1; i++)
        {
            tanks[i].SetActive(false);
        }
        yield return null;
        TankManager roundWinner = gameManager.GetRoundWinner();
        Assert.IsNotNull(roundWinner, "Round winner should not be null when there is one tank left.");
        Assert.IsTrue(roundWinner.m_Instance.activeSelf, "Round winner should be the last active tank.");
        SceneManager.UnloadSceneAsync("Main");
    }

    [UnityTest]
    public IEnumerator CheckRoundEndMessageReturnsDrawWithNoActiveTanks()
    {
        SceneManager.LoadScene("Main");
        yield return null;
        GameManager gameManager = Object.FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager was not found in the scene.");
        foreach (var tankManager in gameManager.m_Tanks)
        {
            tankManager.m_Instance.SetActive(false);
        }
        string message = gameManager.EndMessage();
         Assert.IsTrue(message.StartsWith("DRAW!"), "The round end message should start with 'DRAW!' when there are no active tanks.");
        SceneManager.UnloadSceneAsync("Main");
        yield return null;
    }



    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]

    [Test]
    public void TankHealthSpawnWith100()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(100f, 42f, -56f), Quaternion.identity);
        var health = tankObject.GetComponent<TankHealth>();
        // Use the Assert class to test conditions
        Assert.AreEqual(100f, health.m_CurrentHealth);
    }

    [Test]
    public void TankHealthTankDied()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(100f, 42f, 33f), Quaternion.identity);
        var health = tankObject.GetComponent<TankHealth>();
        health.TakeDamage(110f);
        Assert.AreEqual(true, health.m_Dead);
    }

    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator NewTestScriptWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
