using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (GUITexture))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
