using System;
using UnityEngine;
using UniversalProfileSDK.Avatars;

namespace UniversalProfileSDK
{
    public static class UPAvatarExtensions
    {
        /// <summary>
        /// Gets the platform from an UPAvatar's FileType field. Attempts to parse it to a unity RuntimePlatform.
        /// </summary>
        /// <param name="upAvatar">UPAvatar to get platform of</param>
        /// <returns>RuntimePlatform or null if parsing failed</returns>
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
        
        /// <summary>
        /// Checks whether UPAvatar's platform matches our current platform.
        /// </summary>
        /// <param name="upAvatar">UPAvatar to check</param>
        /// <returns>True if UPAvatar's platform matches our current one</returns>
        public static bool UPAvatarIsForCurrentPlatform(this UPAvatar upAvatar)
        {
            RuntimePlatform? platform = GetPlatformFromUPAvatar(upAvatar);
            if(platform == null)
                return false;
            
            return Application.platform == platform;
        }
    }
}