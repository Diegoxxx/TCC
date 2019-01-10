using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class Script_avaliacao_aluno : MonoBehaviour {

	DatabaseReference reference;

	public InputField data_aval,temperatura,hora,peso,estatura,cintura,
	envergadura,freq_cardia,indice_MC,agilidade,velocidade,flexibilidade,apt_cardio,resis_musc,
	forca_inferior,forca_superior;

	public Dropdown drop_Alunos;

	public List<Aluno> vet = new List<Aluno>();

	public class Avaliacao{
		
		string data_avaliacao { get; set; }
		float temp_aluno { get; set; }
		string hora_avaliacao { get; set; }
		float peso_aluno { get; set; }
		float estatura_aluno { get; set; }
		float circ_cintura { get; set; }
		float enverg_aluno { get; set; }
		float freq_cardi_aluno { get; set; }
		float imc_aluno { get; set; }
		float agil_aluno { get; set; }
		float velo_aluno { get; set; }
		float flex_aluno { get; set; }
		float apti_aluno { get; set; }
		float resi_aluno { get; set; }
		float for_inf_aluno { get; set; }
		float for_sup_aluno { get; set; }	
	}

	public class Aluno{

		public string nome_aluno { get; set; }
		public string sexo_aluno { get; set; }
		public string turma_aluno { get; set; }
		public int idade_aluno { get; set; }
		public string data_nascimento { get; set; }
		
	}
	// Use this for initialization
	void Start () {
		data_aval.text = System.DateTime.UtcNow.ToString("dd/MM/yyyy");
		hora.text = System.DateTime.UtcNow.ToString("HH:mm");
		//Debug.Log(System.DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"));

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

	public void calcula_IMC()
	{
		
		float altura, peso1 = 0;
		string aux = "";
		if(!estatura.text.Equals("") & !peso.text.Equals(""))
		{
			aux = estatura.text;

			altura = Convert.ToSingle(aux);
			altura = altura * altura;

			aux = peso.text;
	
			peso1 = Convert.ToSingle(aux);
			peso1 = peso1/altura;

			indice_MC.text = ""+ peso1;
		}
	}



	public void Salvar()
	{	
		double controle_aluno = 0;
		//de acordo com a idade faz o switch
		switch (vet[drop_Alunos.value].idade_aluno)
      		{
         		case 7://idade
				 	if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))//sexo mascolino
					 {
						if(Convert.ToSingle(indice_MC.text) == 17.8)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 768)//Apt cardirespiratoria				
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 22)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 164 & Convert.ToSingle(forca_superior.text) <= 201)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 202)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 111 & Convert.ToSingle(forca_inferior.text) <= 133)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 134)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 7.76 & Convert.ToSingle(agilidade.text) >= 7.01)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 7.00)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.62 & Convert.ToSingle(velocidade.text) >= 4.13)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 4.14)
								controle_aluno++;

					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 17.1)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 715)//Apt cardirespiratoria					
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 153 & Convert.ToSingle(forca_superior.text) <= 179)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 180)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 94 & Convert.ToSingle(forca_inferior.text) <= 115)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 116)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 8.41 & Convert.ToSingle(agilidade.text) >= 7.57)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 7.56)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 5.07 & Convert.ToSingle(velocidade.text) >= 4.48)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 4.49)
								controle_aluno++;
					 }
         		    break;

        		case 8://idade
         		    if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 19.2)//IMC			
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 768)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 22)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 180 & Convert.ToSingle(forca_superior.text) <= 224)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 225)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 118 & Convert.ToSingle(forca_inferior.text) <= 139)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 140)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 7.59 & Convert.ToSingle(agilidade.text) >= 6.79)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 7.78)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.47 & Convert.ToSingle(velocidade.text) >= 4.01)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 4.00)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 18.2)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 715)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 167 & Convert.ToSingle(forca_superior.text) <= 199)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 200)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 105 & Convert.ToSingle(forca_inferior.text) <= 126)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 127)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.98 & Convert.ToSingle(agilidade.text) >= 7.23)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 7.22)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.75 & Convert.ToSingle(velocidade.text) >= 4.28)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 4.27)
								controle_aluno++;
					 }
           		    break;

				case 9://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 19.3)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 820)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 22)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 22)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 200 & Convert.ToSingle(forca_superior.text) <= 249)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 250)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 129 & Convert.ToSingle(forca_inferior.text) <= 151)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 152)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 7.19 & Convert.ToSingle(agilidade.text) >= 6.51)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.50)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.31 & Convert.ToSingle(velocidade.text) >= 3.89)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <=3.88)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 19.1)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 780)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 185 & Convert.ToSingle(forca_superior.text) <= 225)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 226)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 116 & Convert.ToSingle(forca_inferior.text) <= 139)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 140)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.63 & Convert.ToSingle(agilidade.text) >= 6.90)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.89)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.54 & Convert.ToSingle(velocidade.text) >= 4.01)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 4.00)
								controle_aluno++;
					 }
           			   break;

				case 10://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 20.7)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 856)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 22)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 22)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 213 & Convert.ToSingle(forca_superior.text) <= 269)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 270)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 135 & Convert.ToSingle(forca_inferior.text) <= 157)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 158)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 7.00 & Convert.ToSingle(agilidade.text) >= 6.26)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.25)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.15 & Convert.ToSingle(velocidade.text) >= 3.75)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.74)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 20.9)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 820)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 200 & Convert.ToSingle(forca_superior.text) <= 244)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 245)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 123 & Convert.ToSingle(forca_inferior.text) <= 145)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 146)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.35 & Convert.ToSingle(agilidade.text) >= 6.61)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.60)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.41 & Convert.ToSingle(velocidade.text) >= 3.98)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <=3.97)
								controle_aluno++;
					 }
           			   break;

				case 11://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.1)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 955)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 21)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 25)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 238 & Convert.ToSingle(forca_superior.text) <= 293)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 294)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 140 & Convert.ToSingle(forca_inferior.text) <= 164)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 165)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.87 & Convert.ToSingle(agilidade.text) >= 6.11)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.10)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.03 & Convert.ToSingle(velocidade.text) >= 3.63)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.62)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.3)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 915)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 220 & Convert.ToSingle(forca_superior.text) <= 276)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 277)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 127 & Convert.ToSingle(forca_inferior.text) <= 149)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 150)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.24 & Convert.ToSingle(agilidade.text) >= 6.50)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.49)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.31 & Convert.ToSingle(velocidade.text) >= 3.88)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.87)
								controle_aluno++;
					 }
           			   break;

				case 12://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.2)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 996)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 19)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 30)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 269 & Convert.ToSingle(forca_superior.text) <= 329)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 330)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 149 & Convert.ToSingle(forca_inferior.text) <= 173)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 174)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.70 & Convert.ToSingle(agilidade.text) >= 6.01)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.00)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.96 & Convert.ToSingle(velocidade.text) >= 3.51)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.50)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.6)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 960)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 20)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 241 & Convert.ToSingle(forca_superior.text) <= 299)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 300)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 130 & Convert.ToSingle(forca_inferior.text) <= 154)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 155)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.17 & Convert.ToSingle(agilidade.text) >= 6.37)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.36)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.25 & Convert.ToSingle(velocidade.text) >= 3.79)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.78)
								controle_aluno++;
					 }
           			   break;

				case 13://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1050)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 35)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 300 & Convert.ToSingle(forca_superior.text) <= 389)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 390)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 159 & Convert.ToSingle(forca_inferior.text) <= 184)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 185)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.54 & Convert.ToSingle(agilidade.text) >= 5.87)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 5.86)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.81 & Convert.ToSingle(velocidade.text) >= 3.38)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <=3.37)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1015)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 23)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 265 & Convert.ToSingle(forca_superior.text) <= 322)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 323)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 133 & Convert.ToSingle(forca_inferior.text) <= 159)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 160)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.10 & Convert.ToSingle(agilidade.text) >= 6.29)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.28)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.19 & Convert.ToSingle(velocidade.text) >= 3.72)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.71)
								controle_aluno++;
					 }
           			   break;

				case 14://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.2)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1100)//Apt cardirespiratoria					
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 18)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 35)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 350 & Convert.ToSingle(forca_superior.text) <= 449)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 450)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 170 & Convert.ToSingle(forca_inferior.text) <= 199)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 200)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.37 & Convert.ToSingle(agilidade.text) >= 5.70)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 5.69)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.67 & Convert.ToSingle(velocidade.text) >= 3.24)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.23)
								controle_aluno++;
					 }else//sexo feminino
					 {
						if(Convert.ToSingle(indice_MC.text) == 22.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1060)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 23)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 280 & Convert.ToSingle(forca_superior.text) <= 343)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 344)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 134 & Convert.ToSingle(forca_inferior.text) <= 160)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 161)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.03 & Convert.ToSingle(agilidade.text) >= 6.23)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.22)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.21 & Convert.ToSingle(velocidade.text) >= 3.71)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.70)
								controle_aluno++;
					 }
           			   break;

				case 15://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 23.0)//IMC				
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1155)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 19)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 35)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 400 & Convert.ToSingle(forca_superior.text) <= 499)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 500)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 180 & Convert.ToSingle(forca_inferior.text) <= 209)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 210)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.26 & Convert.ToSingle(agilidade.text) >= 5.60)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 5.59)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.60 & Convert.ToSingle(velocidade.text) >= 3.17)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.16)
								controle_aluno++;
					 }else//sexo feminino
					 {
						 if(Convert.ToSingle(indice_MC.text) == 22.4)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1120)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 23)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 300 & Convert.ToSingle(forca_superior.text) <= 359)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 360)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 135 & Convert.ToSingle(forca_inferior.text) <= 162)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 163)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 7.00 & Convert.ToSingle(agilidade.text) >= 6.20)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.19)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.25 & Convert.ToSingle(velocidade.text) >= 3.73)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.72)
								controle_aluno++;
					 }
           			   break;

				case 16://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 24.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1190)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 40)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 453 & Convert.ToSingle(forca_superior.text) <= 552)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 553)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 186 & Convert.ToSingle(forca_inferior.text) <= 214)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 215)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <= 6.10 & Convert.ToSingle(agilidade.text) >= 5.43)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 5.42)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.50 & Convert.ToSingle(velocidade.text) >= 3.13)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.12)
								controle_aluno++;
					 }else//sexo feminino
					 {
						 if(Convert.ToSingle(indice_MC.text) == 24.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1160)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 23)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 320 & Convert.ToSingle(forca_superior.text) <= 369)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 370)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 131 & Convert.ToSingle(forca_inferior.text) <= 158)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 159)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.94 & Convert.ToSingle(agilidade.text) >= 6.16)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.15)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.23 & Convert.ToSingle(velocidade.text) >= 3.71)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <=3.70)
								controle_aluno++;
					 }
           			   break;

				case 17://idade
         		     if(vet[drop_Alunos.value].sexo_aluno.Equals("M"))
					 {
						if(Convert.ToSingle(indice_MC.text) == 25.4)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1190)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 45)//Resistência Muscular Localizada				
							controle_aluno++;
						
						if(Convert.ToSingle(forca_superior.text) >= 480 & Convert.ToSingle(forca_superior.text) <= 589)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 590)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 186 & Convert.ToSingle(forca_inferior.text) <= 219)// Força inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 220)
								controle_aluno++;
						
						if(Convert.ToSingle(agilidade.text) <= 6.03 & Convert.ToSingle(agilidade.text) >= 5.44)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 4.43)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 3.53 & Convert.ToSingle(velocidade.text) >= 3.13)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.12)
								controle_aluno++;
					 }else//sexo feminino
					 {
						 if(Convert.ToSingle(indice_MC.text) == 24.0)//IMC					
							controle_aluno++;
						if(Convert.ToSingle(apt_cardio.text) >= 1160)//Apt cardirespiratoria						
							controle_aluno++;
						if(Convert.ToSingle(flexibilidade.text) >= 20)//Flexibilidade				
							controle_aluno++;
						if(Convert.ToSingle(resis_musc.text) >= 23)//Resistência Muscular Localizada				
							controle_aluno++;

						if(Convert.ToSingle(forca_superior.text) >= 310 & Convert.ToSingle(forca_superior.text) <= 374)// Força superiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_superior.text) >= 375)
								controle_aluno++;
						
						if(Convert.ToSingle(forca_inferior.text) >= 121 & Convert.ToSingle(forca_inferior.text) <= 152)// Força  inferiores
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(forca_inferior.text) >= 153)
								controle_aluno++;

						if(Convert.ToSingle(agilidade.text) <=7.00 & Convert.ToSingle(agilidade.text) >= 6.23)// Agilidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(agilidade.text) <= 6.22)
								controle_aluno++;
						
						if(Convert.ToSingle(velocidade.text) <= 4.32 & Convert.ToSingle(velocidade.text) >= 3.80)// velocidade
							controle_aluno += 0.5;
						else 
							if(Convert.ToSingle(velocidade.text) <= 3.79)
								controle_aluno++;
					 }
           			   break;
				default:
          		    controle_aluno = 100000;
           		    break;
      		}
		reference.Child("Avaliacao").Child(data_aval.text).Child("Hora").SetValueAsync(hora.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Aluno").SetValueAsync(drop_Alunos.options[drop_Alunos.value].text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Peso").SetValueAsync(peso.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Estatura").SetValueAsync(estatura.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Cintura").SetValueAsync(cintura.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Envergadura").SetValueAsync(envergadura.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("FreqCardiaca").SetValueAsync(freq_cardia.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Temperatura").SetValueAsync(temperatura.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("IMC").SetValueAsync(indice_MC.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Agilidade").SetValueAsync(agilidade.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Velocidade").SetValueAsync(velocidade.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("Flexibilidade").SetValueAsync(flexibilidade.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("AptidaoCardio").SetValueAsync(apt_cardio.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("ResistenciMuscular").SetValueAsync(resis_musc.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("ForcaInferior").SetValueAsync(forca_inferior.text);
		reference.Child("Avaliacao").Child(data_aval.text).Child("ForcaSuperior").SetValueAsync(forca_superior.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
