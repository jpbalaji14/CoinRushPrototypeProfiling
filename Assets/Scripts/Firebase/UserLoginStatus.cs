using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class UserLoginStatus : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser User;
    DatabaseReference DBreference;
    void Start()
    {
        InitializeFirebase();
    }


    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            bool signedIn = auth.CurrentUser != null;
            if (!signedIn)
            {
                Debug.Log("Signed out " + User.UserId );
                //loginPanel.SetActive(true);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
                //loginPanel.SetActive(false);
                //userNameShowText.text = User.DisplayName;

                //emailAddress = user.Email ?? "";
                //photoUrl = user.PhotoUrl ?? "";
            }
        }
        else
        {
            Debug.Log("Not Signed In bro");
            // haven't login yet
            // your code
            //loginPanel.SetActive(true);
        }
    }
}
