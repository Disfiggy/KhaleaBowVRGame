using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using VRTK.Examples.Archery;

public class ArrowTarget : MonoBehaviour
{
    [SerializeField]
    private LayerMask _targetLayers;

    [SerializeField]
    private float _delay = 2f;

	[SerializeField]
	private Transform _scoreCenter;

	[SerializeField]
	private float _scoreRadius;

	[SerializeField][Range(0f, 10f)]
	private int _maxScore;


    public Subject<ArrowTarget> TargetHit = new Subject<ArrowTarget>();
    public Subject<ArrowTarget> TargetDestroyed = new Subject<ArrowTarget>();

	// Use this for initialization
	void Start ()
    {
		this.OnTriggerEnterAsObservable()
			.Where(c => ((1 << c.gameObject.layer) & _targetLayers) != 0)
            .Subscribe(c =>
            {
                Debug.Log("Target hit. Destroying in " + _delay + " seconds");
				
					int score = Mathf.FloorToInt(Mathf.Lerp(_maxScore, 0, Vector3.Distance(_scoreCenter.position,
						c.attachedRigidbody.GetComponentInChildren<Arrow>().Tip.position) / _scoreRadius));

					Debug.Log("Score is " + score);
				
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

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (_scoreCenter.position, _scoreRadius);
	}
}
