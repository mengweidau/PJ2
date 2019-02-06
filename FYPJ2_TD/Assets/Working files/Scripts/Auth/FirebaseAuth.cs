using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "FirebaseAuth", menuName = "FirebaseAuth")]
public class FirebaseAuth : ScriptableObject
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser firebaseUser;
    DatabaseReference mDatabaseRef;
    DataSnapshot mSnapshot;
    
    public Firebase.Auth.FirebaseAuth Auth
    {
        get
        {
            if (auth == null)
                InitializeFirebase();

            return auth;
        }
    }

    public DatabaseReference Ref
    {
        get
        {
            if (mDatabaseRef == null)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://fyp2-td.firebaseio.com/");
                mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            return mDatabaseRef;
        }
    }

    public DataSnapshot SnapShot
    {
        get
        {
            FetchSnapshot();
            return mSnapshot;
        }
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (Auth.CurrentUser != firebaseUser)
        {
            bool signedIn = firebaseUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && firebaseUser != null)
            {
                Debug.Log("Signed out " + firebaseUser.UserId);
            }
            firebaseUser = Auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + firebaseUser.UserId);
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void SignoutUser()
    {
        auth.SignOut();
    }
    
    public void WriteNewUser()
    {
        int defaultNum = 0;
        if (firebaseUser == null || Ref == null)
            return;

        int numOfLvls = 2;
        for (int i = 1; i < numOfLvls + 1; ++i)
        {
            Ref.Child("users").Child(firebaseUser.UserId).Child("Levels").Child("level" + i).Child("cleared").SetValueAsync(defaultNum);
            Ref.Child("users").Child(firebaseUser.UserId).Child("Levels").Child("level" + i).Child("stars").SetValueAsync(defaultNum);
        }
        Ref.Child("users").Child(firebaseUser.UserId).Child("Currency").Child("gems").SetValueAsync(defaultNum);
        Ref.Child("users").Child(firebaseUser.UserId).Child("Spells").Child("fire").SetValueAsync(defaultNum);
        Ref.Child("users").Child(firebaseUser.UserId).Child("Spells").Child("freeze").SetValueAsync(defaultNum);
    }
    
    public void FetchSnapshot()
    {
        Ref.Child("users").Child(firebaseUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("task.IsFaulted");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("task.IsCanceled");
            }
            else if (task.IsCompleted)
            {
                mSnapshot = task.Result;
            }
        });
    }

    // Update level completed status for input level
    public void UpdateLevelCleared(int _level, int _cleared)
    {
        if (firebaseUser == null || Ref == null)
            return;
        
        Ref.Child("users").Child(firebaseUser.UserId).Child("Levels").Child("level" + _level).Child("cleared").SetValueAsync(_cleared);
    }

    // Update stars for input level
    public void UpdateLevelStars(int _level, int _stars)
    {
        if (firebaseUser == null || Ref == null)
            return;

        Ref.Child("users").Child(firebaseUser.UserId).Child("Levels").Child("level" + _level).Child("stars").SetValueAsync(_stars);
    }

    // Update gems
    public void UpdateGems(int _gem)
    {
        if (firebaseUser == null || Ref == null)
            return;
        
        Ref.Child("users").Child(firebaseUser.UserId).Child("Currency").Child("gems").SetValueAsync(_gem);
    }

    // Update spells
    public void UpdateSpells(string _spell, int _unlocked)
    {
        if (firebaseUser == null || Ref == null)
            return;

        Ref.Child("users").Child(firebaseUser.UserId).Child("Spells").Child(_spell).SetValueAsync(_unlocked);
    }

    // Fetch level completed status for input level
    public bool FetchLevelCheck(int _level)
    {
        bool returnCheck = false;
        
        int check = System.Convert.ToInt32(SnapShot.Child("Levels").Child("level" + _level).Child("cleared").Value);
        if (check == 1)
            returnCheck = true;

        return returnCheck;
    }

    // Fetch number of stars for input level
    public int FetchStars(int _level)
    {
        return System.Convert.ToInt32(SnapShot.Child("Levels").Child("level" + _level).Child("stars").Value);
    }

    // Fetch number of gems
    public int FetchGems()
    {
        return System.Convert.ToInt32(SnapShot.Child("Currency").Child("gems").Value);
    }

    public int FetchSpellStatus(string _spell)
    {
        return System.Convert.ToInt32(SnapShot.Child("Spells").Child(_spell).Value);
    }

    public void CreateUser(string email, string password)
    {
        //check whether email is valid, password is valid
        //create user if valid email/password
        //prompt user to re-input textfields with proper requirements

        Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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
            firebaseUser = newUser;
            WriteNewUser();
        });
    }

    public void SignInUser(string _email, string _password)
    {
        Auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(task => {
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

            firebaseUser = newUser;
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        });
    }
}
