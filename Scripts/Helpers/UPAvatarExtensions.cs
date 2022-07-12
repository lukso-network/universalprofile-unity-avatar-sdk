using System;
using UnityEngine;
using UniversalProfileSDK.Avatars;

namespace UniversalProfileSDK
{
    public static class UPAvatarExtensions
    {
        public static RuntimePlatform? GetPlatformFromUPAvatar(this UPAvatar upAvatar)
        {
            string fileType = upAvatar.FileType;
            string startString = "assetbundle/";
            if(!fileType.StartsWith(startString))
                return null;

            string platformName = fileType.Substring(startString.Length);

            if(Application.isEditor)
            {
                if(!platformName.EndsWith("editor", StringComparison.OrdinalIgnoreCase))
                    platformName += "editor";
            }
            else
            {
                if(!platformName.EndsWith("player", StringComparison.OrdinalIgnoreCase))
                    platformName += "player";
            }

            if(Enum.TryParse<RuntimePlatform>(platformName, true, out var platform))
                return platform;
            return null;
        }
        
        public static bool UPAvatarIsForCurrentPlatform(this UPAvatar upAvatar)
        {
            RuntimePlatform? platform = GetPlatformFromUPAvatar(upAvatar);
            if(platform == null)
                return false;
            
            return Application.platform == platform;
        }
    }
}