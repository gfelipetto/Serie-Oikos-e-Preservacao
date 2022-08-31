using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class DrawController : MonoBehaviour {

    private Camera m_camera;
    private GameObject brush;
    private LineRenderer currentLineRenderer;
    private Vector2 lastPos;

    GameObject brushInstance;

    public static int count = 0;
     
    public List<Transform> objectsToLine;

    public GameObject target;

    private void  Awake ( ) {
        m_camera = Camera.main;
        brush = Resources.Load<GameObject>("Prefabs/Brush1");
        Draw();
        PieceController_Cerrado s;
        float random = Random.Range(0f, 100f);
        if (random > (100 - 80) && count < 3 && this.name != "grama") {

            target = GameObject.Find(this.name);
            if (target.tag == "Grid") {
                target.transform.position = this.transform.position;
                s = target.GetComponent<PieceController_Cerrado>();
                s.isCorrect = true;
                s.canMove = false;
                target.GetComponent<SpriteRenderer>().sortingOrder = 2;
                count++;
                //Debug.Log(this.name);
            }

        }


    }

    public void Draw ( ) {
        

        foreach (Transform item in objectsToLine) {

            CreateBrush();
            PointToMousePos(item);
            FinishBrush();
        }
    }

    Color RandomColor ( ) {
        Color c;
        float r, g, b;
        r = Random.Range(0f, 1f);
        g = Random.Range(0f, 1f);
        b = Random.Range(0f, 1f);

        c = new Color(r, g, b);

        return c;

    }

    void CreateBrush ( ) {
        brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        currentLineRenderer.endColor = RandomColor();
        currentLineRenderer.SetPosition(0, new Vector2(this.transform.position.x, this.transform.position.y));
        currentLineRenderer.SetPosition(1, new Vector2(this.transform.position.x, this.transform.position.y));

    }

    void AddAPoint ( Vector2 pointPos ) {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos ( Transform finalPosition ) {
        Vector2 _finalPositon = finalPosition.position;
        if (lastPos != _finalPositon) {
            AddAPoint(_finalPositon);
            lastPos = _finalPositon;
        }
    }

    void FinishBrush ( ) {
        LineRenderer lr = brushInstance.GetComponent<LineRenderer>();
        lr.Simplify(100f);
    }

   

}