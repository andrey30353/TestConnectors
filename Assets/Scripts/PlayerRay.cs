using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public LayerMask selectableLayer;

    private Main _main;

    private Connector _movedConnector;   
    private Connector _firstConnector;
    private Connector _secondConnector;

    private LineRenderer _connectionLine = default;

    private Ray _ray;
    private RaycastHit _hit;

    private void Start()
    {
        _main = GetComponent<Main>();
        _connectionLine = _main.CreateConnectionLine(Vector3.zero, Vector3.zero);
        _connectionLine.name = "Visible connection line";
    }

    private void Update()
    {
        // Перетаскивание с платформы ПКМ
        if (Input.GetMouseButtonDown(1))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000, selectableLayer))
            {               
                var connector = _hit.collider.GetComponent<Connector>();
                if (connector != null)
                {
                    _movedConnector = connector;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _movedConnector = null;
        }

        // Создание связей ЛКМ
        if (Input.GetMouseButtonDown(0))
        {           
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000, selectableLayer))
            {
                var connector = _hit.collider.GetComponent<Connector>();
                if (connector != null)
                {
                    if (_firstConnector == null)
                    {
                        _firstConnector = connector;
                        _main.Select(connector, false);
                    }
                    else
                    {
                        if (connector != _firstConnector)
                        {
                            _main.CreateConnection(_firstConnector, connector);
                        }
                        UnselectAll();
                    }
                }
            }
            else
            {
                UnselectAll();
            }
        }

        // Создание связей перетаскиванием ЛКМ
        if (Input.GetMouseButton(0))
        {           
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000, selectableLayer))
            {
                var connector = _hit.collider.GetComponent<Connector>();
                if (connector != null)
                {
                    if (_firstConnector != null && _secondConnector == null && connector != _firstConnector)
                    {
                        _main.Select(connector, true);
                        _secondConnector = connector;
                    }
                }
            }
            else
            {
                if (_secondConnector != null)
                {
                    _main.Unselect(_secondConnector);
                    _secondConnector = null;
                }
            }

            if (_firstConnector != null)
            {
                var worldPosition = GetMousePositionInWorld(_firstConnector);

                _connectionLine.SetPosition(0, _firstConnector.transform.position);
                _connectionLine.SetPosition(1, worldPosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000, selectableLayer))
            {
                if (_firstConnector != null && _secondConnector != null)
                {
                    _main.CreateConnection(_firstConnector, _secondConnector);
                    UnselectAll();
                }
            }
            else
            {
                UnselectAll();
            }
        }

        // перемещение коннектора
        if (_movedConnector != null)
        {
            var worldPosition = GetMousePositionInWorld(_movedConnector);
            _movedConnector.Move(worldPosition);
        }
    }

    private bool Raycast()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(_ray, out _hit, 1000, selectableLayer);
    }

    private void HideLine()
    {
        _connectionLine.SetPosition(0, Vector3.zero);
        _connectionLine.SetPosition(1, Vector3.zero);
    }

    private Vector3 GetMousePositionInWorld(Connector connector)
    {
        var plane = new Plane(Vector3.up, connector.PositionY);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out var distance))
        {
            var worldPosition = ray.GetPoint(distance);
            return worldPosition;
        }

        return Vector3.zero;
    }

    private void UnselectAll()
    {
        _main.UnselectAll();

        _firstConnector = null;
        _secondConnector = null;

        HideLine();
    }
}
