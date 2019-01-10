using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Globalization;

public class Script_Cadastra_Aluno : MonoBehaviour {

	DatabaseReference reference;
	
	List<string> listaTurma = new List<string>();
	string s = "";

	public Dropdown drop_Turmas,drop_Alunos;
	int indice_Aluno;
	public InputField nome,idade,data_nasc;

	public Toggle masculino,feminino;

	public GameObject popup;

	public List<Aluno> vet = new List<Aluno>();

	public class Aluno{

		public string nome_aluno { get; set; }
		public string sexo_aluno { get; set; }
		public string turma_aluno { get; set; }
		public int idade_aluno { get; set; }
		public string data_nascimento { get; set; }
		
	}
	public void atualizar_Campos()
	{
		indice_Aluno = drop_Alunos.value;
		int i = 0;
		//foreach(Aluno aluno in vet)
		//{
		//	if(aluno.nome_aluno.Equals(drop_Alunos.options[drop_Alunos.value].text))
		//	{
				nome.enabled = true;
				
				nome.text = vet[drop_Alunos.value].nome_aluno;
				foreach(string s in listaTurma)
				{	
					if(vet[drop_Alunos.value].turma_aluno.Equals(s)){
						drop_Turmas.value = i;
						break;
					}						
					i++;
				}

				if(vet[drop_Alunos.value].sexo_aluno.Equals("M")){
					masculino.isOn = true;
					Debug.Log("M");
				}					
				else{
					feminino.isOn = true;
					Debug.Log("F");
				}
					

				idade.text = vet[drop_Alunos.value].idade_aluno.ToString();
				data_nasc.text = vet[drop_Alunos.value].data_nascimento;

				nome.enabled = false;
				//break;
		//	}
			
		//}

	}

	// Use this for initialization
	void Start () {
		nome.enabled = false;
		indice_Aluno = drop_Alunos.value;
		//drop_Alunos.ClearOptions();

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

		FirebaseDatabase.DefaultInstance.GetReference("Turmas").OrderByChild("Professor").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
          if (e2.DatabaseError != null) {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values for teachers.");
          
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children) { //percorre o snapshot do inicio ao fim
              if (childSnapshot.Child("Professor") == null || childSnapshot.Child("Professor").Value == null) {
                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                break;
              } else {
                Debug.Log("Leaders entry : "+ childSnapshot.Child("Professor").Value.ToString() + " - "
				+ childSnapshot.Child("Horario").Value.ToString() + " - "+ childSnapshot.Child("Identificacao").Value.ToString());

				s = childSnapshot.Child("Identificacao").Value.ToString();			
							
				listaTurma.Add(s);//adiciona nom da turma a lista
				
              }
            }
			//apaga itens do dropdown
			drop_Turmas.ClearOptions();
			// adiciona as turmas que estao casdastrados no banco ao dropdown
			drop_Turmas.AddOptions(listaTurma);
          }
        };
		
		FirebaseDatabase.DefaultInstance.GetReference("Alunos").OrderByChild("id").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
          if (e2.DatabaseError != null) {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values");
          vet = new List<Aluno>();
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children) { //percorre o snapshot do inicio ao fim
              if (childSnapshot.Child("Nome") == null || childSnapshot.Child("Nome").Value == null) {
                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                break;
              } else {
				         
				vet.Add(new Aluno(){nome_aluno = childSnapshot.Child("Nome").Value.ToString(),
								    idade_aluno = Convert.ToInt32(childSnapshot.Child("Idade").Value.ToString()),
									turma_aluno = childSnapshot.Child("Turma").Value.ToString(),
									data_nascimento = childSnapshot.Child("DataNascimento").Value.ToString(),
									sexo_aluno = childSnapshot.Child("Sexo").Value.ToString()
									});				
              }
            }
			drop_Alunos.ClearOptions();
			foreach(Aluno aluno in vet)
			{
				drop_Alunos.options.Add (new Dropdown.OptionData() {text= aluno.nome_aluno});
			}			
          }
        };
		

	}

	public void formato_data()
	{
		// 29/12/1995
		DateTime dDate;

		if (DateTime.TryParseExact(data_nasc.text, "dd/MM/yyyy", null,DateTimeStyles.None, out dDate))
		{
 			    
			Debug.Log("Converted " + data_nasc.text + " to "+ dDate);
				return;
		}
		else
		{
   			 popup.SetActive(true);
				return;
		}
	}

	public void fechar_popup()
	{
		popup.SetActive(false);
	}

	public void novo_aluno()
	{
		indice_Aluno = vet.Count;
		nome.enabled = true;
	}

	public void Salvar()
	{	
		reference.Child("Alunos").Child("" + indice_Aluno).Child("id").SetValueAsync(indice_Aluno);
		reference.Child("Alunos").Child("" + indice_Aluno).Child("Nome").SetValueAsync(nome.text);
		reference.Child("Alunos").Child("" + indice_Aluno).Child("Turma").SetValueAsync(drop_Turmas.options[drop_Turmas.value].text);
		reference.Child("Alunos").Child("" + indice_Aluno).Child("Idade").SetValueAsync(idade.text);

		if(masculino.isOn)
			reference.Child("Alunos").Child("" + indice_Aluno).Child("Sexo").SetValueAsync("M");
		else
			reference.Child("Alunos").Child("" + indice_Aluno).Child("Sexo").SetValueAsync("F");
	
		reference.Child("Alunos").Child("" + indice_Aluno).Child("DataNascimento").SetValueAsync(data_nasc.text);
		
		nome.enabled = false;
	}

	public void controle_masc()
	{
		if(masculino.isOn)
			feminino.isOn = false;
	}
	public void controle_femi()
	{
		if(feminino.isOn)
			masculino.isOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
