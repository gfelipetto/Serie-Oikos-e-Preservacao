using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundInfo : MonoBehaviour {

    [SerializeField] private CaatingaManager manager;
    [SerializeField] private int timesToLoseGround = 3;

    public GroundState currentStateGround = GroundState.NotPlowed;
    [SerializeField] private CaatingaManager.ActionTypeSeedEnum currentStateSeed = CaatingaManager.ActionTypeSeedEnum.Null;
    [SerializeField] private GroundInfo[] closeGrounds;

    private CaatingaManager.ActionTypeSeedEnum previousSeedPlanted;
    private Coroutine co, needWater;
    private CaatingaManager cm;
    public static float wrongParamater = -15f;
    public static float corretParamater = 15f;
    public ParticleSystem ps_water;
    public ParticleSystem ps_reaped;

    public enum GroundState {
        Null,
        Ruined, //Arruinado
        NotPlowed, //Precisa Arar
        Plowed, // Arado
        Planted, // Plantado
        Watered, // Regado
        Reaped, // Colhido
        Mistake
    }

    private void Start ( ) {
        cm = FindObjectOfType<CaatingaManager>();
    }
    public void MakeActionTools ( CaatingaManager.ActionTypeToolsEnum currentTypeTools ) {
        switch (currentTypeTools) {
            case CaatingaManager.ActionTypeToolsEnum.Plow:
                if (currentStateGround == GroundState.NotPlowed || currentStateGround == GroundState.Plowed) SwitchStateGround();
                else if (currentStateGround == GroundState.Ruined) {
                    //manager.Balloon.FeedbackText.SetText("Plantação morta");
                    //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
                    break;
                } else if (currentStateGround == GroundState.Null) {
                    //manager.Balloon.FeedbackText.SetText("Plantação restrita");
                    //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
                    break;
                } else {
                    LosePlantationGround();
                }
                break;

            case CaatingaManager.ActionTypeToolsEnum.Water:
                if (currentStateGround == GroundState.Planted) SwitchStateGround();
                break;

            case CaatingaManager.ActionTypeToolsEnum.Reap:
                if (currentStateGround == GroundState.Reaped) ReapGround();
                break;

            default:
                break;
        }
    }
    public void MakeActionSeed ( CaatingaManager.ActionTypeSeedEnum currenTypeSeed ) {
        if (currentStateGround == GroundState.Plowed && currentStateSeed == CaatingaManager.ActionTypeSeedEnum.Null) {
            SetGroundPlant(currenTypeSeed);
        } else {
            manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
        }

    }
    public string FilterGroundInfo ( GroundState status ) {
        switch (status) {
            case GroundState.Null:
                return "Plantação Restrita";
                break;
            case GroundState.Ruined:
                return "Plantação morta";
                break;
            case GroundState.NotPlowed:
                return "Não plantou ...";
                break;
            case GroundState.Plowed:
                return "Arou ...";
                break;
            case GroundState.Planted:
                return "Plantou ...";
                break;
            case GroundState.Watered:
                return "Regou ...";
                break;
            case GroundState.Reaped:
                return "Colheu ...";
                break;
            case GroundState.Mistake:
                return "Não plantou ...";
                break;
            default:
                return "";
                break;
        }
    }

    private void SwitchStateGround ( ) {
        switch (currentStateGround) {
            case GroundState.NotPlowed:
                SetGroundPlow();
                break;

            case GroundState.Planted:
                SetGroundWater();
                break;

            case GroundState.Reaped:
                ReapGround();
                break;

            default:
                break;
        }
    }

    private void LosePlantationGround ( ) {
        if (co != null) StopCoroutine(co);
        SetTypesToDeFault();
        //manager.Balloon.FeedbackText.SetText("Perdeu a Plantação");
        currentStateGround = GroundState.Ruined;
        //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
        currentStateGround = GroundState.NotPlowed;
        manager.SendMessage("ChangeValueProgressBar", wrongParamater);
    }
    private void SetGroundPlow ( ) {
        currentStateGround = GroundState.Plowed;
        //manager.Balloon.FeedbackText.SetText("Arou ...");
        //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
    }
    private void SetGroundPlant ( CaatingaManager.ActionTypeSeedEnum type ) {
        if (type != CaatingaManager.ActionTypeSeedEnum.Null) {
            if (type == previousSeedPlanted) {
                manager.SendMessage("ChangeValueProgressBar", wrongParamater);
                timesToLoseGround--;
            }
            foreach (GroundInfo gi in closeGrounds) {
                if (gi.currentStateSeed == type) {

                    currentStateGround = GroundState.Mistake;
                    //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
                    manager.SendMessage("ChangeValueProgressBar", wrongParamater);
                    currentStateGround = GroundState.Plowed;
                    //manager.Balloon.FeedbackText.SetText("Não plantou ...");
                    return;
                }
            }
            foreach (var item in cm.seedImages) {
                if (item.name == type.ToString()) {
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item;
                }
            }
            currentStateSeed = type;
            currentStateGround = GroundState.Planted;
            manager.SendMessage("ChangeValueProgressBar", corretParamater);
            needWater = StartCoroutine(TimeToChangeType(5f, GroundState.NotPlowed));
            //manager.Balloon.FeedbackText.SetText("Plantou ...");
            //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
            cm.ResetTimerPlantation();
        }

    }
    private void SetGroundWater ( ) {
        if (co == null) {
            if (needWater != null) {
                StopCoroutine(needWater);
                needWater = null;
            }
            currentStateGround = GroundState.Watered;
            co = StartCoroutine(TimeToChangeType(15f, GroundState.Reaped));
            //manager.Balloon.FeedbackText.SetText("Regou ...");
            //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));
        }
    }
    private void ReapGround ( ) {
        previousSeedPlanted = currentStateSeed;
        SetTypesToDeFault();
        manager.SendMessage("ChangeValueProgressBar", corretParamater);
        //manager.Balloon.FeedbackText.SetText("Colheu ...");
        //manager.Balloon.FeedbackText.SetText(FilterGroundInfo(currentStateGround));

    }
    private void SetTypesToDeFault ( ) {
        currentStateSeed = CaatingaManager.ActionTypeSeedEnum.Null;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
        if (timesToLoseGround <= 0) currentStateGround = GroundState.Ruined;
        else currentStateGround = GroundState.NotPlowed;
    }
    private IEnumerator TimeToChangeType ( float time, GroundState nextType ) {

        float currentTime = 0f;
        if (currentStateGround == GroundState.Planted) {
            yield return new WaitForSecondsRealtime(5f);
            ps_water.gameObject.SetActive(true);
        }
        do {
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        currentStateGround = nextType;
        if (currentStateGround == GroundState.Reaped) {
            ps_reaped.gameObject.SetActive(true);
        }

        if (co != null) {
            co = null;

        }
        if (needWater != null) {
            LosePlantationGround();
            needWater = null;
        }


        
    }
}
