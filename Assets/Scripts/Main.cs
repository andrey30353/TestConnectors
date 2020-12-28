using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Connectable _connectablePrefab = default;
    [SerializeField] private LineRenderer _connectionPrefab = default;

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

    public void Select(Connector connector, bool onlyThis)
    {       

        if (onlyThis)
        {
            connector.Color = _selectedColor;
            return;
        }

        foreach (var connectable in _connectableList)
        {
            if (connectable.Connector == connector)
                connectable.ConnectorColor = _selectedColor;
            else
                connectable.ConnectorColor = _readyToConnectColor;
        }
    }

    public void Unselect(Connector connector)
    {
        connector.Color = _readyToConnectColor;      
    }   

    public void CreateConnection(Connector connector1, Connector connector2)
    {
        if (!connector1.CanConnectWith(connector2))
            return;

        var line = CreateConnectionLine(connector1.transform.position, connector2.transform.position);        

        connector1.AddConnection(connector2, line);
        connector2.AddConnection(connector1, line);        
    }

    public LineRenderer CreateConnectionLine(Vector3 start, Vector3 end)
    {
        var line = Instantiate(_connectionPrefab, Vector3.zero, Quaternion.identity);
        line.SetPositions(new Vector3[] { start, end });
        line.transform.SetParent(transform);
        return line;
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
