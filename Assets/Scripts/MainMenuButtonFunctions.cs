﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFunctions : MonoBehaviour {

	public void ExitGame(){
		Application.Quit();
	}

	public void GoToScene(string scene){
		SceneManager.LoadScene(scene);

	}

	
}
