using System;

namespace BlackJack.LibClient.Protocol
{
    public interface IProtoProvider
    {
        /// Query message type by message id
        Type GetTypeById(Int32 vId);

        /// Query message id by message type
        Int32 GetIdByType(Type vType);
    }
}
