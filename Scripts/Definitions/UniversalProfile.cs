using Newtonsoft.Json;
using UniversalProfileSDK.Avatars;

namespace UniversalProfileSDK
{
    [JsonObject]
    public class UniversalProfile
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
        
        [JsonProperty("description")]
        public string Description { get; private set; }
        
        [JsonProperty("avatar")]
        public UPAvatar[] Avatars { get; private set; }

        [JsonProperty("tags")]
        public string[] Tags { get; private set; }

        private UniversalProfile() { }

        public override string ToString()
        {
            return $"Universal Profile: {Name}";
        }
    }
}