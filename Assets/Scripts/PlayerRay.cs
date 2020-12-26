using UnityEngine;

public class PlayerRay : MonoBehaviour
{ 
    public LayerMask selectableLayer;

    private Main _main;

    private Connector _movedConnector;   

    private Ray _ray;
    private RaycastHit _hit;   

    private void Start()
    {
        _main = GetComponent<Main>();
    }

    private void Update()
    {     
        // Перетаскивание с платформы ПКМ
        if (Input.GetMouseButtonDown(1))
        {  
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                      
            if (Physics.Raycast(_ray, out _hit, 1000, selectableLayer))
            {
                //print(_hit.collider.gameObject.name);

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
                    _main.Select(connector);
                }
            }
            else
            {
                _main.UnselectAll();
            }
        }

        // перемещение коннектора
        if (_movedConnector != null)
        {
            var plane = new Plane(Vector3.up, 0);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                var worldPosition = ray.GetPoint(distance);
                _movedConnector.transform.position = worldPosition + Vector3.up * _movedConnector.PositionY;
            }
        }
    }
}
