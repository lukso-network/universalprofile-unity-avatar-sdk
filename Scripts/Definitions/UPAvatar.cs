using System;
using Newtonsoft.Json;

namespace UniversalProfileSDK.Avatars
{
    [JsonObject]
    public class UPAvatar
    {
        [JsonProperty("hashFunction")]
        public string HashFunction { get; private set; }
        
        [JsonProperty("hash")]
        public string Hash { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("fileType")]
        public string FileType { get; private set; }

        public UPAvatar(string hash, string hashFunction, string url)
        {
            Hash = hash;
            HashFunction = hashFunction;
            Url = url;
        }

        /// <summary>
        /// Compares two UPAvatars by the hash property
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the hashes are the same</returns>
        public bool Equals(UPAvatar other)
        {
            return Hash.Equals(other.Hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}