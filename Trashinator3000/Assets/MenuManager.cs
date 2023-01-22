using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject loginMenu;
    public TMP_Text username;
    public bool openMenu;
    // Start is called before the first frame update
    void Start()
    {
        if(username.text != "") {
            openMenu = false;
        }
        else {
            openMenu = true;
        }
        loginMenu.SetActive(openMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleLogin() {
        if(username.text!="") {
            openMenu = !openMenu;
            loginMenu.SetActive(openMenu);
        }
    }
}
