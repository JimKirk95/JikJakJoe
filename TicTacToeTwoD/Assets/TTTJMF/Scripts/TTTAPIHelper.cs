using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerTotStats
{
    public string Name;
    public string StOne;
    public string StTwo;
}

[Serializable]
public class UpdPlayer
{

    public string CallerID = "JMF";
    public string CallerPW = "JMF";
    public string Nickname = "Jack";
    public string Password = "pass";
    public int[] NewStats = { 4, 2, 1, 2, 1 };
}



public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class TTTAPIHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(GetRequest());
        //StartCoroutine(Get());
       // StartCoroutine(Post());
        //StartCoroutine(Put());

        StartCoroutine(Delete());

        /*
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://jmfwebapi2021.azurewebsites.net/API/TopWinners"));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = "{\"Items\":" + reader.ReadToEnd() + "}";
        PlayerTotStats[] info = JsonHelper.FromJson<PlayerTotStats>(jsonResponse);

        

        Debug.Log($"oi {info[0].Name}, {info[0].StOne}, {info[0].StTwo}");
        Debug.Log($"oi {info[1].Name}, {info[1].StOne}, {info[1].StTwo}");
        Debug.Log($"oi {info[2].Name}, {info[2].StOne}, {info[2].StTwo}");
        */
    }




    //UnityWebRequest delete = UnityWebRequest.Delete("http://www.myserver.com/foo.txt");


    IEnumerator Delete()
    {

        WWWForm form = new WWWForm();
        form.AddField("CallerID", "JMF");
        form.AddField("CallerPW", "JMF");
        form.AddField("Nickname", "Github1");
        form.AddField("Password", "JimKirk95");
        form.AddField("DeleteID", "DJMF");
        form.AddField("DeletePW", "DJMF");
        using (UnityWebRequest www = UnityWebRequest.Post("https://jmfwebupdt2021.azurewebsites.net/API/UpdScores/42", form))
        {
            www.method = "DELETE";
            //www.SetRequestHeader("X-HTTP-Method-Override", "PATCH");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log("Dan: " + www.error);
                Debug.Log("Da2: " + www.downloadHandler.text);

            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(":\nReceived: " + www.downloadHandler.text);

            }
        }
    }




    IEnumerator Put()
    {

        UpdPlayer player = new UpdPlayer();
        string json =  JsonUtility.ToJson(player);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        Debug.Log("OI: " + json);

        using (UnityWebRequest www = UnityWebRequest.Put("https://jmfwebupdt2021.azurewebsites.net/API/UpdScores/42", json))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Dan: " + www.error);
                Debug.Log("Da2: " + www.downloadHandler.text);

            }
            else
            {
                Debug.Log("Update complete!");
                Debug.Log(":\nReceived: " + www.downloadHandler.text);

            }
        }
    }



    IEnumerator Post()
    {
      
        WWWForm form = new WWWForm();
        form.AddField("CallerID", "JMF");
        form.AddField("CallerPW", "JMF");
        form.AddField("Nickname", "Github1");
        form.AddField("Password", "JimKirk95"); ;
        using (UnityWebRequest www = UnityWebRequest.Post("https://jmfwebupdt2021.azurewebsites.net/API/NewPlayer", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(":\nReceived: " + www.downloadHandler.text);

            }
        }
    }


    IEnumerator GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://jmfwebapi2021.azurewebsites.net/API/TopWinners"))
        {
            yield return webRequest.SendWebRequest(); 
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                    string jsonResponse = "{\"Items\":" + webRequest.downloadHandler.text + "}";
                    PlayerTotStats[] info = JsonHelper.FromJson<PlayerTotStats>(jsonResponse);
                    Debug.Log($"oi {info[0].Name}, {info[0].StOne}, {info[0].StTwo}");
                    Debug.Log($"oi {info[1].Name}, {info[1].StOne}, {info[1].StTwo}");
                    Debug.Log($"oi {info[2].Name}, {info[2].StOne}, {info[2].StTwo}");
                    break;
            }
        }
    }


    IEnumerator Get()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://jmfwebapi2021.azurewebsites.net/API/TopWinners"))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!: " + www.downloadHandler.text);
                string jsonResponse = "{\"Items\":" + www.downloadHandler.text + "}";
                PlayerTotStats[] info = JsonHelper.FromJson<PlayerTotStats>(jsonResponse);
                Debug.Log($"oi {info[0].Name}, {info[0].StOne}, {info[0].StTwo}");
                Debug.Log($"oi {info[1].Name}, {info[1].StOne}, {info[1].StTwo}");
                Debug.Log($"oi {info[2].Name}, {info[2].StOne}, {info[2].StTwo}");
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
