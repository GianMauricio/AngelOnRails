using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using TMPro;
using UnityEngine.UI;


///KEY HASHES
/// Wtf is this access token
/// GGQVlhS0R5OFBGbjlaLWI0ZA1ZA2QTVwVDE5eWY4d2Y5WGlKek94Y25TVlRiN0ZAhZAWdXYnQ1Y0JsWm5CVlJWYThBNUFDdElzYUVMZA0FSWm9HRFlsRDdhSTVuZAUpHQThKMHUtVW9aLXBrN0ozV2dhMTVfdy1sWFVFV1VBd0E5YzAyMU03VkVhMF9TOFpHd1U5em5KalJadnZAqSFRub0tqOHQxOGVB
public class FBManager : MonoBehaviour
{
    public GameObject ShareUI;
    public GameObject ResultUI;
    public TextMeshProUGUI UploadingText; /*Because I'm lazy and I don't wanna do the weird GetComponentsInChildren*/

    private void Awake()
    {
        Debug.Log("FB init");
        if (!FB.IsInitialized)
        {
            FB.Init(onFBInitialize, OnHideFB);
            Debug.Log("Success");
        }

        else
        {
            FB.ActivateApp();
            Debug.Log("Not needed");
        }
    }

    private void onFBInitialize()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("FB Done init");
        }

        else
        {
            Debug.LogError("Failed to Init FB");
        }
    }

    private void OnHideFB(bool shown)
    {
        if (shown)
        {
            Time.timeScale = 1;
        }

        else
        {
            Time.timeScale = 0;
        }
    }

    //Login callback
   private void FBLoginDone(ILoginResult res)
    {
        //Check if successfully logged in
        if (FB.IsLoggedIn)
        {
            //get user data
            GetUserData();
            Debug.Log("Success user login");
        }

        else {
            Debug.LogError("User failed to login");
        }
    }

    public void LoginFB()
    {
        //check first if FB SDK is initialized
        if (FB.IsInitialized)
        {
            //Check if already logged in
            if (!FB.IsLoggedIn)
            {
                //create permissions to access user data
                List<string> permissions = new List<string>() { "public_profile", "email" };
                //call login with permissions and callback
                FB.LogInWithReadPermissions(permissions, FBLoginDone);

                UploadScreenshot();
            }
            else
            {
                //user data called here
                Debug.Log("User already logged in");
                GetUserData();
            }

        }
        else
        {
            Debug.LogError("FB not yet initialized");
        }
    }

    //callback for the query of user data
    private void GetUserDataDone(IGraphResult res)
    {
        // check if there is no error
        if (string.IsNullOrEmpty(res.Error))
        {
            //will return a dictionary of<string, object>
            //keys are the same ones used in the query
            //eg. get the users name
            string user_name = res.ResultDictionary["name"].ToString();
            //get the user email
            string email = res.ResultDictionary["email"].ToString();

            UploadScreenshot();

        }
        else
        {
            Debug.Log("Error getting user data");
        }
    }

    private void GetUserData()
    {
        //The graph API query to get the name and the email
        //For other data aside from name and email you can check out
        //https://developers.facebook.com/docs/graph-api/reference/user
        // Just remember to set the extra permissions on login and the graph explorer
        
        //when testing a login on pc
        string emailQuery = "/me?fields=name,email";
        //call the graph api
        FB.API(emailQuery, HttpMethod.GET, GetUserDataDone);
    }

    private void UploadPhotoDone(IGraphResult res)
    {
        ShareUI.SetActive(false);
        ResultUI.SetActive(true);

        Debug.Log("Attempting to upload photo");
        if (string.IsNullOrEmpty(res.Error))
        {
            Debug.Log("Uploaded photo with id: " + res.ResultDictionary["id"].ToString());
            //Add stuff to change UI here
            UploadingText.text = "Upload Complete!";
        }
        else
        {
            Debug.Log("Error uploading photo" + res.Error);
            //Add stuff to change error UI here
            UploadingText.text = "Upload Error :(";
        }
    }

    IEnumerator ScreenshotAndUpload()
    {
        ShareUI.SetActive(false);
        ResultUI.SetActive(true);
        UploadingText.text = "Uploading Image...";
        yield return new WaitForEndOfFrame();
        //Capture the current state of screen in a texture
        Texture2D screen = ScreenCapture.CaptureScreenshotAsTexture();
        //Convert Texture to bytes
        byte[] screenBytes = screen.EncodeToPNG();

        //Create a form to hold data
        WWWForm form = new WWWForm();
        // add the bytes and name the image to be sent
        form.AddBinaryData("image", screenBytes, "screenshot.png");
        //add a caption to the image
        form.AddField("caption", "Sample Screenshot");
        FB.API("me/photos", HttpMethod.POST, UploadPhotoDone, form);

        Debug.Log("Uploading image");
    }

    public void UploadScreenshot()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShareUI.SetActive(false);
            ResultUI.SetActive(true);

            UploadingText.text = "No internet!";
        }

        if (FB.IsLoggedIn)
        {
            StartCoroutine(ScreenshotAndUpload());
        }

        else
        {
            Debug.Log("Not logged in");
        }
    }
}
