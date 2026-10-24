using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class social : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openwhatsapp()
    {
        string serial = SystemInfo.deviceUniqueIdentifier;
        Application.OpenURL("whatsapp://send?phone=989916944294&text=" + "SN" + serial+"-->PapitaFruits");
    }

    public void call()
    {
#if UNITY_ANDROID
        Application.OpenURL("tel://09127719978");
#endif
    }

    public void openwhatsappnew(string witch)
    {

        string serial = witch;
        string whats = UnityEngine.PlayerPrefs.GetString("whatsCode");

        Application.OpenURL(whats + serial);
    }
}
