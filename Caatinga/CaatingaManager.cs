using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaatingaManager : MonoBehaviour
{

    [SerializeField, Tooltip("Tempo total de jogo em segundos")] private float timer = 120f;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private TextMeshProUGUI txtTimerPlantation;

    [SerializeField] private Slider progressBar;

    [SerializeField] private BalloonManager balloon;

    [SerializeField] private DissolveCaatingaEffect dissolve;

    [SerializeField] private ActionTypeToolsEnum currentTypeToolsVar = ActionTypeToolsEnum.Null;
    [SerializeField] private ActionTypeSeedEnum currentTypeSeedVar = ActionTypeSeedEnum.Null;


    private GroundInfo currentGroundInfo;
    private Coroutine co;

    public GameObject finalScreenObjLose;
    public GameObject finalScreenObjWin;
    public GameObject[] birdsImages;
    public Sprite[] seedImages;
    public BalloonManager Balloon { get => balloon; set => balloon = value; }

    public Image feedbackImg;
    public float x;
    public float y;
    public enum ActionTypeToolsEnum
    {
        Null, // Sem seleção
        Plow, // Arar
        Plant, // Plantar
        Water, // Regar
        Reap // Colher
    };

    public enum ActionTypeSeedEnum
    {
        Null, //Sem seleção
        Pineapple, // Abacaxi
        Licuri, // Licuri
        Corn // Milho
    }

    private void Awake()
    {
        StartCoroutine(TimeCooldown());
        co = StartCoroutine(TimeCooldownPlantation());
        feedbackImg.color = Color.clear;
    }
    private void FixedUpdate ( ) {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x += x;
        mousePos.y += y;
        feedbackImg.transform.position = mousePos;
    }
    private IEnumerator TimeCooldown()
    {
        while (timer > 0)
        {
            txtTimer.SetText($"Tempo de Jogo: {timer.ToString("F0")}");
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

        }
        yield return "Tempo Esgotado";
        //finalScreenObj.SetActive(true);
        finalScreenObjWin.SetActive(true);
        AudioManager.instance.Play("Win");
    }
    private IEnumerator TimeCooldownPlantation()
    {
        float timeTolosePlantation = 40f;
        while (timeTolosePlantation >= 0)
        {
            txtTimerPlantation.SetText($"Tempo Plantação: {timeTolosePlantation.ToString("F0")}");
            timeTolosePlantation -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        finalScreenObjLose.SetActive(true);
        AudioManager.instance.Play("Lose");
    }
    private void ChangeValueProgressBar(float value)
    {
        progressBar.value -= (1f / -value);
        dissolve.Dis();

        if (progressBar.value < .2f)
        {
            birdsImages[3].SetActive(false);
            birdsImages[2].SetActive(false);
            birdsImages[1].SetActive(false);
            birdsImages[0].SetActive(true);
        }

        else if (progressBar.value < .5f && progressBar.value >= 2f)
        {
            birdsImages[3].SetActive(false);
            birdsImages[2].SetActive(false);
            birdsImages[1].SetActive(true);
            birdsImages[0].SetActive(true);
        }

        else if (progressBar.value < .85f && progressBar.value >= .5f)
        {
            birdsImages[3].SetActive(false);
            birdsImages[2].SetActive(true);
            birdsImages[1].SetActive(true);
            birdsImages[0].SetActive(true);
        }

        else if (progressBar.value >= .85f)
        {
            birdsImages[3].SetActive(true);
            birdsImages[2].SetActive(true);
            birdsImages[1].SetActive(true);
            birdsImages[0].SetActive(true);
        }

        
        if (progressBar.value < 0.15f) {
            finalScreenObjLose.SetActive(true);
            AudioManager.instance.Play("Lose");
        }
    }
    private void LateUpdate ( ) {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            SwitchActionToNull();
            feedbackImg.color = Color.clear;
            feedbackImg.sprite = null;
        }
    }
    private void SwitchActionToNull()
    {
        currentTypeToolsVar = ActionTypeToolsEnum.Null;
        currentTypeSeedVar = ActionTypeSeedEnum.Null;

    }
    public void ResetTimerPlantation()
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(TimeCooldownPlantation());
    }
    public void SelectActionType(string _actionTypeVar)
    {
        SwitchActionToNull();
        currentTypeToolsVar = (ActionTypeToolsEnum)System.Enum.Parse(typeof(ActionTypeToolsEnum), _actionTypeVar);
    }
    public void SelectSeedType(string _seedTypeVar)
    {
        SwitchActionToNull();
        currentTypeSeedVar = (ActionTypeSeedEnum)System.Enum.Parse(typeof(ActionTypeSeedEnum), _seedTypeVar);
        currentTypeToolsVar = ActionTypeToolsEnum.Plant;
    }
    public void OnClickGround(GroundInfo groundInfo)
    {
        currentGroundInfo = groundInfo;
        if (currentTypeToolsVar != ActionTypeToolsEnum.Null && currentTypeSeedVar == ActionTypeSeedEnum.Null)
        {
            currentGroundInfo.MakeActionTools(currentTypeToolsVar);
        }
        else currentGroundInfo.MakeActionSeed(currentTypeSeedVar);
        balloon.FeedbackText.SetText(currentGroundInfo.FilterGroundInfo(currentGroundInfo.currentStateGround));
    }

    public void GetImageOnClick (Image img ) {
        feedbackImg.color = Color.white;
        feedbackImg.sprite = img.sprite;
    }

   
}