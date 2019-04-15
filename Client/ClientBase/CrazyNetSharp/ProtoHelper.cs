using System;
using System.IO;
using ProtoBuf.Meta;
using BlackJack.LibClient.Protocol;
using System.Diagnostics;

namespace BlackJack.LibClient
{
    public static class ProtoHelper
    {
        /// <summary>
        /// Decodes the message.
        /// </summary>
        /// <returns>The message.</returns>
		/// <param name="dataBlock">Data block.</param>
		/// <param name="dataOffset">Data offset.</param>
		/// <param name="dataLen">Data length.</param>
		/// <param name="newMsg">New message.</param>
        /// <returns> 
        /// 	!= null     : A protocol message
        ///     == null     : No full message exist
        ///     Exception   : ProtoException to indicate HACK stream
        /// </returns>
        public static Object DecodeMessage(MessageBlock rCache, IProtoProvider protoProvider, out int msgId)
        {
            msgId = 0;
            // get the message head's length
            int headLength = sizeof(ushort) * 2;

            // Not enough data for header
            if (rCache.Length < headLength)
            {
                //Debug.WriteLine(String.Format("DecodeMessage Not enough data for header. rCache.Length={0} < headLength={1}", rCache.Length, headLength));
                return null;
            }

            // get message length
            ushort pakFullLength = rCache.ReadUInt16();

            // Bad stream
            if (pakFullLength < headLength)
                throw new ProtoException(string.Format("Hack stream, TotalLength={0}", pakFullLength));

            int pakBodyLength = pakFullLength - headLength;

            //Debug.WriteLine(String.Format("DecodeMessage Before rCache.Length={0} pakBodyLength={1} ", rCache.Length, pakBodyLength));

            // Not enough data for body
            // 注意这里需要考虑后面一个包头的长度
            if (rCache.Length < pakBodyLength + sizeof(ushort))
            {
                // Move read ptr to back
                // 此处只处理了UInt16长度的数据，所以回滚需要一致，不能使用headLength
                rCache.ReadPtr(-(sizeof(ushort)));
                //Debug.WriteLine(String.Format("DecodeMessage Not enough data for body rCache.Length={0} < pakBodyLength={1}", rCache.Length, pakBodyLength));
                return null;
            }

            // get message id field
            ushort pakMessageIdField = rCache.ReadUInt16();
            // get compressed tag
            bool isCompressed = pakMessageIdField >> (sizeof(ushort) * 8 - 1) == 1;
            // get the protocol id
            msgId = (ushort)(pakMessageIdField & 0x7FFF);

            // deserialize message, we should decompress message body data if needed
            Type pakType = protoProvider.GetTypeById(msgId);
            // Use ProtoBuf to deserialize message			
            object ccMsg = Activator.CreateInstance(pakType);

            //Debug.WriteLine(String.Format("DecodeMessage msgId={0} pakBodyLength={1} isCompressed={2} pakType={3}", msgId, pakBodyLength, isCompressed, pakType.ToString()));
            try
            {
                using (MemoryStream readStream = rCache.GetStream(pakBodyLength))
                {
                    //Debug.WriteLine(String.Format("DecodeMessage readStream={0}", printByteArray(readStream.ToArray())));
                    //RuntimeTypeModel.Default.Deserialize(readStream, ccMsg, pakType);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("msgId={0}, isCompressed={1} pakBodyLength={2}",
                    msgId, isCompressed, pakBodyLength), ex);
            }

            //Debug.WriteLine(String.Format("DecodeMessage rCacheInfo Length={0} WrPtr={1} RdPtr={2}", rCache.Length, rCache.WrPtr, rCache.RdPtr));
            return ccMsg;
        }

        ///// <summary>
        ///// 打印byte数组
        ///// </summary>
        ///// <param name="array"></param>
        ///// <returns></returns>
        //private static string printByteArray(byte[] array)
        //{
        //    if (array == null || array.Length == 0) return string.Empty;

        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    foreach (var c in array)
        //    {
        //        sb.AppendFormat("{0} ", c);
        //    }
        //    return sb.ToString();
        //}

        /// <summary>
        /// Encodes the message through protobuff, the message's format is : messageSize(UInt16)-ProtocolId(UInt16, including the compressTag and the protocolId value)-ProtocolData
        /// </summary>
        /// <returns>The ecoded message stream.</returns>
        /// <param name="vMsg">The message.</param>
        public static ArraySegment<byte> EncodeMessage(Object vMsg, IProtoProvider protoProvider)
        {
            byte[] fullBuf = new byte[(Int32)ProtoConst.MAX_PACKAGE_LENGTH];
            MemoryStream wrStream = new MemoryStream(fullBuf);
            BinaryWriter bnWriter = new BinaryWriter(wrStream);

            // get the message head's length
            int headLength = sizeof(ushort) * 2;

            // Total length, placehold only now
            bnWriter.Write((ushort)0);
            // Message ID, placehold only now
            bnWriter.Write((ushort)0);
            // write message body data to stream
            RuntimeTypeModel.Default.Serialize(wrStream, vMsg);

            // if the length of message body data is longer than a specific value, then we need compress the message body data
            bool needCompress = wrStream.Position - headLength >= ProtoConst.ZIP_BUFFER_MIN_LENGTH;
            ushort fullLength = 0;
            if (needCompress)
            {
                // allocate the buffer for keeping compressed data
                //using (MemoryStream zipMs = new MemoryStream())
                //{
                //    using (GZipStream compressionStream = new GZipStream(zipMs, CompressionMode.Compress))
                //    {
                //        compressionStream.Write(wrStream.ToArray(), headLength, (int)wrStream.Position - headLength);
                //    }
                //    wrStream.Seek(headLength, SeekOrigin.Begin);
                //    wrStream.Write(zipMs.ToArray(), 0, zipMs.ToArray().Length);
                //}

                using (MemoryStream zipMs = new MemoryStream(wrStream.ToArray(), headLength, (int)wrStream.Position - headLength))
                {
                    var compressedByte = QuickLZSharp.QuickLZ.compress(zipMs.ToArray(), 3);
                    wrStream.Seek(headLength, SeekOrigin.Begin);
                    wrStream.Write(compressedByte, 0, compressedByte.Length);
                }

                // Rewrite the message length
                fullLength = (ushort)wrStream.Position;
                bnWriter.Seek(0, SeekOrigin.Begin);
                bnWriter.Write(fullLength);
                // Rewrite the message ID info
                ushort protocolId = (ushort)(protoProvider.GetIdByType(vMsg.GetType()));
                protocolId = (ushort)(protocolId | 0x8000);
                bnWriter.Write(protocolId);
            }
            else
            {
                // Rewrite the message length
                fullLength = (ushort)wrStream.Position;
                bnWriter.Seek(0, SeekOrigin.Begin);
                bnWriter.Write(fullLength);
                // Rewrite the message ID info
                ushort protocolId = (ushort)(protoProvider.GetIdByType(vMsg.GetType()));
                protocolId = (ushort)(protocolId & 0x7FFF);
                bnWriter.Write(protocolId);
            }

            return
                new ArraySegment<byte>(fullBuf, 0, fullLength);
        }
    }

    /// <summary>
    /// 当接收到一条在ProtocolDic找不到的新协议时，将新协议的数据作为DefaultProtocolType类型进行Deserialize
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"DefaultProtocolType")]
    public partial class DefaultProtocolType : global::ProtoBuf.IExtensible
    {
        public DefaultProtocolType() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}

