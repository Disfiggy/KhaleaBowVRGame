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
                Vector3 hitPoint = c.attachedRigidbody.GetComponentInChildren<Arrow>().Tip.position;
				hitPoint = Vector3.ProjectOnPlane(hitPoint - _scoreCenter.position, _scoreCenter.forward);

				Debug.Log("Target hit at " + hitPoint + ". Center is at " + _scoreCenter.position + ". Destroying target in " + _destroyDelay + " seconds");

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
		float dist = hitPoint.magnitude;
		float div = dist / _scoreRadius;
		float lerp = Mathf.Lerp (_maxScore, 0, div);

		Debug.Log ("Distance: " + dist + "\nNormalized: " + div + "\nFinal score: " + lerp);

		return Mathf.RoundToInt(lerp);
    }

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (_scoreCenter.position, _scoreRadius);
	}
}
