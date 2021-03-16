using UnityEngine;

public class TTTPlayerPrefs
{
    private const string keyNick = "Nickname"; //Chave para armazenar o Nickname
    private const string keySenha = "Password"; //Chave para armazenar a senha
    private const string keyRegistrado = "Registro"; //Chave para armazenar se o usuário já está registrado

    private const string keyXLiberados = "XUps"; //Chave para X liberados
    private const string keyOLiberados = "OUps"; //Chave para O liberados
    private const string keyMoedas = "Moedas"; //Chave para dinheiro


    public static int TTTOs { get; set; } = 0; //Os liberados
    public static int TTTXs { get; set; } = 0; //Xs liberados
    public static int TTTMoedas { get; set; } = 0; // Dinheiro :-)

    public static string TTTNickname { get; private set; } = ""; //Player Nickname
    public static string TTTPassword { get; private set; } = ""; //Player Password
    public static bool TTTRegistrado { get; private set; } = false; //Se o usurário já foi registrado

    public static bool IsRegistrado()
    {
        TTTRegistrado = PlayerPrefs.GetInt(keyRegistrado, 0) != 0; //Converte de int para bool
        return TTTRegistrado;
    }
    public static void RecuperaTTTPrefs() //Recupera as preferências
    {
        TTTNickname = PlayerPrefs.GetString(keyNick, "");
        TTTPassword = PlayerPrefs.GetString(keySenha, "");
        TTTOs = PlayerPrefs.GetInt(keyOLiberados, 0);
        TTTXs = PlayerPrefs.GetInt(keyXLiberados, 0);
        TTTMoedas = PlayerPrefs.GetInt(keyMoedas, 0);
    }
    public static void Registra(string novoNick, string novaSenha)
    {
        if ((novoNick.Length > 0)&&(novaSenha.Length>0))
        {
            TTTNickname = novoNick;
            TTTPassword = novaSenha;
            TTTRegistrado = true;
            TTTMoedas = 0;
            TTTOs = 0;
            TTTXs = 0;
            PlayerPrefs.SetString(keyNick, TTTNickname);
            PlayerPrefs.SetString(keySenha, TTTPassword);
            PlayerPrefs.SetInt(keyRegistrado, TTTRegistrado ? 1 : 0); //Converte de bool para int e salva variável
            PlayerPrefs.SetInt(keyMoedas, TTTMoedas);
            PlayerPrefs.SetInt(keyXLiberados, TTTXs);
            PlayerPrefs.SetInt(keyOLiberados, TTTOs);
        }
    }
    public static void ApagaRegistro()
    {
        TTTNickname = "";
        TTTPassword = "";
        TTTRegistrado = false;
        TTTMoedas = 0;
        TTTOs = 0;
        TTTXs = 0;
        PlayerPrefs.SetInt(keyMoedas, TTTMoedas);
        PlayerPrefs.SetInt(keyXLiberados, TTTXs);
        PlayerPrefs.SetInt(keyOLiberados, TTTOs);
        PlayerPrefs.SetString(keyNick, TTTNickname);
        PlayerPrefs.SetString(keySenha, TTTPassword);
        PlayerPrefs.SetInt(keyRegistrado, TTTRegistrado ? 1 : 0); //Converte de bool para int e salva variável
    }








    private const string keyVolume = "Volume"; //Chave para armazenar o Volume
    private const string keyMudo = "Mudo";  //Chave para armazenar o Mudo
    private const string keyIndiceMusica = "IndiceMusica";  //Chave para armazenar o índice da música
    private const string keyIntervalo = "Intervalo";  //Chave para armazenar o Intervalo
    private const string keyNunca = "Nunca";  //Chave para armazenar o Nunca
    private const string keyExecucoes = "Execucoes";  //Chave para armazenar o Execucoes
    public static float Volume { get; private set; } = .5f; //Volume em tempo de execução
    public static bool Mudo { get; private set; } = false; //Mudo em tempo de execução
    public static int IndiceMusica { get; private set; } = 0; //Indice em tempo de execução
    public static int Intervalo { get; private set; } = 10; //Intervalo em tempo de execução
    public static bool Nunca { get; private set; } = true; //Nunca em tempo de execução
    public static int Execucoes { get; private set; } = 0; //Número de execuções em tempo de execução

    public static void AtualizaVolume(float novoVolume) // Atualiza e salva volume
    {
        if ((novoVolume >= 0f) && (novoVolume <= 1f)) //Verifica se está entre 0 e 1
        {
            Volume = novoVolume; //Atualiza variável
            PlayerPrefs.SetFloat(keyVolume, Volume); //Salva variável
        }
    }
    public static void AtualizaMudo(bool novoMudo) //Atualiza e salva Mudo 
    {
        Mudo = novoMudo;  //Atualiza variável
        PlayerPrefs.SetInt(keyMudo, Mudo ? 1 : 0); //Converte de bool para int e salva variável
    }
    public static void AtualizaIndiceMusica(int novoIndice) //Atualiza e salva Indice
    {
        if (novoIndice >= 0) // Verifica se não é negativo
        {
            IndiceMusica = novoIndice;
            PlayerPrefs.SetInt(keyIndiceMusica, IndiceMusica);
        }
    }
    public static void AtualizaIntervalo(int novoIntervalo) //Atualiza e salva Intervalo
    {
        if (novoIntervalo > 0) // Verifica se é maior que zero
        {
            Intervalo = novoIntervalo;
            PlayerPrefs.SetInt(keyIntervalo, Intervalo);
        }
    }
    public static void AtualizaNunca(bool novoNunca) //Atualiza e salva Nunca 
    {
        Nunca = novoNunca;  //Atualiza variável
        PlayerPrefs.SetInt(keyNunca, Nunca ? 1 : 0); //Converte de bool para int e salva variável
    }
    public static void AtualizaExecucoes(int novoExecucoes) //Atualiza e salva Execucoes
    {
        if (novoExecucoes >= 0) // Verifica se não é negativo
        {
            Execucoes = novoExecucoes;
            PlayerPrefs.SetInt(keyExecucoes, Execucoes);
        }
    }
    public static void RecuperaPlayerPrefs() //Recupera as preferências
    {
        Volume = PlayerPrefs.GetFloat(keyVolume, .5f);
        Mudo = PlayerPrefs.GetInt(keyMudo, 0) != 0; //Converte de int para bool
        IndiceMusica = PlayerPrefs.GetInt(keyIndiceMusica, 0);
        Intervalo = PlayerPrefs.GetInt(keyIntervalo, 10);
        Nunca = PlayerPrefs.GetInt(keyNunca, 1) != 0;  //Converte de int para bool
        Execucoes = PlayerPrefs.GetInt(keyExecucoes, 0);
    }

}
