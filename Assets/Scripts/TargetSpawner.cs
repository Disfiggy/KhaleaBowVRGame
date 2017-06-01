using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class TargetSpawner : MonoBehaviour
{
    public bool enabled = false;

    [SerializeField]
    private float _spawnInterval;

    [SerializeField]
    private Transform _lookAtTarget;

    [SerializeField]
    private GameObject _targetPrefab;

    [SerializeField]
    private float _startDistance = 2;

    [SerializeField]
    private float _distanceIncrease = 0.5f;

    [SerializeField]
    private int _maxTargets = 3;

    private float _currentDistance;

    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private List<ArrowTarget> _targets = new List<ArrowTarget>();

    // Use this for initialization
    void Start ()
    {
        _spawnPoints = FindObjectsOfType<SpawnPoint>().ToList();

        _currentDistance = _startDistance;

        Observable.Interval(System.TimeSpan.FromSeconds(_spawnInterval))
            .Where(_ => enabled && _targets.Count < _maxTargets)
            .Subscribe(_ => SpawnTarget())
            .AddTo(this);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            enabled = !enabled;
	}

    void SpawnTarget ()
    {
        Vector3 pos = GetRandomAvailableSpawnPointWithinDistance(_currentDistance).transform.position;
        Quaternion rot = Quaternion.LookRotation(_lookAtTarget.position - new Vector3(pos.x, 0f, pos.z), Vector3.up);

        ArrowTarget target = Instantiate<GameObject>(_targetPrefab, pos, rot).GetComponent<ArrowTarget>();
        target.TargetHit.Subscribe(_ => _currentDistance += _distanceIncrease).AddTo(this);
        target.TargetDestroyed.Subscribe(t => _targets.Remove(t)).AddTo(this);

        _targets.Add(target);
    }

    SpawnPoint GetRandomAvailableSpawnPointWithinDistance (float distance)
    {
        List<SpawnPoint> validSpawnPoints = _spawnPoints.FindAll(sp => sp.Available && sp.transform.position.sqrMagnitude <= distance * distance);

        return validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
    }
}
