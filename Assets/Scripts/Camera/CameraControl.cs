using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;           
    public float m_MinSize = 6.5f;                  
    [HideInInspector] public Transform[] m_Targets; 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              


    private void Awake ()
    {
        m_Camera = GetComponentInChildren<Camera> ();
    }


    private void FixedUpdate ()
    {
        // mover camara a posicion
        Move ();

        // cambiamos tamaño de zoom
        Zoom ();
    }


    private void Move ()
    {
        // buscar posicion de los tanques
        FindAveragePosition ();

        // hacer smooth  a posicion
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition ()
    {
        Vector3 averagePos = new Vector3 ();
        int numTargets = 0;

        // recorremos el array de posiciones
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // si no esta activo pasamos al siguiente
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            
            averagePos += m_Targets[i].position;
            numTargets++;
        }

        // Si hay tanques sumamos la posiciones .
        if (numTargets > 0)
            averagePos /= numTargets;

        // guardamos.
        averagePos.y = transform.position.y;

        // la posicion de la camara es la media de las posiciones
        m_DesiredPosition = averagePos;
    }


    private void Zoom ()
    {
        // tamañao adecuado con zoom suave 
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize ()
    {
        // posicion a donde debe ir la camara
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        // iniciamos a 0 el valor del zoom
        float size = 0f;

        // recorremos posiciones de los tanques
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // si no esta ativo pasamos al siguiente
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // buscamos la posicion de cada tanque
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            //tamaño maximo del zoom
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // tamaño maximo depende de donde este la camara
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        // buffer de tamaño
        size += m_ScreenEdgeBuffer;

        // tamaño minimo de zoom
        size = Mathf.Max (size, m_MinSize);

        return size;
    }


    public void SetStartPositionAndSize ()
    {
        // buscar la posicion q
        FindAveragePosition ();

        // ajustamos posicion sin smooth
        transform.position = m_DesiredPosition;

        // tamaño optimo de caamara
        m_Camera.orthographicSize = FindRequiredSize ();
    }
}