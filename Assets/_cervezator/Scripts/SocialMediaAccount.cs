using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SocialMediaAccount : MonoBehaviour
{
    public TextMeshProUGUI link;

    public void OpenLink() {
        Application.OpenURL(link.text);
    }

}
