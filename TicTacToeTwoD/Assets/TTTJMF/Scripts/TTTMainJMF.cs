using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TTTMainJMF : MonoBehaviour
{
    [SerializeField] Text PlayerTXT;
    [SerializeField]  Sprite Branco;
    [SerializeField] List<Sprite> XSprites;
    [SerializeField] List<Sprite> OSprites;
    [SerializeField] Button[] LinhaUm;
    [SerializeField] Button[] LinhaDois;
    [SerializeField] Button[] LinhaTres;
    Button[][] Matriz = new Button[3][];

    UpdPlayer playerStats = new UpdPlayer();

    int[,] array = {{0,0,0},
                    {0,0,0},
                    {0,0,0}};
    int row = 0;
    int col = 0;
    int player = 1;
    static bool win = false;
    int counter = 0;

    public void GetInput(Button button = null)
    {
        if ((!win) && (counter < 9))
        {
            string[] coord = button.name.Substring(7, 3).Split(',');
            row = int.Parse(coord[0]) - 1;
            col = int.Parse(coord[1]) - 1;
            if (array[row, col] == 0)
            {
                SetArray();
                win = CheckWinner();
                if (!win)
                {
                    ChangePlayer();
                    Print();
                    counter++;
                    if (counter == 9)
                    {
                        PrintResult();
                    }
                }
                else
                {
                    Print();
                    PrintResult();
                }
            }
        } 
    }

    void ChangePlayer()
    {
        if (player == 1)
        {
            player = 2;
        }
        else
        {
            player = 1;
        }
    }

    void SetArray()
    {
        if (player == 1)
        {
            array[row, col] = 1;
        }
        else
        {
            array[row, col] = 10;
        }
    }

    private void Print() //Atualiza imagens dos botões (podia atualizar só o recém jogado também)
    {
        PlayerTXT.text = $"Player {player}";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (array[i, j] == 0)
                {
                    Matriz[i][j].image.sprite = Branco;
                }
                else if (array[i, j] == 1)
                {
                    Matriz[i][j].image.sprite = XSprites[TTTPlayerPrefs.TTTXe - 1];
                }
                else
                {
                    Matriz[i][j].image.sprite = OSprites[TTTPlayerPrefs.TTTOe - 1];
                }
            }
        }

    }

    bool CheckWinner()
    {
        int sum = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                sum += array[i, j];
            }
            if (sum == 3 || sum == 30)
            {
                return true;
            }
            sum = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            sum = 0;
            for (int j = 0; j < 3; j++)
            {
                sum += array[j, i];
            }
            if (sum == 3 || sum == 30)
            {
                return true;
            }

        }
        sum = array[1, 1] + array[2, 2] + array[0, 0];
        if (sum == 3 || sum == 30)
        {
            return true;
        }
        sum = array[0, 2] + array[1, 1] + array[2, 0];
        if (sum == 3 || sum == 30)
        {
            return true;
        }
        return false;
    }

    void PrintResult() //Atualiza resultados e dá moedas
    {
        if (win)
        {
            if (player == 1)
            {
                PlayerTXT.text = "Player 1 won the game";
                if (TTTPlayerPrefs.IsRegistrado())
                {
                    TTTPlayerPrefs.GanhouMoeda(3);//Vitória 3 moedas
                    CallApiPut(3);
                }
            }
            else
            {
                PlayerTXT.text = "Player 2 won the game"; //Derrota, sem moedas
                if (TTTPlayerPrefs.IsRegistrado())
                {
                    CallApiPut(0);
                }
            }
        }
        else
        {
            PlayerTXT.text = "Nobody won";
            if (TTTPlayerPrefs.IsRegistrado())
            {
                TTTPlayerPrefs.GanhouMoeda(1); //Empate 1 moeda
                CallApiPut(1);
            }
        }
    }

    void Start()
    {
        Matriz[0] = LinhaUm;
        Matriz[1] = LinhaDois;
        Matriz[2] = LinhaTres;
        for (int i = 0; i < Matriz.Length; i++)
        {
            Button[] Linha = Matriz[i];
            for (int j = 0; j < Linha.Length; j++)
            {
                Button Botao = Linha[j];
                Botao.onClick.AddListener(() => {GetInput(Botao); });
            }
        }
        Print();
    }

    public void ButtonReset()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[i, j] = 0;
            }
        }
        row = 0;
        col = 0;
        player = 1;
        win = false;
        counter = 0;
        Print();
    }

    public Sprite getXImage(int i) //Return X Strite for Config Canvas
    {
        if (i < XSprites.Count)
        {
            return XSprites[i];
        }
        return XSprites[0];
    }
    public Sprite getOImage(int i) //Return O Sprite for Config Canvas
    {
        if (i < OSprites.Count)
        {
            return OSprites[i];
        }
        return OSprites[0];
    }




    //DEVIA COLOCAR EM UMA OUTRA CLASSE... mas isso consome tempo... então vai aqui mesmo :-)
    void CallApiPut(int points) //Atualização de estatísticas do jogador
    {
        //0 = derrota;
        //1 = empate;
        //3 = vitória;
        playerStats.Nickname = TTTPlayerPrefs.TTTNickname;
        playerStats.Password = TTTPlayerPrefs.TTTPassword;
        playerStats.NewStats[0] = 1;
        playerStats.NewStats[3] = 1;
        switch (points)
        {
            case 0:
                break;
            case 1:
                playerStats.NewStats[2] = 1;
                playerStats.NewStats[5] = 1;
                break;
            case 3:
                playerStats.NewStats[1] = 1;
                playerStats.NewStats[4] = 1;
                break;
        }
        string json = JsonUtility.ToJson(playerStats);
        StartCoroutine(ApiPut("https://jmfwebupdt2021.azurewebsites.net/API/UpdScores/42", json, CallbackActionPut));
    }


    public IEnumerator ApiPut(string Uri, string jsonData, System.Action<bool, string> callback) //Put = Update
    {
        using (UnityWebRequest www = UnityWebRequest.Put(Uri, jsonData))
        {
            www.SetRequestHeader("Content-Type", "application/json");
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


    void CallbackActionPut(bool isSuccess, string message) //Processa depois do DELETe
    {
        if (isSuccess)
        {
            string[] newStats = message.Split('[', ']')[1].Split(',');
//            Debug.Log($"J: {newStats[0]} A: {newStats[1]} B: {newStats[2]} c: {newStats[3]} d: {newStats[4]} e: {newStats[5]} ");
            TTTPlayerPrefs.TTTTG = int.Parse(newStats[0]);
            TTTPlayerPrefs.TTTTW = int.Parse(newStats[1]);
            TTTPlayerPrefs.TTTTD = int.Parse(newStats[2]);
            TTTPlayerPrefs.TTTWG = int.Parse(newStats[3]);
            TTTPlayerPrefs.TTTWW = int.Parse(newStats[4]);
            TTTPlayerPrefs.TTTWD = int.Parse(newStats[5]);
            TTTPlayerPrefs.SaveNewStats();

        }
        else
        {
            // guardar dados para atualização futura
            // por enquanto não faz nada
            string[] splitter = { " xJMFx " };
            string[] error = message.Split(splitter, StringSplitOptions.None);
            if (error.Length > 1)
            {
//                Debug.Log($"{error[0]}\n{error[1]}!");
            }
            else
            {
//                Debug.Log($"{error[0]}!");
            }
        }
    }

}
