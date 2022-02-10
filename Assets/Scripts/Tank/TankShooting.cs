using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              
    public Rigidbody m_Shell;                   
    public Transform m_FireTransform;          
    public Slider m_AimSlider;                  
    public AudioSource m_ShootingAudio;         
    public AudioClip m_ChargingClip;            
    public AudioClip m_FireClip;                
    public float m_MinLaunchForce = 15f;        
    public float m_MaxLaunchForce = 30f;        
    public float m_MaxChargeTime = 0.75f;       


    private string m_FireButton;                
    private float m_CurrentLaunchForce;         
    private float m_ChargeSpeed;                
    private bool m_Fired;                       


    private void OnEnable()
    {
        // con el tanque encendido reiniciamos fuerza e intterfaz
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start ()
    {
        // asignamos fuego a cada jugador
        m_FireButton = "Fire" + m_PlayerNumber;

        // medimos fuerza segun pulsacion del boton de disparo
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update ()
    {
        // damos valor al minimo del slider
        m_AimSlider.value = m_MinLaunchForce;

        // al llegar al maximo el tanque lanzara la bala
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // 
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire ();
        }
        // al presionar el boton.
        else if (Input.GetButtonDown (m_FireButton))
        {
            // reseteamos la fuerza
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // cambiamos la imagen de acarga de la bala
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play ();
        }
        // si se presiona y aun no se ha lanzado sumamos fuerza del disparo
        else if (Input.GetButton (m_FireButton) && !m_Fired)
        {
            
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        //al soltar el boton se dispara la bala
        else if (Input.GetButtonUp (m_FireButton) && !m_Fired)
        {
        
            Fire ();
        }
    }


    private void Fire ()
    {
        // cbandera para comprobar que se ha llamado al metodo disparo
        m_Fired = true;

        // hacemos referencia al rigidbody de la bala
        Rigidbody shellInstance =
            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // setamanos fuerza y direccion de la bala
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;

        // entran los audios de disparo
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();

        // reiniciamos la fuerza de disparo
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}