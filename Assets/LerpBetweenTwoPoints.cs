using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class LerpBetweenTwoPoints : MonoBehaviour
{
	public Transform startPoint;
	public Transform endPoint;

	[Range(0f, 1f)]
	public float value;

	// Use this for initialization
	void Start ()
	{
		this.UpdateAsObservable ()
			.Where (_ => startPoint != null && endPoint != null)
			.Subscribe (_ => transform.position = Vector3.Lerp (startPoint.position, endPoint.position, Mathf.Clamp01(value)))
			.AddTo (this);
	}
}
