using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFirebase : MonoBehaviour
{
  Firebase.FirebaseApp app;
  Firebase.Auth.FirebaseAuth auth;
  Firebase.Auth.FirebaseUser user;

  private void Awake()
  {
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
    {
      var dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available)
      {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
#if UNITY_EDITOR
        app = Firebase.FirebaseApp.Create();
#else
        app = Firebase.FirebaseApp.DefaultInstance;
#endif
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        Debug.Log("Firebase is ready");
        // Set a flag here to indicate whether Firebase is ready to use by your app.
      }
      else
      {
        UnityEngine.Debug.LogError(System.String.Format(
          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
      }
    });
  }
  
  // Track state changes of the auth object.
  void AuthStateChanged(object sender, System.EventArgs eventArgs)
  {
    if (auth.CurrentUser != user)
    {
      bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
      if (!signedIn && user != null)
      {
        Debug.Log("Signed out " + user.UserId);
      }
      user = auth.CurrentUser;
      if (signedIn)
      {
        Debug.Log("Signed in " + user.UserId);
      }
    }
  }

  void OnDestroy()
  {
    auth.StateChanged -= AuthStateChanged;
    auth = null;
  }

  public void CreateUserWithEmailAndPassword(string email, string password)
  {
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
    {
      if (task.IsCanceled)
      {
        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        return;
      }
      if (task.IsFaulted)
      {
        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        return;
      }

      // Firebase user has been created.
      Firebase.Auth.FirebaseUser newUser = task.Result;
      Debug.LogFormat("Firebase user created successfully: {0} ({1})",
          newUser.DisplayName, newUser.UserId);
    });
  }

  public void SigninWithEmailAndPassword(string email, string password)
  {
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    Firebase.Auth.Credential credential =
    Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
    auth.SignInWithCredentialAsync(credential).ContinueWith(task => 
    {
      if (task.IsCanceled)
      {
        Debug.LogError("SignInWithCredentialAsync was canceled.");
        return;
      }
      if (task.IsFaulted)
      {
        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
        return;
      }

      user = task.Result;
      Debug.Log("User signed in successfully: " + user.DisplayName + ", ID: " + user.UserId + ", Email: " + user.Email + "Photo: " + user.PhotoUrl);
    });
  }

}
