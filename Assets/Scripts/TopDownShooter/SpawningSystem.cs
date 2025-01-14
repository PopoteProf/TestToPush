using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningSystem : MonoBehaviour
{

    public bool DoSpawn;
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private float _spawningDelay = 5;
    [SerializeField] private TopDownMonster _monsterPrefabs;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _displayPointGizmo;

    private float _timer;
    private Color col = new Color(1, 0.2f, 1,0.5f);

    private void Update() {
        if (!DoSpawn) return;
        _timer += Time.deltaTime;

        if (_timer >= _spawningDelay)
        {
            _timer = 0;
            Transform spawnPos = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            TopDownMonster monster =Instantiate(_monsterPrefabs, spawnPos.position, spawnPos.rotation);
            monster.SetTarget(_target);
        }
    }

    private void OnDrawGizmos() {
        if (!_displayPointGizmo) return;

        Gizmos.color = col;
        foreach (var point in _spawnPoints)
        {
            if (point == null) return;
            
            Gizmos.DrawCube(point.position,Vector3.one);
        }
    }
}