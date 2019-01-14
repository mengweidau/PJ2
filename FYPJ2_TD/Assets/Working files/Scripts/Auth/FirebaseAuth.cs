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

    public class User
    {
        public string username;
        public int val;

        public User()
        {
        }

        public User(string _name, int _val)
        {
            this.username = _name;
            this.val = _val;
        }
    }


    public Firebase.Auth.FirebaseAuth GetAuth()
    {
        if (auth == null)
            InitializeFirebase();

        return auth;
    }

    public DatabaseReference GetReference()
    {
        if (mDatabaseRef == null)
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://fyp2-td.firebaseio.com/");
            mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        }
        return mDatabaseRef;
    }

    //public Firebase.Auth.FirebaseAuth Auth
    //{
    //    get
    //    {
    //        if (auth == null)
    //        {
    //            InitializeFirebase();
    //        }

    //        return auth;
    //    }
    //}


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
        if (auth.CurrentUser != firebaseUser)
        {
            bool signedIn = firebaseUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && firebaseUser != null)
            {
                Debug.Log("Signed out " + firebaseUser.UserId);
            }
            firebaseUser = auth.CurrentUser;
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
    
    public void writeNewUser(string _name, int _val)
    {
        if (firebaseUser == null || mDatabaseRef == null)
            return;

        User user = new User(_name, _val);
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("users").Child(firebaseUser.UserId).SetRawJsonValueAsync(json);
    }

    public void FetchUserData()
    {
        if (firebaseUser == null || mDatabaseRef == null)
            return;

        mDatabaseRef.Child("users").Child(firebaseUser.UserId).GetValueAsync().ContinueWith(task => 
        {
          if (task.IsFaulted)
          {
              // Handle the error...
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                Debug.Log("FOUND DATABASE");
                Debug.Log("username: " + snapshot.Child("username").Value.ToString());
                Debug.Log("val: " + snapshot.Child("val").Value.ToString());

                /*
                 * Dictionary<string, System.Object> datalist = (Dictionary<string, System.Object>)task.Result.Value;
                // Do something with snapshot...
                Debug.Log("FOUND DATABASE");
                Debug.Log("username id : " + (string)datalist["username"]);
                Debug.Log("val : " + System.Convert.ToUInt32( datalist["val"]));
                //Debug.Log("username: " + snapshot.Child("username").Value.ToString());
                //Debug.Log("val: " + snapshot.Child("val").Value.ToString());
                 */

                /*
                 Debug.Log("FOUND DATABASE");
                Debug.Log("username: " + snapshot.Child("username").Value.ToString());
                Debug.Log("val: " + snapshot.Child("val").Value.ToString());

                User user = JsonUtility.FromJson<User>(snapshot.Value.ToString());
                Debug.Log("username: " + user.username);
                Debug.Log("val: " + user.val);
                 */
            }
        });
    }

    public void CreateUser(string email, string password)
    {
        //check whether email is valid, password is valid
        //create user if valid email/password
        //prompt user to re-input textfields with proper requirements

        GetAuth().CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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
        });
    }

    public void SignInUser(string _email, string _password)
    {
        auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(task => {
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
