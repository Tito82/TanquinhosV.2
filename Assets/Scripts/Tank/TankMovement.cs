using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber;              
    public float m_Speed = 12f;                 
    public float m_TurnSpeed = 180f;          
    public AudioSource m_MovementAudio;         
    public AudioClip m_EngineIdling;            
    public AudioClip m_EngineDriving;           
    public float m_PitchRange = 0.2f;          


    private string m_MovementAxisName;          
    private string m_TurnAxisName;             
    private Rigidbody m_Rigidbody;             
    private float m_MovementInputValue;       
    private float m_TurnInputValue;           
    private float m_OriginalPitch;             


    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }


    private void OnEnable ()
    {
        // comprobamos que no es kinematic
        m_Rigidbody.isKinematic = false;

        // reseteamos valores de inicio
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        // cuando esta parado es kinematic
        m_Rigidbody.isKinematic = true;
    }


    private void Start ()
    {
        // los ejes asociados a el jugador
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // guardamos el audio
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update ()
    {
        // guardamos valores de los ejes 
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

        EngineAudio ();
    }


    private void EngineAudio ()
    {
        //si no hay señal el tanque esta parado
        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
        {
            // entra el sonido del tanque
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // y al moverse cambiamos al audio de movimiento
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play ();
            }
        }
        else
        {
            // aqui entra el sonido de espera cuando no se mueve el tanque
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // cambia a movimiento
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate ()
    {
        // ajusta la posición y la orientación del rigiboby  en FixedUpdate. 
        Move ();
        Turn ();
    }


    private void Move ()
    {
        // creamos los vectore s de movimiento
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Alo aplicamos al rigidbody
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        // grados de giro segun el imput 
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        //hacemos que gire en esos ejes.
        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        // y aplicamos estos movimientos al rigdibody.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }

    public int GetPlayerNumber(){
    
        Debug.Log("-> " + m_PlayerNumber);
        return m_PlayerNumber;
    }
}