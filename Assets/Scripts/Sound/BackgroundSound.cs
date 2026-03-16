using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSound : MonoBehaviour
{
   
    private static bool Playing = false;
    private static bool Battleplaying = false;

    Scene current_Scene;
    string scene;

    
    private void Update()
    {
        current_Scene = SceneManager.GetActiveScene();
        scene = current_Scene.name;
        if (scene == "BattleScene")
        {
           
            if(Playing == true)
            {
                SoundEffectManager.audioSource.Stop();
                print("stopping source");
                Playing = false;
                
            }
            if (!Battleplaying)
            {
                PlayFightMusic();
                Battleplaying = true;
            }
        }



        else if (scene == "map2" || scene == "map1" || scene == "AidenTestScene")
        {

            if (Battleplaying == true)
            {
                SoundEffectManager.audioSource.Stop();
                print("stopping source");
                Battleplaying = false;
                
            }
            if (!Playing)
            {
                PlayOverworldMusic();
                Playing = true;
            }
        }
        void PlayFightMusic()
        {
            SoundEffectManager.Play("FightBackground");
        }

        void PlayOverworldMusic()
        {
            SoundEffectManager.Play("OverworldBackground");
        }

    }
}

