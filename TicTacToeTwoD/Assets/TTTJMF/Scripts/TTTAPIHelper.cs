using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// MUITO BEM, AQUI ERA PARA FAZER AS COISAS MAIS SEPARADAS:
///     EM CAMADAS
///     EM DIFERENTES ARQUIVOS
///     MELHORAR A REUTILIZAÇÃO DE CÓDIGO
///     E COM MAIS VEFIFICAÇÕES
/// Mas, não vai dar tempo não :-)
/// Preferi me concentrar nas features aparentes e deixar os bastidores bagunçados mesmo
/// Um, dia, com tempo, quem sabe eu arrume talvez eu use o tic-tac-toe como exemplo de um tutorial/Template
/// </summary>

[Serializable]
public class PlayerTotStats
{
    public string Name;
    public string StOne;
    public string StTwo;
}
[Serializable]
public class PlayerWeekStats
{
    public string Name;
    public string WSO;
    public string WSTw;
    public string WSTh;
    public string WSF;
}
[Serializable]
public class UpdPlayer
{
    public string CallerID = "JMF"; //Hardcoded, mas deveria mudar poderia atribuir um id automático
    public string CallerPW = "JMF"; //Ao cadastra o usuário, ou criar um com base no dispositivo
    public string Nickname = "";
    public string Password = "";
    public int[] NewStats = { 0, 0, 0, 0, 0, 0};
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
{   //AQUI VALERIA A PENA USAR UM "FOLDABLE" INSPECTOR... quando tiver mais tempo
    [Header("Top Scores")]
    [SerializeField] Text[] Players;
    [SerializeField] Text[] Scores;
    [SerializeField] Text[] PlayersTG;
    [SerializeField] Text[] ScoresTG;
    [SerializeField] Text[] PlayersWW;
    [SerializeField] Text[] ScoresWW;
    [SerializeField] Text[] PlayersWG;
    [SerializeField] Text[] ScoresWG;
    [Space(10)]
    [Header("Create User")]
    [SerializeField] Canvas TelaCreateUser;
    [SerializeField] Text CreateUser;
    [SerializeField] InputField newUserNick;
    [SerializeField] InputField newPassword;
    [SerializeField] Button Create;
    [Space(10)]
    [Header("Delete User")]
    [SerializeField] Text DeleteUser;
    [SerializeField] Toggle ConfirmaDelete;
    [SerializeField] Text DeleteUserMessage;
    [SerializeField] Canvas CanvasDelete;
    [SerializeField] InputField UserToDelete;
    [Space(10)]
    [Header("Configurações")]
    [SerializeField] Canvas CanvasConfig;
    [SerializeField] Button callConfig;
    void Start()
    {
        Create.onClick.AddListener(ActionCreatePlayer);
        if (TTTPlayerPrefs.IsRegistrado())
        {
            TTTPlayerPrefs.RecuperaTTTPrefs();
            TelaCreateUser.gameObject.SetActive(false);
            callConfig.gameObject.SetActive(true);
            //Carregar Customização
        }
        else
        {
            callConfig.gameObject.SetActive(false);
            TelaCreateUser.gameObject.SetActive(true);
        }
        //* //DESCOMENTAR (OU COMENTAR) AQUI PARA DESATIVAR A CHAMADA À API dos TOP3
        //Deveria fazer chamadas periódicas para atualizar, mas fica para a próxima.
        //Por enquanto vai atualizar só uma vez na inicialização da Scene
        CallApiGet(); //Atuliza TOP SCORES
        //Também deveria guardar uma cópia local dos Top3 para o caso de falta de conexão... mas...        
        //*/
    }

    public void AcaoAbrirExcluir()//Reseta tela de excluir jogador
    {   //No caso do usurário ter modificado e desistido, precisa limpar tudo
        ConfirmaDelete.isOn = false;
        DeleteUser.gameObject.SetActive(false);
        DeleteUserMessage.text = "Confirme a exclução cllicando\nna caixa e no botão Excluir.";
        UserToDelete.text = TTTPlayerPrefs.TTTNickname;
    }

    public void CallApiGet()//Chama APIs dos Top3
    {
        int length = (int)Math.Min(Players.Length, Scores.Length);
        for (int i = 0; i < length; i++)
        {
            Players[i].text = "Loading winners ...";
            Scores[i].text = "...";
        }
        length = (int)Math.Min(PlayersTG.Length, ScoresTG.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersTG[i].text = "Loading players ...";
            ScoresTG[i].text = "...";
        }
        length = (int)Math.Min(PlayersWG.Length, ScoresWG.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersWG[i].text = "Loading wplayers ...";
            ScoresWG[i].text = ".";
        }
        length = (int)Math.Min(PlayersWW.Length, ScoresWW.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersWW[i].text = "Loading wwinners ...";
            ScoresWW[i].text = ".";
        }
        StartCoroutine(ApiGet("https://jmfwebapi2021.azurewebsites.net/API/TopWinners", CallbackActionGet));
        StartCoroutine(ApiGet("https://jmfwebapi2021.azurewebsites.net/API/TopPlayers", CallbackActionGetTG));
        StartCoroutine(ApiGet("https://jmfwebapi2021.azurewebsites.net/API/WeekPlayers", CallbackActionGetWG));
        StartCoroutine(ApiGet("https://jmfwebapi2021.azurewebsites.net/API/WeekWinners", CallbackActionGetWW));
    }
    public IEnumerator ApiGet(string Uri, System.Action<bool, string> callback) //GET = Read / SELECT
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Uri))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                callback(false, www.error);
            }
            else
            {
                string jsonResponse = "{\"Items\":" + www.downloadHandler.text + "}";
                callback(true, jsonResponse);
            }
        }
    }
    void CallbackActionGet(bool isSuccess, string jsonData) //Atualiza top Winners
    {
        int length = (int)Math.Min(Players.Length, Scores.Length);
        for (int i = 0; i < length; i++)
        {
            Players[i].text = "No winner...";
            Scores[i].text = "..";
        }
        if (isSuccess)
        {
            PlayerTotStats[] info = JsonHelper.FromJson<PlayerTotStats>(jsonData);
            length = (int)Math.Min(info.Length, length);
            for (int i = 0; i < length; i++)
            {
                Players[i].text = info[i].Name;
                Scores[i].text = info[i].StOne;
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                Players[i].text = "Sem conexão...";
                Scores[i].text = "..";
            }
        }
    }
    void CallbackActionGetTG(bool isSuccess, string jsonData) //Atualiza top Players
    {
        int length = (int)Math.Min(PlayersTG.Length, ScoresTG.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersTG[i].text = "No player...";
            ScoresTG[i].text = "..";
        }
        if (isSuccess)
        {
            PlayerTotStats[] info = JsonHelper.FromJson<PlayerTotStats>(jsonData);
            length = (int)Math.Min(info.Length, length);
            for (int i = 0; i < length; i++)
            {
                PlayersTG[i].text = info[i].Name;
                ScoresTG[i].text = info[i].StOne;
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                PlayersTG[i].text = "Sem conexão...";
                ScoresTG[i].text = "..";
            }
        }
    }
    void CallbackActionGetWG(bool isSuccess, string jsonData)  //Atualiza week Players
    {
        int length = (int)Math.Min(PlayersWG.Length, ScoresWG.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersWG[i].text = "No player...";
            ScoresWG[i].text = "..";
        }
        if (isSuccess)
        {

            PlayerWeekStats[] info = JsonHelper.FromJson<PlayerWeekStats>(jsonData);
            length = (int)Math.Min(info.Length, length);
            for (int i = 0; i < length; i++)
            {
                PlayersWG[i].text = info[i].Name;
                ScoresWG[i].text = info[i].WSO;
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                PlayersWG[i].text = "Sem conexão...";
                ScoresWG[i].text = "..";
            }
        }
    }
    void CallbackActionGetWW(bool isSuccess, string jsonData) //Atualiza week Winners 
    {
        int length = (int)Math.Min(PlayersWW.Length, ScoresWW.Length);
        for (int i = 0; i < length; i++)
        {
            PlayersWW[i].text = "No winner...";
            ScoresWW[i].text = "..";
        }
        if (isSuccess)
        {

            PlayerWeekStats[] info = JsonHelper.FromJson<PlayerWeekStats>(jsonData);
            length = (int)Math.Min(info.Length, length);
            for (int i = 0; i < length; i++)
            {
                PlayersWW[i].text = info[i].Name;
                ScoresWW[i].text = info[i].WSO;
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                PlayersWW[i].text = "Sem conexão...";
                ScoresWW[i].text = "..";
            }
        }
    }

    public void ActionRestablishMessage(string value) //Reseta mensagem de criação de jogador 
    {
        newUserNick.onValueChanged.RemoveListener(ActionRestablishMessage);
        newPassword.onValueChanged.RemoveListener(ActionRestablishMessage);
        CreateUser.text = $"Entre um Nick e uma Senha\nE clique em Criar";
        Create.gameObject.SetActive(true); //reabilita botão de criar jogador
    }

    public void ActionCreatePlayer() //Botão de criar jogador
    {   //Faz verificações e chama função
        Create.gameObject.SetActive(false);//Desabilita próprio botão para não ser clicado 2 vezes
        if ((newUserNick.text.Length > 0) && (newPassword.text.Length > 0))
        {
            newUserNick.enabled = false; //trava input field para usuário não modificar
            newPassword.enabled = false; //trava input field para usuário não modificar
            CreateUser.text = $"Registrando usuário\nPor favor aguarde...";
            CallApiPost();//Chama função de criação
        }
        else
        {
            if (newUserNick.text.Length == 0)
            {
                if (newPassword.text.Length == 0)
                {
                    CreateUser.text = $"Nick e senha não podem ser vazios\n Entre novo nick e senha";
                    newUserNick.onValueChanged.AddListener(ActionRestablishMessage);
                    newPassword.onValueChanged.AddListener(ActionRestablishMessage);
                }
                else //só nick vazio
                {
                    CreateUser.text = $"O nick não pode ser vazio\nEntre nick com 1 a 20 caracteres";
                    newUserNick.onValueChanged.AddListener(ActionRestablishMessage);
                }
            }
            else // Só password vazia
            {
                CreateUser.text = $"A senha não pode ser vazia\nEntre senha com 1 a 20 caracteres";
                newPassword.onValueChanged.AddListener(ActionRestablishMessage);
            }
        }
    }
    void CallApiPost() //Prepara dados e chama corrotina da API
    {
        WWWForm json = new WWWForm();
        json.AddField("CallerID", "JMF"); //Deveria buscar credenciais online, de outra API, ou de um GSheet
        json.AddField("CallerPW", "JMF"); //Mas fica para a próxima também... aqui vai hardcoded
        json.AddField("Nickname", newUserNick.text);
        json.AddField("Password", newPassword.text);
        StartCoroutine(ApiPost("https://jmfwebupdt2021.azurewebsites.net/API/NewPlayer", json, CallbackActionPost));
    }
    public IEnumerator ApiPost(string Uri, WWWForm jsonData, System.Action<bool, string> callback) //POST = Create / INSERT
    {
        using (UnityWebRequest www = UnityWebRequest.Post(Uri, jsonData))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                callback(false, $"{www.error} xJMFx {www.downloadHandler.text}");
            }
            else
            {
                callback(true, www.downloadHandler.text);
            }
        }
    }
    void CallbackActionPost(bool isSuccess, string message) //Processa depois da chamada da API de criação
    {
        if (isSuccess)
        {
            TTTPlayerPrefs.Registra(newUserNick.text, newPassword.text);
            newUserNick.enabled = true;
            newPassword.enabled = true;
            Create.gameObject.SetActive(true); //reativa botão de criação pensando em nova criação
            TelaCreateUser.gameObject.SetActive(false); //Fecha tela de criação
            callConfig.gameObject.SetActive(true); //Habilita botão de configurações  
        }
        else
        {
            newUserNick.enabled = true;
            newPassword.enabled = true;
            string[] splitter = { " xJMFx " };
            string[] error = message.Split(splitter, StringSplitOptions.None);
            if (error.Length > 1)
            {
                if (error[1].Contains("Nickname já usado"))
                {//deveria fazer mais verificações e retornar mensagens diferentes...
                 //Mas, pra demo/teste essa já está bom.
                    CreateUser.text = $"Usuário já usado\n por favor escolha outro";
                    newUserNick.onValueChanged.AddListener(ActionRestablishMessage);
                }
                else
                {
                    CreateUser.text = $"Erro inesperado, tente outro nick/senha\n{error[1]}!";
                    newUserNick.onValueChanged.AddListener(ActionRestablishMessage);
                    newPassword.onValueChanged.AddListener(ActionRestablishMessage);
                }
            }
            else
            {
                CreateUser.text = $"Erro desconhecido, tente outro nick/senha\n{error[0]}";
                newUserNick.onValueChanged.AddListener(ActionRestablishMessage);
                newPassword.onValueChanged.AddListener(ActionRestablishMessage);
            }
        }
    }

    public void ActionDeletePlayer() //Respota ao botão de deletar usuário
    {
        if (ConfirmaDelete.isOn) //Verifica confirmação
        {
            DeleteUserMessage.text = "Excluindo usuário\naguarde...";
            CallApiDelete();
        }
        else
        {
            DeleteUser.gameObject.SetActive(true); //Mostra aviso adicional
        }
    }
    void CallApiDelete() //Prepara chamada da API
    {
        WWWForm JSONDelete = new WWWForm();
        JSONDelete.AddField("CallerID", "JMF"); //Deveria buscar credenciais online, de outra API, ou de um GSheet
        JSONDelete.AddField("CallerPW", "JMF"); //Mas fica para a próxima também... aqui vai hardcoded
        JSONDelete.AddField("Nickname", TTTPlayerPrefs.TTTNickname);
        JSONDelete.AddField("Password", TTTPlayerPrefs.TTTPassword);
        JSONDelete.AddField("DeleteID", "DJMF");
        JSONDelete.AddField("DeletePW", "DJMF");        
        StartCoroutine(ApiDelete("https://jmfwebupdt2021.azurewebsites.net/API/UpdScores/42", JSONDelete, CallbackActionDelete));
    }
    public IEnumerator ApiDelete(string Uri, WWWForm jsonData, System.Action<bool, string> callback) //DELETE = Delete
    {
        using (UnityWebRequest www = UnityWebRequest.Post(Uri, jsonData))
        {   //Precisa criar como POST, com BODY e mudar para DELETE
            www.method = "DELETE"; //Truquezinho por limitação do UnityWebRequest
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                callback(false, $"{www.error} xJMFx {www.downloadHandler.text}");
            }
            else
            {
                callback(true, www.downloadHandler.text);
            }
        }
    }
    void CallbackActionDelete(bool isSuccess, string message) //Processa depois do DELETe
    {
        if (isSuccess)
        {
            ConfirmaDelete.isOn = false; //Limpa Canvas para outro delete posterior
            DeleteUser.gameObject.SetActive(false); //Texto adicional de aviso
            DeleteUserMessage.text = "Confirme a exclução cllicando\nna caixa e no botão Excluir.";
            CanvasDelete.gameObject.SetActive(false); //Fecha tela de exclusão
            TTTPlayerPrefs.ApagaRegistro(); //Apaga registro do usuário
            CanvasConfig.gameObject.SetActive(false); //Fecha canvas de config
            newUserNick.text = "";
            newPassword.text = "";
            callConfig.gameObject.SetActive(false);
            TelaCreateUser.gameObject.SetActive(true); //Abre tela para criar novo usuário
        }
        else
        {
            string[] splitter = { " xJMFx " };
            string[] error = message.Split(splitter, StringSplitOptions.None);
            if (error.Length > 1)
            {
                DeleteUserMessage.text = $"{error[0]}\n{error[1]}!";
            }
            else
            {
                DeleteUserMessage.text = $"Erro desconhecido\n{error[0]}";
            }
        }
    }







    public IEnumerator APIPut() //PUT = Update / UPDATE
    {
        UpdPlayer player = new UpdPlayer();
        //Informações sobre o Player ??????????????
        string json = JsonUtility.ToJson(player);
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












}
