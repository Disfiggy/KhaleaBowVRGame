using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTriggerTargets : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    RaycastHit hit;

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 20f, _layerMask))
            {
                hit.collider.GetComponentInParent<ArrowTarget>().TargetHit.OnNext(null);
            }
        }
	}
}
