namespace UniversalProfileSDK.Avatars
{
    public static class AvatarSDKConfig
    {
        /// <summary>
        /// Rpc url to use when getting profile data
        /// </summary>
        public static string RpcUrl = "https://rpc.l14.lukso.network";
        
        /// <summary>
        /// Ipfs url to use when downloading profiles and avatar bundles
        /// </summary>
        public static string IpfsUrl = "https://cloudflare-ipfs.com/ipfs/";
        
        /// <summary>
        /// Time before bundle download times out
        /// </summary>
        public static int BundleDownloadTimeoutSeconds = 120;
        
        /// <summary>
        /// Time before getting UniversalProfile times out
        /// </summary>
        public static int GetProfileTimeoutSeconds = 20;
    }
}