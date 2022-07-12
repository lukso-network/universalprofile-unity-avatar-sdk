using System.Collections;

namespace UniversalProfileSDK.Avatars
{
    public abstract class AvatarBundleLoaderBase
    {


        protected UPAvatar UPAvatar { get; set; }

        public AvatarBundleLoaderBase(UPAvatar upAvatar)
        {
            UPAvatar = upAvatar;
        }

        public abstract IEnumerator LoadAvatarBundle(AvatarSDKDelegates.AvatarBundleLoadCompleted onLoaded, AvatarSDKDelegates.AvatarBundleLoadFailed onFailed, AvatarSDKDelegates.ProgressChangedDelegate onProgressChanged);
    }
}