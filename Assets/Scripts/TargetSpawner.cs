using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public bool enabled = false;

    [SerializeField]
    private Bounds _spawnBounds;

    [SerializeField]
    private float _spawnInterval;

    [SerializeField]
    private Transform _lookAtTarget;

    [SerializeField]
    private GameObject _targetPrefab;

    [SerializeField]
    private int _startDistance = 2;

    [SerializeField]
    private int _maxTargets = 3;

    private int _currentDistance;

    private List<ArrowTarget> _targets = new List<ArrowTarget>();

    // Use this for initialization
    void Start ()
    {
        _spawnBounds.center = transform.position;
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
        Vector2 random = Random.insideUnitCircle.normalized;
        Vector3 pos = _spawnBounds.ClosestPoint(new Vector3(random.x, 0f, random.y) * _currentDistance);
        Quaternion rot = Quaternion.LookRotation(_lookAtTarget.position - pos, Vector3.up);

        ArrowTarget target = Instantiate<GameObject>(_targetPrefab, pos, rot).GetComponent<ArrowTarget>();
        target.TargetHit.Subscribe(_ => _currentDistance++).AddTo(this);
        target.TargetDestroyed.Subscribe(t => _targets.Remove(t));
        _targets.Add(target);
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _spawnBounds.size);
    }
}
