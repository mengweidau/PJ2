using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class FirebaseInit : MonoBehaviour
{
    protected Firebase.Auth.FirebaseAuth auth;
    string email = "";
    string password = "";


    // Use this for initialization
    void Start()
    {

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //Create and hold a reference to your FirebaseApp, i.e.
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width * .25f, 0, Screen.width * .5f, Screen.height * .25f));
        GUILayout.FlexibleSpace();

        //Create user
        GUILayout.BeginHorizontal();
        GUILayout.Label("email:", GUILayout.Width(150));
        email = GUILayout.TextField(email);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Password:", GUILayout.Width(150));
        password = GUILayout.TextField(password);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create User"))
        {
            CreateUser();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Sign In"))
        {
            SignInWithEmailAndPassword();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public void CreateUser()
    {
        Debug.Log(email);
        Debug.Log(password);
        //check whether email is valid, password is valid
        //create user if valid email/password
        //prompt user to re-input textfields with proper requirements

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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

    public void SignInWithEmailAndPassword()
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        });
    }
}
