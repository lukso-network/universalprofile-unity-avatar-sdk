using System;
using UnityEngine;

namespace UniversalProfileSDK.Avatars
{
    public static class AvatarSDKDelegates
    {
        public delegate void ProgressChangedDelegate(float percent);

        public delegate void GetProfileSuccess(UniversalProfile profile);
        public delegate void GetProfileFailed(Exception exception);
        
        public delegate void AvatarBundleLoadCompleted(AssetBundle loadedBundle);
        public delegate void AvatarBundleLoadFailed(Exception exception);
    }
}