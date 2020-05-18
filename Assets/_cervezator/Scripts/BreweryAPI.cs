using PlayFab;
using PlayFab.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BreweryAPI : MonoBehaviour
{
    public TextMeshProUGUI breweryNameText;
    public Transform socialMediaContainer;
    public SocialMediaAccount socialMediaAccountPrefab;

    void Start()
    {
        RandomBrewery();    
    }

    public void RandomBrewery() {
        StartCoroutine(RandomBreweryRequest());
    }

    IEnumerator RandomBreweryRequest() {
        // Consultar la base de datos de cervecerias
        UnityWebRequest www = UnityWebRequest.Get("https://sandbox-api.brewerydb.com/v2/brewery/random/?key=c18b053c1490e5a8d2593a9586913cb7&withSocialAccounts=Y");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            // Si ocurre un error escribirlo en la consola
            Debug.Log(www.error);
        } else {
            // Si no hay error leer el resultado en formato json y mostrar los datos relevantes en pantalla
            Debug.Log(www.downloadHandler.text);
            var jsonSerializer = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
            var result = jsonSerializer.DeserializeObject<Dictionary<string,object>>(www.downloadHandler.text);
            var data = result["data"] as JsonObject;

            // Mostrar el nombre de la cerveceria
            Debug.Log(data["name"] as string);
            breweryNameText.text = data["name"] as string;

            // Borrar los anteriores links a redes sociales
            foreach (Transform t in socialMediaContainer) {
                Destroy(t.gameObject);
            }

            // Si la nueva cerveceria tiene redes sociales, mostrar los links
            if (data.Keys.Contains("socialAccounts")) {
                var socialAccounts = data["socialAccounts"] as JsonArray;
                foreach (JsonObject socialAccount in socialAccounts) {
                    if (socialAccount.Keys.Contains("link")) {
                        Debug.Log(socialAccount["link"]);
                        SocialMediaAccount newSocialMediaAccount = Instantiate(socialMediaAccountPrefab, socialMediaContainer);
                        newSocialMediaAccount.link.text = socialAccount["link"] as string;
                    }
                }
            }
        }
    }


}
