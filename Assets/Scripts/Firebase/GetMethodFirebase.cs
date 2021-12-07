using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetMethodFirebase : MonoBehaviour
{
    public InputField _displayText;   
    private string mURL = "https://fir-official-1cdea-default-rtdb.firebaseio.com/";
    
    public void OnClickGet()
    {
        StartCoroutine(GetDataFromServer(mURL));
    }
    IEnumerator GetDataFromServer(string inUrl)
    {
        using (UnityWebRequest Getrequest = UnityWebRequest.Get(inUrl+".json"))
        {
            yield return Getrequest.SendWebRequest();

            if (Getrequest.isNetworkError || Getrequest.isHttpError)
            {
                Debug.Log(Getrequest.error);
            }
            Debug.Log(Getrequest.downloadHandler.text);
            _displayText.text = Getrequest.downloadHandler.text;        
        }
    }
}


