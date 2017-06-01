using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ArrowTarget : MonoBehaviour
{
    [SerializeField]
    protected LayerMask _targetLayers;

    public Subject<Collider> TargetHit = new Subject<Collider>();
    public Subject<ArrowTarget> TargetDestroyed = new Subject<ArrowTarget>();

	// Use this for initialization
	void Awake ()
    {
		this.OnTriggerEnterAsObservable()
			.Where(c => ((1 << c.gameObject.layer) & _targetLayers) != 0)
            .Subscribe(c =>
            {
                TargetHit.OnNext(c);
            })
            .AddTo(this);
	}
}
