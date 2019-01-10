using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Script_Cadastra_Turma : MonoBehaviour {

	DatabaseReference reference;

	public InputField identificacao;
	public InputField serie;
	public InputField horario;

	string s = "";

	public Dropdown drop_professores;

	List<string> listaProf = new List<string>();

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

    		} else {
   					 UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    				// Firebase Unity SDK is not safe to use here.
    			}
    	});

		FirebaseDatabase.DefaultInstance.GetReference("Professores").OrderByChild("Nome").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
          if (e2.DatabaseError != null) {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values for teachers.");
          
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children) { //percorre o snapshot do inicio ao fim
              if (childSnapshot.Child("Nome") == null || childSnapshot.Child("Nome").Value == null) {
                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                break;
              } else {
                Debug.Log("Leaders entry : "+ childSnapshot.Child("Nome").Value.ToString() + " - "
				+ childSnapshot.Child("Telefone").Value.ToString() + " - "+ childSnapshot.Child("email").Value.ToString());

				s = childSnapshot.Child("Nome").Value.ToString();			
							
				listaProf.Add(s);//adiciona o nome do professor a lista
				
              }
            }
			//apaga itens do dropdown dos professores
			drop_professores.ClearOptions();
			// adiciona os professores que estao casdastrados no banco ao dropdown
			drop_professores.AddOptions(listaProf);
          }
        };
	}

	public void salvar()
	{

		Debug.Log(drop_professores.options[drop_professores.value].text);  

		 
		string key = reference.Child("Turmas").Push().Key;
		//utiliza referencia de turma do database para cadastrar uma nova turma
		reference.Child("Turmas").Child(key).Child("Identificacao").SetValueAsync(identificacao.text);
		reference.Child("Turmas").Child(key).Child("Serie").SetValueAsync(serie.text);
		reference.Child("Turmas").Child(key).Child("Horario").SetValueAsync(horario.text);
		reference.Child("Turmas").Child(key).Child("Professor").SetValueAsync(drop_professores.options[drop_professores.value].text);
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
