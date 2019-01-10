using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class Script_Cadastra_Modalidade : MonoBehaviour {

	DatabaseReference reference;

	public Slider slider_coopProt;
	public Slider slider_respeito;
	public Slider slider_desMotor;
	public Slider slider_desTatico;

	public Dropdown drop_modalidade;

	public GameObject popup;

	public Text soma;
	float valorMax;

	public class Modalidade
	{
		public string nome { get; set; }
		public int Cooperacao { get; set; }
		public int Respeito { get; set; }
		public int DesempenhoMotor { get; set; }
		public int DesempenhoTatico { get; set; }

	}

	public List<Modalidade> vet = new List<Modalidade>();

	public void atualiza_soma()
	{	
		float aux = slider_respeito.value + slider_coopProt.value + slider_desMotor.value + slider_desTatico.value;
		soma.text = "Soma: " + aux + "0";
	}

	public void fechar_popup()
	{
		popup.SetActive(false);
	}

	public void salvar()
	{	
		valorMax = slider_respeito.value + slider_coopProt.value + slider_desMotor.value + slider_desTatico.value;

		if(valorMax != 10.0)
		{	
			popup.SetActive(true);
			return;
		}

		Debug.Log("passou");

		reference.Child("Modalidades").Child("" + drop_modalidade.value).Child("Nome").SetValueAsync(drop_modalidade.options[drop_modalidade.value].text);
		reference.Child("Modalidades").Child("" + drop_modalidade.value).Child("Respeito").SetValueAsync(slider_respeito.value);
		reference.Child("Modalidades").Child("" + drop_modalidade.value).Child("DesempenhoMotor").SetValueAsync(slider_desMotor.value);
		reference.Child("Modalidades").Child("" + drop_modalidade.value).Child("Cooperacao").SetValueAsync(slider_coopProt.value);
		reference.Child("Modalidades").Child("" + drop_modalidade.value).Child("DesempenhoTatico").SetValueAsync(slider_desTatico.value);

	}

	public void atualizar_Sliders()
	{

		foreach(Modalidade mod in vet)
		{
			if(mod.nome.Equals(drop_modalidade.options[drop_modalidade.value].text))
			{
				slider_coopProt.value = mod.Cooperacao;
				slider_respeito.value = mod.Respeito;
				slider_desMotor.value = mod.DesempenhoMotor;
				slider_desTatico.value = mod.DesempenhoTatico;
			}
		}

	}

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

		FirebaseDatabase.DefaultInstance.GetReference("Modalidades").OrderByChild("Nome").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
          if (e2.DatabaseError != null) {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values");
          
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children) { //percorre o snapshot do inicio ao fim
              if (childSnapshot.Child("Nome") == null || childSnapshot.Child("Nome").Value == null) {
                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                break;
              } else {
				/* Debug.Log("Leaders entry : "
				        + childSnapshot.Child("Nome").Value.ToString() 
				+ " - "	+ childSnapshot.Child("Cooperacao").Value.ToString() 
				+ " - " + childSnapshot.Child("Respeito").Value.ToString()
				+ " - " + childSnapshot.Child("DesempenhoMotor").Value.ToString()
				+ " - " + childSnapshot.Child("DesempenhoTatico").Value.ToString());
				*/                
				vet.Add(new Modalidade(){nome = childSnapshot.Child("Nome").Value.ToString(),
										 Cooperacao = Convert.ToInt32(childSnapshot.Child("Cooperacao").Value.ToString()),
										 Respeito = Convert.ToInt32(childSnapshot.Child("Respeito").Value.ToString()),
										 DesempenhoMotor = Convert.ToInt32(childSnapshot.Child("DesempenhoMotor").Value.ToString()),
										 DesempenhoTatico = Convert.ToInt32(childSnapshot.Child("DesempenhoTatico").Value.ToString())
										});

              }
            }
          }
        };
		float aux = slider_respeito.value + slider_coopProt.value + slider_desMotor.value + slider_desTatico.value;
		soma.text = "Soma: " + aux + "0";
	}

	// Update is called once per frame
	void Update () {
		//Debug.LogError("tamanho lista: " + vet.Count);
	}
}
