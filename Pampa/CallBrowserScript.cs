using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CallBrowserScript : MonoBehaviour {

    public void PampaCall ( ) {
        //button.onClick.AddListener(( ) => AbreURL($"https://www.wikiaves.com.br/wiki/" + CropTextures.ave));
        AbreURL("https://www.wikiaves.com.br/wiki/" + CropTextures.ave);
    }

    public void AbreURL ( string url ) {
        Application.ExternalEval($"window.open(\"{url}\",\"_blank\")");
        //Application.ExternalEval($"window.open(\"{url}\")");
        //Application.OpenURL(url);

    }
}
