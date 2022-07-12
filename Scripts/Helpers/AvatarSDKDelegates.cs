using System;
using UnityEngine;

namespace UniversalProfileSDK.Avatars
{
    /// <summary>
    /// Shared delegates used in the SDK.
    /// </summary>
    public static class AvatarSDKDelegates
    {
        /// <summary>
        /// Delegate to run when download progress changes.
        /// </summary>
        public delegate void ProgressChangedDelegate(float percent);

        /// <summary>
        /// Delegate to run on successful profile load. It's parameter being the loaded profile.
        /// </summary>
        public delegate void GetProfileSuccess(UniversalProfile profile);
        
        /// <summary>
        /// Delegate to run on failed profile load. It's parameter being the exception raised.
        /// </summary>
        public delegate void GetProfileFailed(Exception exception);
        
        /// <summary>
        /// Delegate to run on successful AssetBundle load. It's parameter being the loaded bundle.
        /// </summary>
        public delegate void AvatarBundleLoadCompleted(AssetBundle loadedBundle);
        
        /// <summary>
        /// Delegate to run on failed AssetBundle load. It's parameter being the exception raised.
        /// </summary>
        public delegate void AvatarBundleLoadFailed(Exception exception);
    }
}