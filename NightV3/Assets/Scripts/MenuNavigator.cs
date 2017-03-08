using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour {
	/*
	*	Saving and Loading should occur in this class only
	 */
	void Start(){
		PersistenceLayer.LoadGameData();
	}

	public void LoadAndPlay() {
		PersistenceLayer.LoadPlayerData();
		SceneManager.LoadScene("Game");
	}

	public void NewGame(string difficulty) {
		SceneManager.LoadScene("Game");
	}

	public void NextLevel(){
		SceneManager.LoadScene("Game");
	}

	public void LoadGameOver() {
		PersistenceLayer.Clear();
		SceneManager.LoadScene("Game Over");
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene("Main Menu");
	}

	public void LoadEndDay(){
		PersistenceLayer.Save();
		SceneManager.LoadScene("End Day");
	}

	public void LoadDifficultSelect(){
		SceneManager.LoadScene("Difficulty Menu");
	}
}
