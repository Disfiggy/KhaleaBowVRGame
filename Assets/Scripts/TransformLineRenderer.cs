using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRendererRef;

    [SerializeField]
    private Transform[] _transforms;
	
	// Update is called once per frame
	void LateUpdate ()
    {
        for (int i = 0; i < _transforms.Length; i++)
        {
            _lineRendererRef.SetPosition(i, _transforms[i].position);
        }
	}
}
