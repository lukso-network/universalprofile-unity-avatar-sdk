using System.Collections;

namespace UniversalProfileSDK.Avatars
{
    /// <summary>
    /// Base class for avatar bundle loaders
    /// </summary>
    public abstract class AvatarBundleLoaderBase
    {
        protected UPAvatar UPAvatar { get; set; }

        protected AvatarBundleLoaderBase(UPAvatar upAvatar)
        {
            UPAvatar = upAvatar;
        }

        public abstract IEnumerator LoadAvatarBundle(AvatarSDKDelegates.AvatarBundleLoadCompleted onLoaded, AvatarSDKDelegates.AvatarBundleLoadFailed onFailed, AvatarSDKDelegates.ProgressChangedDelegate onProgressChanged);
    }
}