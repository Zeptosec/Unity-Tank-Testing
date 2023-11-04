using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TankHealthTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TankHealthSpawnWith100()
    {
        GameObject tank = new();
        TankHealth th = tank.AddComponent<TankHealth>();
        th.ResetTank();
        // Use the Assert class to test conditions
        Assert.AreEqual(100f, th.m_CurrentHealth);
    }

    [Test]
    public void TankHealthTakeDamage()
    {
        GameObject tank = new();
        TankHealth th = tank.AddComponent<TankHealth>();
        th.ResetTank();
        th.TakeDamage(10f);
        // Use the Assert class to test conditions
        Assert.AreEqual(90f, th.m_CurrentHealth);
    }

    [Test]
    public void TankHealthTankDied()
    {
        GameObject tank = new();
        TankHealth th = tank.AddComponent<TankHealth>();
        th.ResetTank();
        th.TakeDamage(110f);
        // Use the Assert class to test conditions
        Assert.AreEqual(true, th.m_Dead);
    }

    //[Test]
    //public void TankHealthSpawnTestSimplePasses()
    //{
    //    GameObject tank = new();
    //    TankHealth th = tank.AddComponent<TankHealth>();
    //    th.ResetTank();
    //    th.TakeDamage(110f);
    //    // Use the Assert class to test conditions
    //    Assert.AreEqual(true, th.m_Dead);
    //}


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator TankHealthTestWithEnumeratorPasses()
    //{
    //    //health.ResetTank();
    //    // Use yield to skip a frame.
    //    yield return null;
    //    //health.TakeDamage(10);
    //    // Use the Assert class to test conditions.
    //    //Assert.AreEqual(100f, theal.m_CurrentHealth);
    //}
}
