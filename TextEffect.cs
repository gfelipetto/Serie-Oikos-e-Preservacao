using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour {

    private TextMeshProUGUI myText;
    private List<int> wordIndex = new List<int>();
    public List<string> wordsToEffect = new List<string>();


    void Start ( ) {
        myText = this.GetComponent<TextMeshProUGUI>() ?? this.gameObject.AddComponent<TextMeshProUGUI>();
        int i = 0;

        myText.ForceMeshUpdate();
        foreach (TMP_WordInfo w in myText.textInfo.textComponent.textInfo.wordInfo) {
            foreach (string wEffect in wordsToEffect) {
                if (string.Equals(w.GetWord(),wEffect,StringComparison.OrdinalIgnoreCase)) {
                    wordIndex.Add(i);

                }
            }
            i++;
        }
    }
    void FixedUpdate ( ) {
        WaveEffect();
    }

    void WaveEffect (  ) {
        int _wordIndex = 0;
        myText.ForceMeshUpdate();

        foreach (int item in wordIndex) {
            _wordIndex = item;
            TMP_WordInfo[] wordInfo = myText.textInfo.wordInfo;

            for (int i = wordInfo[_wordIndex].firstCharacterIndex; i <= wordInfo[_wordIndex].lastCharacterIndex; i++) {

                TMP_CharacterInfo charInfo = wordInfo[_wordIndex].textComponent.textInfo.characterInfo[i];
                if (!charInfo.isVisible) {
                    continue;
                }

                TMP_MeshInfo meshInfo = wordInfo[_wordIndex].textComponent.textInfo.meshInfo[charInfo.materialReferenceIndex];

                for (int j = 0; j < 4; j++) {
                    int index = charInfo.vertexIndex + j;
                    Vector3 verticePos = meshInfo.vertices[index];
                    meshInfo.vertices[index] = verticePos + new Vector3(0, Mathf.Sin(Time.time * 2f + verticePos.x * 0.01f) * 5f, 0);
                }

            }
            for (int i = 0; i < wordInfo[_wordIndex].textComponent.textInfo.meshInfo.Length; i++) {
                TMP_MeshInfo meshInfo = wordInfo[_wordIndex].textComponent.textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                wordInfo[_wordIndex].textComponent.UpdateGeometry(meshInfo.mesh, i);
            }
        }

    }












}
