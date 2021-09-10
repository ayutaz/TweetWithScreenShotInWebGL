using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;
using System;

namespace TweetWithScreenShot
{
    public class TweetManager : MonoBehaviour
    {

        private static TweetManager _sinStance;
        public string[] hashTags;

        [SerializeField]
        private string clientID;

        public string ClientID
        {
            get
            {
                if (string.IsNullOrEmpty(clientID)) throw new Exception("ClientIDをセットしてください");
                return clientID;
            }
        }

        public static TweetManager Instance
        {
            get
            {
                if (_sinStance == null)
                {
                    _sinStance = FindObjectOfType<TweetManager>();
                    if (_sinStance == null)
                    {
                        var obj = new GameObject(nameof(TweetManager));
                        _sinStance = obj.AddComponent<TweetManager>();
                    }
                }
                return _sinStance;
            }
        }

        public static IEnumerator TweetWithScreenShot(string text)
        {
            yield return new WaitForEndOfFrame();
            var tex = ScreenCapture.CaptureScreenshotAsTexture();

            // imgurへアップロード
            string uploadedURL = "";

            UnityWebRequest www;

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("image", Convert.ToBase64String(tex.EncodeToJPG()));
            wwwForm.AddField("type", "base64");

            www = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwForm);

            www.SetRequestHeader("AUTHORIZATION", "Client-ID " + Instance.ClientID);

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data: " + www.downloadHandler.text);
                XDocument xDoc = XDocument.Parse(www.downloadHandler.text);
                //Twitterカードように拡張子を外す
                string url = xDoc.Element("data").Element("link").Value;
                url = url.Remove(url.Length - 4, 4);
                uploadedURL = url;
            }

            text += " " + uploadedURL;
            string hashtags = "&hashtags=";
            if (_sinStance.hashTags.Length > 0)
            {
                hashtags += string.Join (",", _sinStance.hashTags);
            }

            // ツイッター投稿用URL
            var tweetURL = "http://twitter.com/intent/tweet?text=" + text + hashtags;

#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", TweetURL));
#elif UNITY_EDITOR
            System.Diagnostics.Process.Start (tweetURL);
#else
            Application.OpenURL(TweetURL);
#endif
        }
    }
}
