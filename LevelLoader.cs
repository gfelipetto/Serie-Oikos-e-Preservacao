using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public Animator transition;



    public void LoadScene ( string scene ) {
        //SceneManager.LoadScene(scene, LoadSceneMode.Single);
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scene);

        StartCoroutine(LoadLevel(sceneIndex));
    }

    IEnumerator LoadLevel(int sceneIndex ) {
        transition.SetTrigger("Start");
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(1.3f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }
}
