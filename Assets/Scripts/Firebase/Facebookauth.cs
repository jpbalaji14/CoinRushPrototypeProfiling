using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System;
using UnityEngine.UI;
public class Facebookauth : MonoBehaviour
{
    FirebaseAuth _auth;
    public Text _fbUserName;
    public Image _fbDp;
    public  RectTransform _fbDpTransform;
    public Text _loginInfo;
    public GameObject _loginButton;
    public Firebase.Auth.FirebaseUser _newUser;
   
    //Firebase Database References
    DatabaseReference reference;
    Player playerDetails = new Player();
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;     
        playerDetails._playerCurrentLevel = 50;
        playerDetails._coins = 20;
        playerDetails._energy = 50;
    }
    private void InitCallBack()
    {
        if (!FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to initialize");
        }
    }
    private void OnHideUnity(bool isgameshown)
    {
        if (!isgameshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Facebook_Login()
    {
        var permission = new List<string>() { "public_profile", "email" };
        
        FB.LogInWithReadPermissions(permission, AuthCallBack);
    }
    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        { 
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string accesstoken;
            string[] data;
            string acc;
            string[] some;
            DealWithFbMenus(FB.IsLoggedIn);

#if UNITY_EDITOR
            data = result.RawResult.Split(',');
            acc = data[3];
            some = acc.Split('"');
            accesstoken = some[3];
            Debug.Log(accesstoken);
#elif UNITY_ANDROID
            Debug.Log("this is raw access " + result.RawResult);
            data = result.RawResult.Split(',');
            acc = data[0];
            some = acc.Split('"');
            accesstoken = some[3];
#endif
            authwithfirebase(accesstoken);
        }
        else
        {
          Debug.Log("User Cancelled login");
        }
    }
    public void authwithfirebase(string accesstoken)
    {
        _auth = FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accesstoken);
        _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("signin encountered error" + task.Exception);
            }
            _newUser = task.Result;
            Debug.Log("Disp name: "+ _newUser.DisplayName);
            
            //Database Details Update
            playerDetails._playerName = _newUser.DisplayName;
            playerDetails._playerID = _newUser.UserId;
            Debug.Log(_newUser.UserId);
            SaveNewUser(_newUser.UserId);
        });
    }

    public void SaveNewUser(string userId)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        //string userNameId =userId;
        string json = JsonUtility.ToJson(playerDetails);
        reference.Child("Facebook Users").Child(currentUser.UserId).SetRawJsonValueAsync(json);
    }
    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            _loginButton.SetActive(false);
            _loginInfo.text = "Logged In";            
            FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
            FB.Mobile.RefreshCurrentAccessToken();
        }
    }
    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string fbname = result.ResultDictionary["name"].ToString();
            _fbUserName.text = fbname;
        }
    }
    private void GetPicture(IGraphResult result)
    {
        if (result.Error == null)
        {
            Image img = _fbDp.GetComponent<Image>();
            img.sprite = Sprite.Create(result.Texture, new Rect(0, 0,130,130) /*_fbDpTransform.rect.width, _fbDpTransform.rect.height)*/, new Vector2());
        }
    }
}