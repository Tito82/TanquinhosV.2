using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void CargarJuego(string nNombreScene){
       SceneManager.LoadScene(nNombreScene);
    }

    public void Salir(){
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
