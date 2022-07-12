using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace UniversalProfileSDK.Avatars
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteAvatarBundleLoader : AvatarBundleLoaderBase
    {
        readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        public RemoteAvatarBundleLoader(UPAvatar avatar) : base(avatar)
        {
            
        }
        
        /// <summary>
        /// Load avatar bundle coroutine
        /// </summary>
        /// <param name="onLoaded">Delegate to run on successful load</param>
        /// <param name="onFailed">Delegate to run on failed load</param>
        /// <param name="onProgressChanged">Delegate to run every fixed update step to, for example, update an UI progress bar somewhere</param>
        /// <returns>IEnumerator used for coroutines</returns>
        public override IEnumerator LoadAvatarBundle(AvatarSDKDelegates.AvatarBundleLoadCompleted onLoaded, AvatarSDKDelegates.AvatarBundleLoadFailed onFailed, AvatarSDKDelegates.ProgressChangedDelegate onProgressChanged)
        {
            string url = AvatarSDKUtils.IpfsStringToFullIpfsURL(UPAvatar.Url);
            var bundleHandler = new DownloadHandlerAssetBundle(url, 0)
            {
                autoLoadAssetBundle = false
            };

            using UnityWebRequest www = new UnityWebRequest(url, "GET", bundleHandler, null);
            www.timeout = AvatarSDKConfig.BundleDownloadTimeoutSeconds;

            UnityWebRequestAsyncOperation op = www.SendWebRequest();
            float lastPercent = op.progress;
            
            do
            {
                var result = op.webRequest.result;
                switch(result) //TODO: More detailed error handling
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.ProtocolError:
                    case UnityWebRequest.Result.DataProcessingError:
                        onFailed?.Invoke(new Exception($"{result}: Failed to download avatar {UPAvatar.Hash}"));
                        yield break;
                }
                
                onProgressChanged?.Invoke(op.progress);
                yield return waitForFixedUpdate;
            } 
            while(!op.isDone);

            if(bundleHandler.assetBundle is null)
            {
                onFailed?.Invoke(new Exception("Request succeeded but didn't return any data."));
                yield break;
            }

            onLoaded?.Invoke(bundleHandler.assetBundle);
        }
    }
}