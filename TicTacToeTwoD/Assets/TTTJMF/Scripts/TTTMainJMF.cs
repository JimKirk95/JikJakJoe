using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Print()
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
                    Matriz[i][j].image.sprite = XSprites[5];
                }
                else
                {
                    Matriz[i][j].image.sprite = OSprites[3];
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

    void PrintResult()
    {
        if (win)
        {
            if (player == 1)
            {
                PlayerTXT.text = "Player 1 won the game";
            }
            else
            {
                PlayerTXT.text = "Player 2 won the game";
            }
        }
        else
        {
            PlayerTXT.text = "Nobody won";
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
}
