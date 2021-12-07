using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using System;

public class AuthManager : MonoBehaviour
{
    public DependencyStatus _dependencyStatus;
    public FirebaseAuth _auth;
    public FirebaseUser _user;

    //Login Variables
    public InputField _emailInputField;
    public InputField _passwordInputField;
    public Text _warningLoginText;
    public Text _confirmLoginText;

    [Space]
    //Register Variables
    public InputField _usernameRegisterInputField;
    public InputField _emailRegisterInputField;
    public InputField _passwordRegisterInputField;
    public InputField _passwordRegisterVerifyInputField;
    public Text _warningRegisterText;
    public Text _confirmRegisterText;

    private void Awake()
    {
       // FirebaseApp app = FirebaseApp.DefaultInstance;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            if(_dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("cant Initialize");
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("setting up firebase Auth");
        _auth = FirebaseAuth.DefaultInstance;
    }
    public void LoginButton()
    {
        StartCoroutine(Login(_emailInputField.text, _passwordInputField.text));
    }

    IEnumerator Login(string inEmail, string inPassword)
    {
        var LoginTask = _auth.SignInWithEmailAndPasswordAsync(inEmail, inPassword);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed";
            switch (authError)
            {
                case AuthError.MissingEmail:
                    message = "Email Missing";
                    break;

                case AuthError.MissingPassword:
                    message = "Password Missing";
                    break;

                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;

                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User does not exists";
                    break;
            }
            _warningLoginText.text = message;
        }
        else
        {
            _user = LoginTask.Result;
            _warningLoginText.text = "";
            _confirmLoginText.text = "Successful login";
        }
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(_emailRegisterInputField.text, _passwordRegisterInputField.text,_usernameRegisterInputField.text));
    }

    IEnumerator Register(string inEmail, string inPassword,string inUsername)
    {
        if(inUsername == "")
        {
            _warningRegisterText.text = "Missing Username";
        }
        else if(_passwordRegisterInputField.text !=_passwordRegisterVerifyInputField.text)
        {
            _warningRegisterText.text = "Password does not match";
        }
        else
        {
            var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(inEmail,inPassword);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if(RegisterTask.Exception !=null)
            {
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string message = "Register Failed";
                switch(authError)
                {
                    case AuthError.MissingEmail:
                        message = "Email Missing";
                        break;

                    case AuthError.MissingPassword:
                        message = "Password missing";
                        break;

                    case AuthError.WeakPassword:
                        message = "Weak password";
                        break;

                    case AuthError.EmailAlreadyInUse:
                        message = "Email already in use";
                        break;
                }
                _warningRegisterText.text = message;
            }
            else
            {
                _user = RegisterTask.Result;

                if(_user !=null)
                {
                    UserProfile userProfile = new UserProfile {DisplayName =inUsername};

                    var ProfileTask = _user.UpdateUserProfileAsync(userProfile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        FirebaseException firebaseException = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError authError = (AuthError)firebaseException.ErrorCode;
                        _warningRegisterText.text = "username set failed";
                    }
                    else
                    {
                        _confirmRegisterText.text = "Register Successful";
                        _warningRegisterText.text = "";
                    }
                }
            }
        }
    }
}