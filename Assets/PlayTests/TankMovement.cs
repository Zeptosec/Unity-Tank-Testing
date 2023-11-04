using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TankMovementTest
{

    [UnityTest]
    public IEnumerator TankMovesForward()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(0f, 10f, 0f), Quaternion.identity);
        var tank = tankObject.GetComponent<TankMovement>();
        float initialZPosition = tank.transform.position.z;
        tank.m_MovementInputValue = 10;
        tank.Move();
        yield return new WaitForSeconds(0.2f);
        Assert.Greater(tank.transform.position.z, initialZPosition);
        Object.Destroy(tank);
    }

    [UnityTest]
    public IEnumerator TankTurns()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(0f, 20f, 0f), Quaternion.identity);
        var tank = tankObject.GetComponent<TankMovement>();
        float initialRotation = tank.transform.rotation.eulerAngles.y;
        tank.m_TurnInputValue = 1;
        tank.Turn();
        yield return new WaitForSeconds(0.2f);
        float rotatedAngle = tank.transform.rotation.eulerAngles.y;
        Assert.AreNotEqual(rotatedAngle, initialRotation);
        Object.Destroy(tank);
    }

    [UnityTest]
    public IEnumerator EngineAudioChanges()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(0f, 30f, 0f), Quaternion.identity);
        var tank = tankObject.GetComponent<TankMovement>();
        tank.EngineAudio();
        yield return new WaitForSeconds(0.5f);
        tank.m_MovementInputValue = 5;
        tank.EngineAudio();
        Assert.AreEqual(tank.m_MovementAudio.clip, tank.m_EngineDriving);
        Object.Destroy(tank);
    }

    [UnityTest]
    public IEnumerator TankHandlesCollision()
    {
        
        Vector3 obstaclePosition = new Vector3(0f, 40f, 1f);
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.transform.position = obstaclePosition;

        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(0f, 40f, 0f), Quaternion.identity);
        var tank = tankObject.GetComponent<TankMovement>();
        tank.transform.position = new Vector3(0f, 0f, 0f);
        tank.m_MovementInputValue = 4;
        tank.Move();
        yield return new WaitForSeconds(1f);
        Assert.Less(tank.transform.position.z, obstaclePosition.z);
        Object.Destroy(tank);
        Object.Destroy(obstacle);
    }

    [UnityTest]
    public IEnumerator TankInputSensitivity()
    {
        var tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");
        var tankObject = GameObject.Instantiate(tankPrefab, new Vector3(0f, 50f, 0f), Quaternion.identity);
        var tank = tankObject.GetComponent<TankMovement>();
        float initialPositionX = tank.transform.position.x;
        tank.m_Speed = 10;
        tank.m_MovementInputValue = 0.1f;
        tank.Move();
        yield return null;
        float finalPositionX = tank.transform.position.x;
        Assert.Less(Mathf.Abs(finalPositionX - initialPositionX), 0.1f);
        Object.Destroy(tank);
    }
}
