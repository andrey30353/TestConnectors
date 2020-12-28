using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Connector : MonoBehaviour
{
    public Vector3 PositionY { get; private set; }

    private List<Connector> _connections;
    private List<LineRenderer> _lines;

    public Color Color 
    { 
        get => _mr.material.color; 
        set => _mr.material.color = value; 
    }

    private MeshRenderer _mr;  

    private void Start()
    {
        PositionY = new Vector3(0, transform.position.y, 0);

        _mr = GetComponent<MeshRenderer>();
        _connections = new List<Connector>();
        _lines = new List<LineRenderer>();
    }    

    public bool CanConnectWith(Connector other)
    {
        Assert.IsNotNull(other);
        if (_connections.Contains(other))
        {
            print("Already connected");
            return false;
        }

        return true;
    }

    public void AddConnection(Connector other, LineRenderer line)
    {   
        _connections.Add(other);
        _lines.Add(line);
    }

    public void Move(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        Assert.IsTrue(_connections.Count == _lines.Count);
       
        for (int i = 0; i < _lines.Count; i++)
        {
            _lines[i].SetPosition(0, worldPosition);
            _lines[i].SetPosition(1, _connections[i].transform.position);
        }      
    }
}
