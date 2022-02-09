using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;            // rondas para ganar
    public float m_StartDelay = 3f;             // espara para jugar
    public float m_EndDelay = 3f;               // espera de final
    public CameraControl m_CameraControl;       // control de camara
    public Text m_MessageText;                  // Rtextos sobre pantalala
    public GameObject m_TankPrefab;             // referencia al prefact del tanque.
    public TankManager[] m_Tanks;               // array de las condiciones del tanque
 

    private int m_RoundNumber;                  //ronda actual
    private WaitForSeconds m_StartWait;         // segundos para empezar
    private WaitForSeconds m_EndWait;           // segundos de espera de final.
    private TankManager m_RoundWinner;          // se usa para decir quien gana la ronda
    private TankManager m_GameWinner;           // gana la partida


    private void Start()
    {
        // se crean las esperas
        m_StartWait = new WaitForSeconds (m_StartDelay);
        m_EndWait = new WaitForSeconds (m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        // se crean los tanques y empieza la partida.
        StartCoroutine (GameLoop ());
    }


    private void SpawnAllTanks()
    {
        // se sapawnean los tanques
        for (int i = 0; i < m_Tanks.Length; i++)
        {
           
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        // numero de tanques en nuestro caso 2
        Transform[] targets = new Transform[m_Tanks.Length];

        
        for (int i = 0; i < targets.Length; i++)
        {
           
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        // los objetivos que tiene que seguir la camara
        m_CameraControl.m_Targets = targets;
    }


    // loop para seguir jugando
    private IEnumerator GameLoop ()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundStarting ());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine (RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (m_GameWinner != null)
        {
            // If there is a game winner, restart the level.
            Application.LoadLevel (Application.loadedLevel);
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine (GameLoop ());
        }
    }


    private IEnumerator RoundStarting ()
    {
        // resetear posiciones y deshabilitar control de tanque
        ResetAllTanks ();
        DisableTankControl ();

        // camara a origen
        m_CameraControl.SetStartPositionAndSize ();

        // sumamos el numero de ronda
        m_RoundNumber++;
        m_MessageText.text = "RONDA " + m_RoundNumber;

        // espera para empezar
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying ()
    {
        // accedso a control de tanque
        EnableTankControl ();

        // limpiamos el texto de pantalla
        m_MessageText.text = string.Empty;
     
    

       
        while (!OneTankLeft())
        {
            
            yield return null;
        }
    }


    private IEnumerator RoundEnding ()
    {
        // deshabilitamos lis controles del tanque
        DisableTankControl ();

        //limpiamos texto
        m_RoundWinner = null;

        //mostramos ganados
        m_RoundWinner = GetRoundWinner ();

        // sumamos ronda ganada
        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        // comprobamos si hay victoria final
        m_GameWinner = GetGameWinner ();

        
        string message = EndMessage ();
        m_MessageText.text = message;

        // espera de fin de partida
        yield return m_EndWait;
    }


    // verificar si queda algun tanque
    private bool OneTankLeft()
    {
        // cuenta de tanques que quedan con valor 0
        int numTanksLeft = 0;

        // recorremos os tanques
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // si están activos, sumamos el contador
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    // comprobar si hay ganador de la ronda
    
    private TankManager GetRoundWinner()
    {
        // recorremos el array
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // si hay uno activo devolvemos ganador
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        // si nuinguno esta con vida es un empate
        return null;
    }


    // hacemos lo mismo para comprobar si hay ganador final del juego
    private TankManager GetGameWinner()
    {
        
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            //si ha ganado el numero de rondas es fgandoro
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        // si es null ha perdido
        return null;
    }


    //devolvemos mensaje despues de cada ronda
    private string EndMessage()
    {
        // en caso de empate .
        string message = "EMPATE!";

        // mensaje de ganar ronda
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " GANA LA RONDA!";

      
        message += "\n\n\n\n";

        // mostramos las victorias de cada jugador
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " VICTORIAS\n";
        }

        // si hay ganador mostramos texto
        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " GANA LA PARTIDA!";

        return message;
    }


    // reseteamos todas las propiedades de los tanques
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}