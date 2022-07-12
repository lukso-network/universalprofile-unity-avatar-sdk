using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniversalProfileSDK.Avatars
{
    /// <summary>
    /// Simple avatar cache storing references of UPAvatars to AssetBundles
    /// </summary>
    public static class AvatarCache
    {
        /// <summary>
        /// Runs after avatar was successfully loaded, with the loaded avatar GameObject as it's parameter
        /// </summary>
        public delegate void AvatarLoadSuccessDelegate(GameObject avatar);
        
        /// <summary>
        /// Runs on avatar download failure with the exception that occured as it's parameter 
        /// </summary>
        public delegate void AvatarLoadErrorDelegate(Exception exception);

        static List<CachedAvatar> Cache = new List<CachedAvatar>();
        static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        
        class CachedAvatar
        {
            public AssetBundle ContainingBundle;
            public UPAvatar UPAvatar;

            public CachedAvatar(UPAvatar upAvatar, AssetBundle bundle)
            {
                UPAvatar = upAvatar;
                ContainingBundle = bundle;
            }
        }

        /// <summary>
        /// Coroutine to download avatar AssetBundle from UPAvatar and load it.
        /// </summary>
        /// <param name="upAvatar">UPAvatar to download AssetBundle from</param>
        /// <param name="onSuccess">Delegate to run on successful download</param>
        /// <param name="onFailed">Delegate to run on failed download</param>
        /// <param name="onProgressChanged">Delegate to run every fixed update step to, for example, update a progress bar in the UI somewhere</param>
        /// <returns>IEnumerator for coroutines</returns>
        public static IEnumerator LoadAvatar(UPAvatar upAvatar, AvatarLoadSuccessDelegate onSuccess, AvatarLoadErrorDelegate onFailed, AvatarSDKDelegates.ProgressChangedDelegate onProgressChanged = null)
        {
            AssetBundle avatarBundle = null;
            Exception loaderException = null;
            
            // Get avatar from cache or download and cache it
            CachedAvatar cachedAvatar = Cache.FirstOrDefault(av => av.UPAvatar.Equals(upAvatar));
            if(cachedAvatar != null)
            {
                avatarBundle = cachedAvatar.ContainingBundle;
            }
            else
            {
                RemoteAvatarBundleLoader loader = new RemoteAvatarBundleLoader(upAvatar);
                yield return loader.LoadAvatarBundle(
                (bundle) =>
                {
                    avatarBundle = bundle;
                    Cache.Add(new CachedAvatar(upAvatar, avatarBundle));
                },
                (exception) =>
                {
                    loaderException = exception;
                },
                (percent) =>
                {
                    onProgressChanged?.Invoke(percent);
                });
            }

            if(loaderException != null)
            {
                onFailed?.Invoke(loaderException);
                yield break;
            }
            
            // Load the avatar from it's bundle
            AssetBundleRequest assetBundleRequest = avatarBundle.LoadAssetAsync<GameObject>("_CustomAvatar");
            while(!assetBundleRequest.isDone)
            {
                yield return WaitForFixedUpdate;
                onProgressChanged?.Invoke(assetBundleRequest.progress);
            }

            onSuccess?.Invoke(assetBundleRequest.asset as GameObject);
        }
    }
}