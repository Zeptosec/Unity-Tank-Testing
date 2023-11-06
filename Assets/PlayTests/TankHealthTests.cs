using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TankHealthTests
{
    // A Test behaves as an ordinary method
    [TestCase(10f, 90f)]
    [TestCase(20f, 80f)]
    [TestCase(30f, 70f)]
    [Test]
    public void TakeDamage(float dmg, float expected)
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(100f, 42f, 56f), Quaternion.identity);
        var health = tankObject.GetComponent<TankHealth>();
        health.TakeDamage(dmg);
        Assert.AreEqual(health.m_CurrentHealth, expected);
    }

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
