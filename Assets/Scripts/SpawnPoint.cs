using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private bool _available = false;
    public bool Available { get { return _available; } }

    [SerializeField]
    private LayerMask triggerMask;

    private int _targetsInArea = 0;

    void Start ()
    {
        this.OnTriggerEnterAsObservable()
            .Where(other => (1 << other.gameObject.layer & triggerMask) != 0)
            .Subscribe(_ =>
            {
                _targetsInArea++;
                _available = false;
            })
            .AddTo(this);

        this.OnTriggerExitAsObservable()
            .Where(other => (1 << other.gameObject.layer & triggerMask) != 0)
            .Subscribe(_ =>
            {
                _targetsInArea--;
                _available = _targetsInArea == 0;
            })
            .AddTo(this);
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Available ? Color.green / 2f : Color.white / 2f;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
