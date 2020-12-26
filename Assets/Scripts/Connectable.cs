using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connectable : MonoBehaviour
{
    public Connector Connector { get; private set; }

    public Color ConnectorColor
    {
        get => Connector.Color;
        set => Connector.Color = value;
    }

    private void Start()
    {
        Connector = GetComponentInChildren<Connector>();
    }

}
