using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Script_Cadastra_Escola : MonoBehaviour {

	public InputField nome;
	public InputField endereco;
	public InputField municipio;
	public InputField cep;
	public InputField email;
	public InputField entidade;

	public Toggle turnoManha;
	public Toggle turnoTarde;
	public Toggle turnoNoite;

	public Toggle eduInfantil;
	public Toggle eduInicial;
	public Toggle eduFinal;
	public Toggle eduMedio;

	public InputField numInfantil;
	public InputField numInicial;
	public InputField numFinal;
	public InputField numMedio;
	public InputField numProfessores;
	public InputField numFuncionarios;

	DatabaseReference reference;

	// Use this for initialization
	void Start () {
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    var dependencyStatus = task.Result;
    
    if (dependencyStatus == Firebase.DependencyStatus.Available) {
    FirebaseApp app = FirebaseApp.DefaultInstance;

    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://tcc-gamificacao.firebaseio.com");
    reference = FirebaseDatabase.DefaultInstance.RootReference;

    //mDatabaseRef.Child("users").Child("1").SetRawJsonValueAsync("Diego");

  } else {
    UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    // Firebase Unity SDK is not safe to use here.
  }
});
	}

	public void salvar()
	{

		string key = reference.Child("Escolas").Push().Key;
		
		reference.Child("Escolas").Child(key).Child("Nome").SetValueAsync(nome.text);
		reference.Child("Escolas").Child(key).Child("Endereço").SetValueAsync(endereco.text);
		reference.Child("Escolas").Child(key).Child("Municipio").SetValueAsync(municipio.text);
		reference.Child("Escolas").Child(key).Child("CEP").SetValueAsync(cep.text);
		reference.Child("Escolas").Child(key).Child("Email").SetValueAsync(email.text);
		reference.Child("Escolas").Child(key).Child("Entidade").SetValueAsync(entidade.text);
			
		reference.Child("Escolas").Child(key).Child("Turnos").Child("Manha").SetValueAsync(turnoManha.isOn);
		reference.Child("Escolas").Child(key).Child("Turnos").Child("Tarde").SetValueAsync(turnoTarde.isOn);
		reference.Child("Escolas").Child(key).Child("Turnos").Child("Noite").SetValueAsync(turnoNoite.isOn);

		reference.Child("Escolas").Child(key).Child("Atendimento").Child("Infantil").SetValueAsync(eduInfantil.isOn);
		reference.Child("Escolas").Child(key).Child("Atendimento").Child("Inicial").SetValueAsync(eduInicial.isOn);
		reference.Child("Escolas").Child(key).Child("Atendimento").Child("Final").SetValueAsync(eduFinal.isOn);
		reference.Child("Escolas").Child(key).Child("Atendimento").Child("Medio").SetValueAsync(eduMedio.isOn);

		reference.Child("Escolas").Child(key).Child("numInfantil").SetValueAsync(numInfantil.text);
		reference.Child("Escolas").Child(key).Child("numInicial").SetValueAsync(numInicial.text);
		reference.Child("Escolas").Child(key).Child("numFinal").SetValueAsync(numFinal.text);
		reference.Child("Escolas").Child(key).Child("numMedio").SetValueAsync(numMedio.text);
		reference.Child("Escolas").Child(key).Child("numProfessores").SetValueAsync(numProfessores.text);
		reference.Child("Escolas").Child(key).Child("numFuncionarios").SetValueAsync(numFuncionarios.text);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
