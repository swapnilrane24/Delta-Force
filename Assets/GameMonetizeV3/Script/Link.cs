using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour
{
#if  UNITY_WEBGL || UNITY_EDITOR


    public void ______OpenLinkToYourWebsite()
    {
        openWindow("https://www.gamemonetize.com");
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);
#endif
}