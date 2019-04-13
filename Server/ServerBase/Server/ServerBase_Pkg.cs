using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.ServerBase
{
    /// <summary>
    /// ServerBase Package 
    /// 所有关于消息解析封装的方法都在这
    /// </summary>
    public partial class ServerBase
    {
        /// <summary>
        /// 打包并压缩单个包
        /// </summary>
        /// <param name="packageObj"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferOffest"></param>
        /// <returns></returns>
        private bool PackProtobufObjectInternal(object packageObj, ClientOutputBuffer buffer, int bufferOffest = 0)
        {
            System.Diagnostics.Debug.Assert(packageObj != null, "PackProtobufObjectInternal error: packageObj is null");
            System.Diagnostics.Debug.Assert(buffer != null, "PackProtobufObjectInternal error: buffer is null");

            // 包头的长度
            const int headLen = sizeof(ushort) * 2;
            // 整个包的长度
            ushort pkgLen;
            var dataOffset = bufferOffest;
            bool needCompress;
            try
            {
                // 先将协议的内容写入到缓冲里
                dataOffset += headLen;
                //int objDataStartIndex = dataOffset; // 记录在当前ms中有效数据起始index
                using (var stream = new MemoryStream(buffer.m_buffer,
                    dataOffset,
                    buffer.m_buffer.Length - dataOffset,
                    true, true))
                {
                    // 先序列化协议包
                    m_messagePraser.SerializeTo(packageObj, stream);
                    // 包长度 = 协议内容长度 + 包头长度
                    pkgLen = (ushort)(stream.Position + headLen);
                    dataOffset += (int)stream.Position;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }

            // 再将包头的内容写入到缓冲里，包头内容包括是否压缩的标志位、包长度和协议id，包长度在这里才能计算出来
            using (var headerStream = new MemoryStream(buffer.m_buffer, bufferOffest, headLen))
            {
                using (var writer = new BinaryWriter(headerStream))
                {
                    // 获取协议id
                    ushort protocolId = (ushort)(OpcodeTypeDic.GetIdByType(packageObj.GetType()));
                    // 将协议id的最高位修改为压缩标记位
                    ushort finalyProtocolId = (ushort)protocolId;
                    writer.Write(pkgLen);
                    writer.Write(finalyProtocolId);

                    // buff最后长度
                    buffer.m_dataLen = dataOffset;
                }
            }

            return true;
        }

        /// <summary>
        /// 对数据buff进行解包处理
        /// </summary>
        /// <param name="orgDataBuff">原始数据包</param>
        /// <param name="totalDataAvailable">数据包中一共有多少数据可用</param>
        /// <param name="dataOffset">当前读取到的位置</param>
        /// <param name="msgType">反序列化后的包类型</param>
        /// <param name="deserializeObject">反序列化后的数据类型</param>
        /// <param name="deserializeBuff">无压缩的序列化二进制数据</param>
        /// <returns>已经处理的数据长度</returns>
        public int UnpackProtobufObject(byte[] orgDataBuff, int totalDataAvailable, int dataOffset, out Type msgType, out object deserializeObject, out MemoryStream deserializeBuff)
        {
            System.Diagnostics.Debug.Assert(
                orgDataBuff != null && orgDataBuff.Length != 0 && orgDataBuff.Length >= totalDataAvailable &&
                dataOffset <= totalDataAvailable, "UnPackProtobufObject error: dataBuff is null or empty!");

            var startDataOffset = dataOffset;
            msgType = null;
            deserializeObject = null;
            deserializeBuff = null;
            const int uint16Length = sizeof(ushort);
            var dataLength = totalDataAvailable - dataOffset;

            // 读取完整包的长度
            var msgFullLength = BitConverter.ToUInt16(orgDataBuff, dataOffset);
            // 包为半包或者不足包头长度uint16Length*2
            if (dataLength < msgFullLength || dataLength < uint16Length * 2)
                return 0;

            // 包足够一个完整包
            dataOffset += uint16Length;

            // 消息ID字段，从该字段获取到压缩标志位以及消息ID信息
            var msgIdField = BitConverter.ToUInt16(orgDataBuff, dataOffset);
            // 获取消息的ID
            var msgId = (ushort)msgIdField ;
            dataOffset += uint16Length;

            // 消息运行时类型
            try
            {
                //该调用失败时抛出异常，因此不需判断结果
                msgType = OpcodeTypeDic.GetTypeById(msgId);
                // 反序列化消息，如果数据是压缩的，就必须先解压缩，再反序列化
                deserializeObject = Activator.CreateInstance(msgType);
            }
            catch (Exception)
            {
                Log.Error($"WRONG MSG ID ==============================={msgId}");
                throw;
            }

            // 获取协议内容的长度
            var protoLength = msgFullLength - uint16Length * 2;
         
            deserializeBuff = new MemoryStream(orgDataBuff, dataOffset, protoLength);

            deserializeObject =  m_messagePraser.DeserializeFrom(deserializeObject, deserializeBuff);
       
            dataOffset += protoLength;

            return dataOffset - startDataOffset;
        }
    }
}
