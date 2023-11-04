using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TankShootingTests
{
    GameObject camera;
    TankShooting ts;

    [SetUp]
    public void SetUpObjects()
    {
        camera = GameObject.Instantiate(
                (GameObject)Resources.Load("Main Camera")
        );
        ts = GameObject.Instantiate(
            (GameObject)Resources.Load("Tank"))
            .GetComponent<TankShooting>();
        ts.simulatedInput = true;
    }

    [TearDown]
    public void DestroyObjects() {
        GameObject.Destroy(ts.gameObject);
        GameObject.Destroy(camera);
    }

    [Test]
    public void OnEnableParametersAreSet() 
    {
        Assert.AreEqual(ts.m_MinLaunchForce, ts.CurrentLaunchForce, "Launch force not set to minimum");
        Assert.AreEqual(ts.m_MinLaunchForce, ts.AimSliderValue, "Aim slider not set to minimum");
    }

    [UnityTest]
    public IEnumerator StartParametersAreSet() 
    {
        // Skip frame for Start() to be called
        yield return null;

        Assert.AreNotEqual(null, ts.FireButton, "Fire button not set");
        Assert.AreEqual(ts.GetFireButtonName, ts.FireButton, "Fire button not set");
        Assert.AreEqual(ts.GetChargeSpeed, ts.ChargeSpeed, "Charge speed not set to appropriate value");
    }

    [UnityTest]
    public IEnumerator ChargingSoundPlaysAfterPressingFire() 
    {
        ts.simulatedFireDown = true;        
        // Waiting for next frame, 
        // one Update() iteration will happen
        yield return null;
        
        Assert.AreEqual(ts.m_ChargingClip, ts.m_ShootingAudio.clip, "Wrong audio clip is set");
        Assert.True(ts.m_ShootingAudio.isPlaying, "Audio clip not playing");
    }

    [UnityTest]
    public IEnumerator LaunchForceIncreasingWhileHolding() 
    {
        ts.simulatedFireDown = true;
        yield return null;
        ts.simulatedFireDown = false;
        ts.simulatedFire = true;
        yield return null;

        Assert.AreEqual(ts.CurrentLaunchForce, ts.m_AimSlider.value, "Aim slider should equal current launch force");
        Assert.Greater(ts.CurrentLaunchForce, ts.m_MinLaunchForce, "Launch force less or equal to min value");
    }

    [UnityTest]
    public IEnumerator ShellIsCreatedCorrectlyUponFiring() 
    {
        ts.simulatedFireDown = true;
        yield return null;
        ts.simulatedFireDown = false;
        ts.simulatedFireUp = true;
        yield return null;
        Rigidbody rb = ts.activeShell;

        Assert.True(ts.Fired, "\"Fired\" status should be set as true until next shot is charged");
        Assert.NotNull(rb, "Shell not created");
        Assert.AreEqual(ts.CurrentLaunchForce, rb.velocity.magnitude, "Initial shell speed should equal launch force");
        Assert.True(ts.m_ShootingAudio.isPlaying, "Audio clip not playing");
        Assert.AreEqual(ts.m_FireClip, ts.m_ShootingAudio.clip, "Wrong audio clip is set");
    }

    [UnityTest]
    public IEnumerator FiringAfterHoldingCreatesFasterShell() 
    {
        ts.simulatedFireDown = true;
        yield return null;
        ts.simulatedFireDown = false;
        ts.simulatedFire = true;
        while (!ts.Fired)
            yield return null;
        Rigidbody rb = ts.activeShell;

        Assert.NotNull(rb, "Shell not created");
        Assert.AreEqual(ts.m_MaxLaunchForce, rb.velocity.magnitude, "Initial shell speed should equal max launch force");
    }

    [UnityTest]
    public IEnumerator HoldingFireDoesntCauseRepeatedShooting()
    {
        Assert.Zero(ts.firedShellsDuringSession, "No shells should be fired before test actions");
        ts.simulatedFireDown = true;
        yield return null;
        ts.simulatedFireDown = false;
        ts.simulatedFire = true;
        while (!ts.Fired)
            yield return null;
        Assert.AreEqual(1, ts.firedShellsDuringSession, "One shell should be fired by now");

        // Waiting with fire button being held

        yield return new WaitForSeconds(ts.m_MaxChargeTime * 2f);
        Assert.AreEqual(1, ts.firedShellsDuringSession, "No more shells should have been fired");
    }

}
