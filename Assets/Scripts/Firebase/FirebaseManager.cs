using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    Facebookauth fbauth;
    DatabaseReference reference;
    Player playerDetails = new Player();
    public InputField _userName;
    public InputField _getPlayerDetails;
    public Text  _playerIDDataText, _coinDataText, _energyDataText;
    private string mPlayerIDData, _mCoinData, mEnergyData;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;        
        playerDetails._playerCurrentLevel = Random.Range(1, 30);
        playerDetails._coins = Random.Range(0, 10000);
        playerDetails._energy = Random.Range(0, 50);
    }
    public void Writedata()
    {
        playerDetails._playerName = _userName.text;
        string json = JsonUtility.ToJson(playerDetails);

        reference.Child("Players").Child(playerDetails._playerName).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Write Successful");
            }
        });
    }
    public void ReadData()
    {
        reference.Child("Players").Child(_getPlayerDetails.text).GetValueAsync().ContinueWith(task => 
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                mPlayerIDData = snapshot.Child("_playerID").Value.ToString();
                _mCoinData = snapshot.Child("_coins").Value.ToString();
                mEnergyData = snapshot.Child("_energy").Value.ToString();
            }
        });
    }
   void Update()
   {
      _playerIDDataText.text = mPlayerIDData;
      _coinDataText.text = _mCoinData;
      _energyDataText.text = mEnergyData;
   }
}
