using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTTConfig : MonoBehaviour
{
    [SerializeField] TTTMainJMF MyMain;
    [SerializeField] Text[] PlayerScsores;
    [SerializeField] Text Nickname;
    [SerializeField] Text Coins;
    [SerializeField] Text Xs;
    [SerializeField] Text Os;
    [SerializeField] Image NextX;
    [SerializeField] Image NextO;
    [SerializeField] Image SelectX;
    [SerializeField] Image SelectO;
    [SerializeField] Slider SliderX;
    [SerializeField] Slider SliderO;
    [SerializeField] Button LiberaX;
    [SerializeField] Button LiberaO;




    // Start is called before the first frame update
    void Start()
    {
        SliderX.onValueChanged.AddListener(AcaoSliderX);
        SliderO.onValueChanged.AddListener(AcaoSliderO);
        LiberaX.onClick.AddListener(AcaoCompraX);
        LiberaO.onClick.AddListener(AcaoCompraO);
        AcaoAtualizaTela();
    }


    public void AcaoCompraX() //Desbloquei próximo X
    {
        if ((TTTPlayerPrefs.TTTMoedas > 0) && (TTTPlayerPrefs.TTTXs < 10))
        {
            TTTPlayerPrefs.CompraX();
            AcaoAtualizaTela();
        }
    }

    public void AcaoCompraO() //Desbloquia próximo O
    {
        if ((TTTPlayerPrefs.TTTMoedas > 0) && (TTTPlayerPrefs.TTTOs < 10))
        {
            TTTPlayerPrefs.CompraO();
            AcaoAtualizaTela();
        }
    }

    public void AcaoAtualizaTela()
    {
        PlayerScsores[0].text = TTTPlayerPrefs.TTTTG.ToString();
        PlayerScsores[1].text = TTTPlayerPrefs.TTTTW.ToString();
        PlayerScsores[2].text = TTTPlayerPrefs.TTTTD.ToString();
        PlayerScsores[3].text = TTTPlayerPrefs.TTTWG.ToString();
        PlayerScsores[4].text = TTTPlayerPrefs.TTTWW.ToString();
        PlayerScsores[5].text = TTTPlayerPrefs.TTTWD.ToString();

        Nickname.text = TTTPlayerPrefs.TTTNickname;
        Coins.text = TTTPlayerPrefs.TTTMoedas.ToString();
        Xs.text = $"Xs liberados: {TTTPlayerPrefs.TTTXs}";
        Os.text = $"Os liberados: {TTTPlayerPrefs.TTTOs}";

        SliderX.maxValue = TTTPlayerPrefs.TTTXs;
        SliderO.maxValue = TTTPlayerPrefs.TTTOs;
        SliderX.value = TTTPlayerPrefs.TTTXe;
        SliderO.value = TTTPlayerPrefs.TTTOe;
        SliderX.interactable = SliderX.maxValue > 1 ? true : false;
        SliderO.interactable = SliderO.maxValue > 1 ? true : false;

        LiberaX.interactable = TTTPlayerPrefs.TTTMoedas > 0 ? true : false;
        LiberaO.interactable = TTTPlayerPrefs.TTTMoedas > 0 ? true : false;
        LiberaX.gameObject.SetActive(TTTPlayerPrefs.TTTXs < 10);
        LiberaO.gameObject.SetActive(TTTPlayerPrefs.TTTOs < 10);

        NextX.sprite = MyMain.getXImage(TTTPlayerPrefs.TTTXs);
        NextO.sprite = MyMain.getOImage(TTTPlayerPrefs.TTTOs);
        NextX.gameObject.SetActive(TTTPlayerPrefs.TTTXs < 10);
        NextO.gameObject.SetActive(TTTPlayerPrefs.TTTOs < 10);

        SelectX.sprite = MyMain.getXImage(TTTPlayerPrefs.TTTXe - 1);
        SelectO.sprite = MyMain.getOImage(TTTPlayerPrefs.TTTOe - 1);
    }


    public void AcaoSliderX(float value) //Atualiza escolha de X do usuário
    {
        TTTPlayerPrefs.TTTXe = (int)value;
        SelectX.sprite = MyMain.getXImage(TTTPlayerPrefs.TTTXe-1);
    }
    public void AcaoSliderO(float value) //Atualiza escolho de O do usuário
    {
        TTTPlayerPrefs.TTTOe = (int)value;
        SelectO.sprite = MyMain.getOImage(TTTPlayerPrefs.TTTOe-1);
    }
    public void AcaoBotaoCancela() //Resposta aos botões de cancelar: recupera prefs e atualiza tela
    {
        TTTPlayerPrefs.RecuperaTTTPrefs();
        AcaoAtualizaTela();
    }
    public void AcaoBotaoConfirma() //Respost ao botão OK: salva Prefs
    {
        TTTPlayerPrefs.SaveConfig();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
