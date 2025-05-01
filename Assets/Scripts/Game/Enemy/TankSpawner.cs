using UnityEngine;
using Zenject;
using System.Collections;

public class TankSpawner
{
    private readonly TankController.Factory _tankFactory;
    private readonly Transform[] _spawnPoints;
    private readonly float _interval;
    private Coroutine _spawnCoroutine;

    public TankSpawner(
        TankController.Factory tankFactory,
        Transform[] spawnPoints,
        float interval
    )
    {
        _tankFactory = tankFactory;
        _spawnPoints = spawnPoints;
        _interval    = interval;
        _spawnCoroutine = CoroutineRunner.Run( SpawnLoop() );
    }


    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);

            var idx = Random.Range(0, _spawnPoints.Length);
            var spawnPoint = _spawnPoints[idx];

            var tank = _tankFactory.Create();
            tank.transform.position = spawnPoint.position;
        }
    }

    public void StopSpawning()
    {
        CoroutineRunner.Stop(_spawnCoroutine);
    }
}
