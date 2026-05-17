using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (Payload == null) return Type.Name;
            return $"{Type.Name}<{Payload}>";
        }

        public override bool Equals(object obj)
        {
            if (obj is SamplerType other)
            {
                if (!base.Equals(other)) return false;
                if (Payload == null && other.Payload == null) return true;
                if (Payload == null || other.Payload == null) return false;
                return Payload.Equals(other.Payload);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
