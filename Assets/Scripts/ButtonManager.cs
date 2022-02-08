using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void CargarJuego(string nNombreScene){
       SceneManager.LoadScene(nNombreScene);
    }

    public void Salir(){
        Application.Quit();
    }
}
