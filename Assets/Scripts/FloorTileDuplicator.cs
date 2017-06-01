using UnityEngine;

public class FloorTileDuplicator : MonoBehaviour
{
    [SerializeField]
    private float _height = 1f;

    [SerializeField]
    private GameObject _tilePrefab;

	void Start ()
    {
        int count = Mathf.RoundToInt(transform.position.y / _height) - 1;

        for (int i = 1; i <= count; i++)
        {
            Instantiate<GameObject>(_tilePrefab, transform.position + Vector3.down * i * _height, Quaternion.identity, transform);
        }
	}
}
