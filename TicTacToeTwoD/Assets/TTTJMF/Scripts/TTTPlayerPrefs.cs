using UnityEngine;

public class TTTPlayerPrefs
{
    #region properties //Declarações de variáveis
    private const string keyNick = "Nickname"; //Chave para armazenar o Nickname
    private const string keySenha = "Password"; //Chave para armazenar a senha
    private const string keyRegistrado = "Registro"; //Chave para armazenar se o usuário já está registrado
    private const string keyXLiberados = "XUps"; //Chave para X liberados
    private const string keyOLiberados = "OUps"; //Chave para O liberados
    private const string keyMoedas = "Moedas"; //Chave para dinheiro
    private const string keyXEscolhido = "XUse"; //Chave para X liberados
    private const string keyOEscolhido = "OUse"; //Chave para O liberados
    private const string keyTG = "TotalGames"; //Chave para total de jogos
    private const string keyTW = "TotalWins"; //Chave para total de vitórias
    private const string keyTD = "TotalDraws"; //Chave para total de empates
    private const string keyWG = "WeekGames"; //Chave para jogos na semana
    private const string keyWW = "WeekWins"; //Chave para vitórias na semana
    private const string keyWD = "WeekDraws"; //Chave para empates na semana
    //OBS: podia salvar os playes Status em arquivo... já está até serializado... mas.... 
    public static string TTTNickname { get; private set; } = ""; //Player Nickname
    public static string TTTPassword { get; private set; } = ""; //Player Password
    public static bool TTTRegistrado { get; private set; } = false; //Se o usurário já foi registrado
    public static int TTTOs { get; set; } = 1; //Os liberados
    public static int TTTXs { get; set; } = 1; //Xs liberados
    public static int TTTMoedas { get; set; } = 0; // Dinheiro :-)
    public static int TTTOe { get; set; } = 1; //Os liberados
    public static int TTTXe { get; set; } = 1; //Xs liberados
    public static int TTTTG { get; set; } = 0; //Total de jogos
    public static int TTTTW { get; set; } = 0; //Total de vitórias
    public static int TTTTD { get; set; } = 0; //Total de empates
    public static int TTTWG { get; set; } = 0; //Jogos na semana
    public static int TTTWW { get; set; } = 0; //vitórias na semana
    public static int TTTWD { get; set; } = 0; //Empates na semana
    #endregion

    public static bool IsRegistrado() //Verifica se tem jogador registrado
    {
        TTTRegistrado = PlayerPrefs.GetInt(keyRegistrado, 0) != 0; //Converte de int para bool
        return TTTRegistrado;
    }

    public static int GanhouMoeda(int ganho) //Aumenta Moedas e salva
    {
        if (ganho > 0)
        { 
            TTTMoedas += ganho;
            PlayerPrefs.SetInt(keyMoedas, TTTMoedas);
        }
        return TTTMoedas;
    }
    public static int CompraX() //Compra novo X (mas não salva a compra)
    {
        if ((TTTMoedas > 0) && (TTTXs < 10) )
        {
            TTTXs++;
            TTTMoedas--;
        }
        return TTTMoedas;
    }
    public static int CompraO() //Compra novo O (mas não salva a compra)
    {
        if ((TTTMoedas > 0) && (TTTOs < 10))
        {
            TTTOs++;
            TTTMoedas--;
        }
        return TTTMoedas;
    }

    public static void ApagaRegistro() //"Zera" configurações e chama Recupera
    {
        PlayerPrefs.DeleteKey(keyNick);
        PlayerPrefs.DeleteKey(keySenha);
        PlayerPrefs.DeleteKey(keyRegistrado);
        PlayerPrefs.DeleteKey(keyMoedas);
        PlayerPrefs.DeleteKey(keyXLiberados);
        PlayerPrefs.DeleteKey(keyOLiberados);
        PlayerPrefs.DeleteKey(keyXEscolhido);
        PlayerPrefs.DeleteKey(keyOEscolhido);
        PlayerPrefs.DeleteKey(keyTG);
        PlayerPrefs.DeleteKey(keyTW);
        PlayerPrefs.DeleteKey(keyTD);
        PlayerPrefs.DeleteKey(keyWG);
        PlayerPrefs.DeleteKey(keyWW);
        PlayerPrefs.DeleteKey(keyWD);
        RecuperaTTTPrefs();
    }
    public static void RecuperaTTTPrefs() //Recupera todas as preferências
    {
        TTTNickname = PlayerPrefs.GetString(keyNick, "");
        TTTPassword = PlayerPrefs.GetString(keySenha, "");
        TTTRegistrado = PlayerPrefs.GetInt(keyRegistrado, 0) != 0; //Converte de int para bool
        TTTMoedas = PlayerPrefs.GetInt(keyMoedas, 0);
        TTTOs = PlayerPrefs.GetInt(keyOLiberados, 1);
        TTTXs = PlayerPrefs.GetInt(keyXLiberados, 1);
        TTTOe = PlayerPrefs.GetInt(keyOEscolhido, 1);
        TTTXe = PlayerPrefs.GetInt(keyXEscolhido, 1);
        TTTTG = PlayerPrefs.GetInt(keyTG, 0);
        TTTTW = PlayerPrefs.GetInt(keyTW, 0);
        TTTTD = PlayerPrefs.GetInt(keyTD, 0);
        TTTWG = PlayerPrefs.GetInt(keyWG, 0);
        TTTWW = PlayerPrefs.GetInt(keyWW, 0);
        TTTWD = PlayerPrefs.GetInt(keyWD, 0);
    }


    public static void SaveConfig() //Salva configuração atual, incluindo moedas gastas em compras
    {
        PlayerPrefs.SetString(keyNick, TTTNickname);
        PlayerPrefs.SetString(keySenha, TTTPassword);
        PlayerPrefs.SetInt(keyMoedas, TTTMoedas);
        PlayerPrefs.SetInt(keyXLiberados, TTTXs);
        PlayerPrefs.SetInt(keyOLiberados, TTTOs);
        PlayerPrefs.SetInt(keyXEscolhido, TTTXe);
        PlayerPrefs.SetInt(keyOEscolhido, TTTOe);
        PlayerPrefs.SetInt(keyTG, TTTTG);
        PlayerPrefs.SetInt(keyTW, TTTTW);
        PlayerPrefs.SetInt(keyTD, TTTTD);
        PlayerPrefs.SetInt(keyWG, TTTWG);
        PlayerPrefs.SetInt(keyWW, TTTWW);
        PlayerPrefs.SetInt(keyWD, TTTWD);
    }

    public static void Registra(string novoNick, string novaSenha) //Cria novo registro
    {
        if ((novoNick.Length > 0)&&(novaSenha.Length>0))
        {
            ApagaRegistro(); //Redundande, por garantia
            TTTNickname = novoNick;
            TTTPassword = novaSenha;
            TTTRegistrado = true;
            PlayerPrefs.SetInt(keyRegistrado, 1); // 0 = false; 1 = true;
            SaveConfig();
        }
    }

    public static void SaveNewStats()
    {
        PlayerPrefs.SetInt(keyTG, TTTTG);
        PlayerPrefs.SetInt(keyTW, TTTTW);
        PlayerPrefs.SetInt(keyTD, TTTTD);
        PlayerPrefs.SetInt(keyWG, TTTWG);
        PlayerPrefs.SetInt(keyWW, TTTWW);
        PlayerPrefs.SetInt(keyWD, TTTWD);
    }

}
