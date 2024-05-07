using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UISignIn : MonoBehaviour
{
    [SerializeField] Text errorText;
    [SerializeField] Canvas canvas;
     string username, password;
    void OnEnable() {
        UserAccountManager.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    void OnDisable() {
        UserAccountManager.OnSignInFailed.RemoveListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignInSuccess);
        
    }

    void OnSignInFailed(string error){
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }
    void OnSignInSuccess(){
        Debug.Log("Login");
        //canvas.enabled = false;
        SceneManager.LoadScene(1);
    }

    public void UpdateUsername (string _username){
        username = _username;
    }
    public void UpdatePassword (string _password){
        password = _password;
    }

    public void SignIn() {
        UserAccountManager.Instance.SignIn (username, password);
    }

}
