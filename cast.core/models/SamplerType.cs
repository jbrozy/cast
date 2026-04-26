using System.Collections.Generic;
using cast.core.models.symbols;

namespace cast.core.models
{
    public class SamplerType : CastType
    {
        public CastType? Payload { get; }
        
        public SamplerType(TypeSymbol baseType, CastType? payloadType = null) : base(baseType)
        {
            Payload = payloadType;
        }

        public override string ToString()
        {
            return Payload == null ? $"{Type.Name}" : $"{Type.Name}<{Payload.Type}>";
        }
    }
}