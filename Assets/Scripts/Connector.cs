using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public float PositionY { get; private set; }
       
    public Color Color 
    { 
        get => _mr.material.color; 
        set => _mr.material.color = value; 
    }

    private MeshRenderer _mr;  

    private void Start()
    {
        PositionY = transform.position.y;
        _mr = GetComponent<MeshRenderer>();
    }
}
