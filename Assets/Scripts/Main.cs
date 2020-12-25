using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Connectable _connectablePrefab;
    [SerializeField] private float _radius = 10;
    [SerializeField] private float _count = 10;

    private void Start()
    {
        for (int i = 0; i < _count; i++)
        {
            var randomPosition = Random.insideUnitCircle * _radius;
            var position = new Vector3(randomPosition.x, 0, randomPosition.y);
            Instantiate(_connectablePrefab, position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
