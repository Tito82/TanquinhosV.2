using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;                
    public Slider m_Slider;                             
    public Image m_FillImage;                           
    public Color m_FullHealthColor = Color.green;       
    public Color m_ZeroHealthColor = Color.red;         
    public GameObject m_ExplosionPrefab;               


    private AudioSource m_ExplosionAudio;               
    private ParticleSystem m_ExplosionParticles;        
    private float m_CurrentHealth;                      
    private bool m_Dead;                                


    private void Awake ()
    {
        //instanciamos el prefact de las explosiones
        m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

        // metemos el audio de la explosion
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

        // deshabilitamos 
        m_ExplosionParticles.gameObject.SetActive (false);
    }


    private void OnEnable()
    {
        // restablecemos salud y miramos si esta muerto o no
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        // actualizamos salud en el slider
        SetHealthUI();
    }


    public void TakeDamage (float amount)
    {
        // restamos vida segun el daño.
        m_CurrentHealth -= amount;

        // se cambian los elementos ui
        SetHealthUI ();

        // si llega vida a 0 el tanque muere
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath ();
        }
    }


    private void SetHealthUI ()
    {
        // seteamos valor del slider
        m_Slider.value = m_CurrentHealth;

        // se cambian colores segun la vida y se muestra en el slider
        m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath ()
    {
        // Set the flag so that this function is only called once.
        m_Dead = true;

        // Move the instantiated explosion prefab to the tank's position and turn it on.
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive (true);

        // Play the particle system of the tank exploding.
        m_ExplosionParticles.Play ();

        // Play the tank explosion sound effect.
        m_ExplosionAudio.Play();

        // Turn the tank off.
        gameObject.SetActive (false);
    }
}