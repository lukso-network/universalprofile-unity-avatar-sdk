using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace UniversalProfileSDK.Contracts
{
    [Function("getData", "bytes[]")]
    public class GetDataFunction : FunctionMessage
    {
        [Parameter("bytes32[]", "keys", 1)]
        public List<byte[]> Keys { get; set; }
    }
    
    [FunctionOutput]
    public class GetDataOutputDTO : IFunctionOutputDTO 
    {
        [Parameter("bytes[]", "values", 1)]
        public List<byte[]> Values { get; set; }
    }
}