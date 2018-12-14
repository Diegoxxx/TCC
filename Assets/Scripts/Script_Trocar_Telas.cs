using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Trocar_Telas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Carrega_Menu()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_menu_cadastro");

	}
	public void Carrega_Aluno()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_cadastro_aluno");

	}
	public void Carrega_Escola()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_cadastro_escola");

	}
	public void Carrega_Professor()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_cadastro_professor");

	}

	public void Carrega_Modalidade()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_cadastro_modalidade");

	}

	public void SairDoApp()
	{

		Application.Quit();

	}

}
