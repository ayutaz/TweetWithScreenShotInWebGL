# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.2] - 2025-1-1

### fixed
- WebGL Build Error

## [1.0.1] - 2025-1-1

### fixed
- **forked from original repository**
- Fixed an issue where tweeting from Unity only included part of the text and did not attach the image.
    - Modified the code to URL-encode the tweet text and hashtags to handle special characters and Japanese text properly.
        - Implemented encoding using `UnityWebRequest.EscapeURL`.
- Replaced deprecated use of `UnityWebRequest.isNetworkError` with `UnityWebRequest.result`.
    - Updated error checking to use `if (uploadRequest.result != UnityWebRequest.Result.Success)`.
- Improved code readability by renaming variables.
    - Changed the variable name `www` to `uploadRequest` to avoid confusion with the old `WWW` class.
- Enhanced error messages for easier debugging.
    - Used `Debug.LogError` to display more detailed information when errors occur.
- Updated the Twitter posting URL from `http://` to `https://` to improve security.