using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Script_Cadastra_Professor : MonoBehaviour {

	DatabaseReference reference;

	public InputField nome;
	public InputField telefone;
	public InputField email;
	
	public Toggle graduacao;
	public Toggle especializacao;
	public Toggle mestDoutorado;

	// Use this for initialization
	void Start () {
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
		{
    		var dependencyStatus = task.Result;
    
    		if (dependencyStatus == Firebase.DependencyStatus.Available) 
			{
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
		string key = reference.Child("Professores").Push().Key;

		reference.Child("Professores").Child(key).Child("Nome").SetValueAsync(nome.text);
		reference.Child("Professores").Child(key).Child("Telefone").SetValueAsync(telefone.text);
		reference.Child("Professores").Child(key).Child("email").SetValueAsync(email.text);
			
		reference.Child("Professores").Child(key).Child("Formacao").Child("Graduacao").SetValueAsync(graduacao.isOn);
		reference.Child("Professores").Child(key).Child("Formacao").Child("Especializacao").SetValueAsync(especializacao.isOn);
		reference.Child("Professores").Child(key).Child("Formacao").Child("MestradoDoutorado").SetValueAsync(mestDoutorado.isOn);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
