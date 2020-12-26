using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Connectable _connectablePrefab = default;
    [SerializeField] private float _radius = 10;
    [SerializeField] private int _count = 10;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.yellow;
    [SerializeField] private Color _readyToConnectColor = Color.blue;

    private List<Connectable> _connectableList;

    private Connector _selectedConnector;

    private void Start()
    {
        _connectableList = new List<Connectable>(_count);

        for (int i = 0; i < _count; i++)
        {
            var randomPosition = UnityEngine.Random.insideUnitCircle * _radius;
            var position = new Vector3(randomPosition.x, 0, randomPosition.y);
            var connectable = Instantiate(_connectablePrefab, position, Quaternion.identity);
            connectable.transform.SetParent(transform);

            _connectableList.Add(connectable);
        }
    }

    public void Select(Connector connector)
    {
        if (_selectedConnector != null)
            ConnectWith(connector);

        _selectedConnector = connector;

        foreach (var connectable in _connectableList)
        {
            if (connectable.Connector == connector)
                connectable.ConnectorColor = _selectedColor;
            else
                connectable.ConnectorColor = _readyToConnectColor;
        }
    }

    private void ConnectWith(Connector connector)
    {
        print("ConnectWith");
    }

    public void UnselectAll()
    {
        _selectedConnector = null;

        ResetColor();
    }

    private void ResetColor()
    {
        foreach (var connectable in _connectableList)
        {
            connectable.ConnectorColor = _defaultColor;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
