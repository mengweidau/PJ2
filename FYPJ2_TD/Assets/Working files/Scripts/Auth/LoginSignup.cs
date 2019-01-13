using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSignup : MonoBehaviour {
    [SerializeField] TMP_InputField emailInput, passwordInput;
    [SerializeField] FirebaseAuth fbAuth;

    // Use this for initialization
    void Start () {
        fbAuth.GetAuth();
        fbAuth.GetReference();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("inputs check: \nEmailInput: " + emailInput.text + "\nPasswordInput: " + passwordInput.text);
        }
    }

    public void Login()
    {
        if (emailInput.text.Length < 3)
        {
            //display email error text on gui
            return;
        }
        if (passwordInput.text.Length < 1)
        {
            //display password error text on gui
            return;
        }

        fbAuth.SignInUser(emailInput.text, passwordInput.text);
    }

    public void Register()
    {
        if (emailInput.text.Length < 3)
        {
            //display email error text on gui
            return;
        }
        if (passwordInput.text.Length < 1)
        {
            //display password error text on gui
            return;
        }
        
        fbAuth.CreateUser(emailInput.text, passwordInput.text);
    }

    public void Autofill()
    {
        emailInput.text = "test1@email.com";
        passwordInput.text = "12345678";
    }
}
