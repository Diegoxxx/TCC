using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
	
public class Script_Teste_firebase : MonoBehaviour {

	public string username;
  public string email;
  DatabaseReference reference;
  Firebase.Auth.FirebaseAuth auth;

	// Use this for initialization
	void Start () {
    
    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    auth.SignInAnonymouslyAsync().ContinueWith(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInAnonymouslyAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
    return;
  }

  Firebase.Auth.FirebaseUser newUser = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);
});

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
	public void teste(){

    reference.Child("users").Child("ID").SetValueAsync("Diego");

  }

	// Update is called once per frame
	void Update () {
		// Handle screen touches.
        
	}
}
