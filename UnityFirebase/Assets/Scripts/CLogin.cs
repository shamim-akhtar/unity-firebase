using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLogin : MonoBehaviour
{
  #region Exposed to Inspector
  [SerializeField]
  RectTransform LoginScreen;
  [SerializeField]
  RectTransform LoginProgressScreen;
  [SerializeField]
  RectTransform ProfileScreen;
  [SerializeField]
  Button ButtonBack;
  [SerializeField]
  Text TextStatus;
  [SerializeField]
  RectTransform LoginEmailPasswordScreen;
  [SerializeField]
  Button ButtonEmailLogin;
  [SerializeField]
  Button ButtonLogin;
  [SerializeField]
  Button ButtonRegister;
  [SerializeField]
  InputField InputEmail;
  [SerializeField]
  InputField InputPassword;
  #endregion

  #region Facebook Login
  #endregion

  [SerializeField]
  CFirebase FirebaseLogin;

  string mEmail;
  string mPassword;

  // Start is called before the first frame update
  void Start()
  {
    LoginScreen.gameObject.SetActive(true);
    LoginProgressScreen.gameObject.SetActive(false);
    ProfileScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(false);

    ButtonBack.onClick.AddListener(
      delegate
      {
        Back();
      });

    ButtonRegister.onClick.AddListener(
      delegate
      {
        FirebaseLogin.CreateUserWithEmailAndPassword(InputEmail.text, InputPassword.text);
      });

    ButtonLogin.onClick.AddListener(
      delegate
      {
        FirebaseLogin.SigninWithEmailAndPassword(InputEmail.text, InputPassword.text);
      });
    //ButtonRegister.interactable = false;
    ButtonEmailLogin.onClick.AddListener(
      delegate
      {
        OnClickEmailLogin();
      });
  }

  public void OnLoginProgress()
  {
    LoginScreen.gameObject.SetActive(false);
    LoginProgressScreen.gameObject.SetActive(true);
    ProfileScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(false);

    ButtonBack.gameObject.SetActive(false);
    TextStatus.gameObject.SetActive(true);
    TextStatus.text = "Login in progress ...";
  }

  public void OnLoginSuccess()
  {
    LoginScreen.gameObject.SetActive(false);
    LoginProgressScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(false);
    ProfileScreen.gameObject.SetActive(true);
  }

  public void OnLoginFailed()
  {
    LoginScreen.gameObject.SetActive(false);
    LoginProgressScreen.gameObject.SetActive(true);
    ProfileScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(false);

    ButtonBack.gameObject.SetActive(true);
    TextStatus.gameObject.SetActive(true);
    TextStatus.text = "Login failed ...!";
  }

  void Back()
  {
    LoginScreen.gameObject.SetActive(true);
    LoginProgressScreen.gameObject.SetActive(false);
    ProfileScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(false);
  }

  void OnClickEmailLogin()
  {
    LoginScreen.gameObject.SetActive(false);
    LoginProgressScreen.gameObject.SetActive(false);
    ProfileScreen.gameObject.SetActive(false);
    LoginEmailPasswordScreen.gameObject.SetActive(true);
  }
}
