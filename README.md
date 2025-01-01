# TweetWithScreenShotInWebGL

## オリジナル版からの変更点
* 改行の対応
* www.isNetworkErrorをwww.resultに変更
* unitypackageの自動生成
* Package Managerに対応

## 使い方

### unitypackageのインストール
1. [Releases]()から最新のunitypackageをダウンロード
2. Unityでプロジェクトを開き、ダウンロードしたunitypackageをインポート

### Package Managerのインストール
1. 


UnityのWebGLでサムネイル付き画像ツイートをするサンプルです
おそらくモバイルや他のいろんな環境でも動くと思いますが動作はあまり確認していません

導入方法はこちらにとてもわかりやすいものを書いていただきました！
https://unity-senpai.hatenablog.com/entry/2019/07/07/130111

TweetManagerをアタッチしたGameObjectをhierarchyに置き、InspectorでImgurのClientKeyを設定し、

```
StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot("Hello!"));
```

という風に呼び出します
