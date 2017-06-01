using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using VRTK.Examples.Archery;

public class ScoreTarget : ArrowTarget
{
    [SerializeField]
    private float _destroyDelay = 2f;

	[SerializeField]
	private Transform _scoreCenter;

	[SerializeField]
	private float _scoreRadius;

	[SerializeField][Range(0f, 10f)]
	private int _maxScore;

	// Use this for initialization
	void Start ()
    {
		TargetHit.Subscribe(c =>
            {
                Debug.Log("Target hit. Destroying in " + _destroyDelay + " seconds");

                Vector3 hitPoint = c.attachedRigidbody.GetComponentInChildren<Arrow>().Tip.position;
                hitPoint = Vector3.ProjectOnPlane(hitPoint, _scoreCenter.forward);

                int score = CalculateScore(hitPoint);
				Debug.Log("Score is " + score);
				
                c.attachedRigidbody.isKinematic = true;
				
                Observable.Timer(System.TimeSpan.FromSeconds(_destroyDelay))
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

    int CalculateScore (Vector3 hitPoint)
    {
        return Mathf.FloorToInt(Mathf.Lerp(_maxScore, 0, Vector3.Distance(_scoreCenter.position, hitPoint) / _scoreRadius));
    }

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (_scoreCenter.position, _scoreRadius);
	}
}
