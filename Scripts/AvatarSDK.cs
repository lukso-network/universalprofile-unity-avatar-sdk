using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.UnityClient;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UniversalProfileSDK.Contracts;

namespace UniversalProfileSDK.Avatars
{
    public static class AvatarSDK
    {
        static readonly byte[] Lsp3bytes = "0x5ef83ad9559033e6e941db7d7c495acdce616347d28e90c7ce47cbfcfcad3bc5".HexToByteArray();

        /// <summary>
        /// Get and load remote UniversalProfile coroutine
        /// </summary>
        /// <param name="address">Profile address</param>
        /// <param name="onSuccess">Delegate to run if profile is successfully loaded</param>
        /// <param name="onFail">Delegate to run if profile loading fails</param>
        /// <returns></returns>
        public static IEnumerator GetProfileRemote(string address, AvatarSDKDelegates.GetProfileSuccess onSuccess, AvatarSDKDelegates.GetProfileFailed onFail)
        {
            var queryRequest = new QueryUnityRequest<GetDataFunction, GetDataOutputDTO>(AvatarSDKConfig.RpcUrl, null);
            yield return queryRequest.Query(new GetDataFunction { Keys = new List<byte[]> { Lsp3bytes } }, address);

            if(queryRequest.Result?.Values == null)
            {
                onFail?.Invoke(new NullResponseException("Invalid response. Is your UP address valid?"));
                yield break;
            }

            byte[] resultBytes = queryRequest.Result.Values[0];
            string result = Encoding.UTF8.GetString(resultBytes);
            
            string ipfsString = result.Substring(41);

            using UnityWebRequest www = UnityWebRequest.Get(AvatarSDKConfig.IpfsUrl + ipfsString);
            www.timeout = AvatarSDKConfig.GetProfileTimeoutSeconds;
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError || www.result == UnityWebRequest.Result.ConnectionError)
            {
                onFail?.Invoke(new Exception(www.error)); //TODO: Custom exception
                yield break;
            }
            
            string json = www.downloadHandler.text;
            yield return LoadProfileFromJson(json, onSuccess, onFail);
        }
        
        /// <summary>
        /// Load UniversalProfile from local LSP3 json coroutine
        /// </summary>
        /// <param name="filepath">Local filepath of a UniversalProfile LSP3 json</param>
        /// <param name="onSuccess">Delegate to run if profile is successfully loaded</param>
        /// <param name="onFail">Delegate to run if profile loading fails</param>
        /// <returns></returns>
        public static IEnumerator GetProfileLocal(string filepath, AvatarSDKDelegates.GetProfileSuccess onSuccess, AvatarSDKDelegates.GetProfileFailed onFail)
        {
            if(!File.Exists(filepath))
            {
                onFail.Invoke(new FileNotFoundException($"{filepath} does not exist"));
                yield break;
            }

            string json = File.ReadAllText(filepath);
            yield return LoadProfileFromJson(json, onSuccess, onFail);
        }

        /// <summary>
        /// Load a UniversalProfile from a JSON string
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="onSuccess">Delegate to run if profile is successfully loaded</param>
        /// <param name="onFail">Delegate to run if profile loading fails</param>
        /// <returns></returns>
        public static IEnumerator LoadProfileFromJson(string json, AvatarSDKDelegates.GetProfileSuccess onSuccess, AvatarSDKDelegates.GetProfileFailed onFail)
        {
            if(string.IsNullOrWhiteSpace(json))
            {
                onFail?.Invoke(new JsonException("Empty json received"));
                yield break;
            }

            UniversalProfile profile = null;
            try
            {
                //TODO: Write a custom json converter to grab the nested UP
                var lsp3 = JsonConvert.DeserializeObject<Dictionary<string, UniversalProfile>>(json);
                profile = lsp3?["LSP3Profile"];
                if(profile == null)
                {
                    onFail?.Invoke(new JsonException("Deserialized Universal Profile is null"));
                    yield break;
                }
            }
            catch(Exception ex)
            {
                onFail?.Invoke(ex);
            }
            onSuccess?.Invoke(profile);
        }
    }
}