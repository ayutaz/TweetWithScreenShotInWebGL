using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;
using System;

namespace TweetWithScreenShot
{
    public class TweetManager : MonoBehaviour
    {
        private static TweetManager sinstance;
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
                if (sinstance == null)
                {
                    sinstance = FindObjectOfType<TweetManager>();
                    if (sinstance == null)
                    {
                        var obj = new GameObject(typeof(TweetManager).Name);
                        sinstance = obj.AddComponent<TweetManager>();
                    }
                }
                return sinstance;
            }
        }

        public static IEnumerator TweetWithScreenShot(string text)
        {
            // フレームの終了まで待機（スクリーンショットを正しく取得するため）
            yield return new WaitForEndOfFrame();

            // スクリーンショットをテクスチャとして取得
            var tex = ScreenCapture.CaptureScreenshotAsTexture();

            // 取得した画像をimgurにアップロード
            string uploadedURL = "";

            // UnityWebRequestを使用してPOSTリクエストを送信

            // 送信するフォームデータを作成
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("image", Convert.ToBase64String(tex.EncodeToJPG()));
            wwwForm.AddField("type", "base64");

            // imgurのAPIエンドポイントにPOSTリクエストを作成
            var uploadRequest = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwForm);

            // 認証用のヘッダーを追加
            uploadRequest.SetRequestHeader("AUTHORIZATION", "Client-ID " + Instance.ClientID);

            // リクエストを送信して応答を待機
            yield return uploadRequest.SendWebRequest();

            // リクエストの結果をチェック
            if (uploadRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("アップロードに失敗しました: " + uploadRequest.error);
            }
            else
            {
                Debug.Log("アップロード成功: " + uploadRequest.downloadHandler.text);
                // 応答から画像のURLを取得
                XDocument xDoc = XDocument.Parse(uploadRequest.downloadHandler.text);
                // URLから拡張子を削除（Twitterカード対応のため）
                string url = xDoc.Element("data").Element("link").Value;
                url = url.Remove(url.Length - 4, 4);
                uploadedURL = url;
            }

            // テキストに画像のURLを追加
            text += " " + uploadedURL;

            // ハッシュタグを生成
            string hashtags = sinstance.hashTags.Length > 0 ? string.Join(",", sinstance.hashTags) : "";

            // テキストとハッシュタグをURLエンコード
            string escapedText = UnityWebRequest.EscapeURL(text);
            string escapedHashtags = UnityWebRequest.EscapeURL(hashtags);

            // ツイッター投稿用URLを作成
            string tweetURL = "https://twitter.com/intent/tweet?text=" + escapedText;

            if (!string.IsNullOrEmpty(escapedHashtags))
            {
                tweetURL += "&hashtags=" + escapedHashtags;
            }

            // ツイート画面を開く
    #if UNITY_WEBGL && !UNITY_EDITOR
            OpenWindow(TweetURL);
    #elif UNITY_EDITOR
            System.Diagnostics.Process.Start(tweetURL);
    #else
            Application.OpenURL(TweetURL);
    #endif
        }
    }
}