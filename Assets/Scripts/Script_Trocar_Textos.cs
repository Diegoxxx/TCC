using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Trocar_Textos : MonoBehaviour {

	public Text texto;
	public Slider slider;

	private float tempoAtual;

	public void atualiza_Texto(){

		texto.text = slider.value.ToString()+"0";

	}
}