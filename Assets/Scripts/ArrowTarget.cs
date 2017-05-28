using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class ArrowTarget : MonoBehaviour
{
    [SerializeField]
    private LayerMask _targetLayers;

    [SerializeField]
    private float _delay = 2f;

    public Subject<ArrowTarget> TargetHit = new Subject<ArrowTarget>();
    public Subject<ArrowTarget> TargetDestroyed = new Subject<ArrowTarget>();

	// Use this for initialization
	void Start ()
    {
        this.OnTriggerEnterAsObservable()
            .Where(c => (c.gameObject.layer & _targetLayers) != 0)
            .Subscribe(c =>
            {
                Debug.Log("Target hit. Destroying in " + _delay + " seconds");

                TargetHit.OnNext(this);

                c.attachedRigidbody.isKinematic = true;
                Observable.Timer(System.TimeSpan.FromSeconds(_delay))
                    .Subscribe(_ =>
                    {
                        TargetDestroyed.OnNext(this);

                        Destroy(c.gameObject);
                        Destroy(gameObject);
                    })
                    .AddTo(this);
            })
            .AddTo(this);
	}
}
