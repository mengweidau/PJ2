using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSignup : MonoBehaviour {
    [SerializeField] TMP_InputField emailInput, passwordInput;
    [SerializeField] TextMeshProUGUI popupText;
    [SerializeField] FirebaseAuth fbAuth;

    float popupTimer = 0.0f;
    private void Start()
    {
        popupText.enabled = false;
    }

    private void Update()
    {
        if (popupTimer > 0.0f)
        {
            popupTimer -= 1 * Time.deltaTime;
            if (popupTimer <= 0.0f)
                popupText.enabled = false;
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
