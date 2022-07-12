namespace UniversalProfileSDK.Avatars
{
    public static class AvatarSDKUtils
    {
        public static string IpfsStringToIpfsURL(string ipfsUrl)
        {
            if(string.IsNullOrWhiteSpace(ipfsUrl) || !ipfsUrl.StartsWith("ipfs://"))
                return ipfsUrl;

            return ipfsUrl.Replace("ipfs://", AvatarSDKConfig.IpfsUrl);
        }
    }
}