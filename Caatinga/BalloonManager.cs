using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalloonManager : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI feedbackText;

    public TextMeshProUGUI FeedbackText { get => feedbackText; set => feedbackText = value; }

    public void MoveTo(Transform target ) {

        this.transform.position = target.position;
    }

}
