using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniversalProfileSDK.Avatars
{
    public static class AvatarCache
    {
        public delegate void AvatarLoadSuccessDelegate(GameObject avatar);
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

        public static IEnumerator LoadAvatar(UPAvatar upAvatar, AvatarLoadSuccessDelegate onSuccess, AvatarLoadErrorDelegate onFailed, AvatarSDKDelegates.ProgressChangedDelegate onProgressChanged = null)
        {
            CachedAvatar cachedAvatar = Cache.FirstOrDefault(av => av.UPAvatar.Equals(upAvatar));

            AssetBundle avatarBundle = null;
            Exception loaderException = null;
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

            Cache.Add(new CachedAvatar(upAvatar, avatarBundle));
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