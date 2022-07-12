namespace UniversalProfileSDK.Avatars
{
    public static class AvatarSDKUtils
    {
        /// <summary>
        /// Replaces ipfs:// with the full ipfs url as defined in AvatarSDKConfig.IpfsUrl
        /// </summary>
        /// <param name="ipfsUrl">Ipfs url string to replace</param>
        /// <returns>Full ipfs url</returns>
        public static string IpfsStringToFullIpfsURL(string ipfsUrl)
        {
            if(string.IsNullOrWhiteSpace(ipfsUrl) || !ipfsUrl.StartsWith("ipfs://"))
                return ipfsUrl;

            return ipfsUrl.Replace("ipfs://", AvatarSDKConfig.IpfsUrl);
        }
    }
}