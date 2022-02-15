using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{  //cargamos la escena
    public void LoadScene(string nNombreScene){
       SceneManager.LoadScene(nNombreScene);
    }

    public void Exit(){
        // boton salir util en editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

}
