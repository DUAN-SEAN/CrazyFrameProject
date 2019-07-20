// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: BattleMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Crazy.Common {

  /// <summary>Holder for reflection information generated from BattleMessage.proto</summary>
  public static partial class BattleMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for BattleMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BattleMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNCYXR0bGVNZXNzYWdlLnByb3RvEgxDcmF6eS5Db21tb24iTAoYQzJTX0Jh",
            "dHRsZUNvbW1hbmRNZXNzYWdlEg0KBVJwY0lkGFogASgFEhAKCEJhdHRsZUlk",
            "GAEgASgEEg8KB01lc3NhZ2UYAiABKAkibgoZUzJDX0JvZHlJbml0QmF0dGxl",
            "TWVzc2FnZRINCgVScGNJZBhaIAEoBRIQCghCYXR0bGVJZBgBIAEoBBIQCghQ",
            "bGF5ZXJJZBgCIAEoCRIQCghCb2R5VHlwZRgDIAEoCRIMCgRCb2R5GAQgASgM",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.C2S_BattleCommandMessage), global::Crazy.Common.C2S_BattleCommandMessage.Parser, new[]{ "RpcId", "BattleId", "Message" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.S2C_BodyInitBattleMessage), global::Crazy.Common.S2C_BodyInitBattleMessage.Parser, new[]{ "RpcId", "BattleId", "PlayerId", "BodyType", "Body" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// 战斗指令
  /// </summary>
  public sealed partial class C2S_BattleCommandMessage : pb::IMessage<C2S_BattleCommandMessage> {
    private static readonly pb::MessageParser<C2S_BattleCommandMessage> _parser = new pb::MessageParser<C2S_BattleCommandMessage>(() => new C2S_BattleCommandMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C2S_BattleCommandMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.BattleMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_BattleCommandMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_BattleCommandMessage(C2S_BattleCommandMessage other) : this() {
      rpcId_ = other.rpcId_;
      battleId_ = other.battleId_;
      message_ = other.message_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_BattleCommandMessage Clone() {
      return new C2S_BattleCommandMessage(this);
    }

    /// <summary>Field number for the "RpcId" field.</summary>
    public const int RpcIdFieldNumber = 90;
    private int rpcId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RpcId {
      get { return rpcId_; }
      set {
        rpcId_ = value;
      }
    }

    /// <summary>Field number for the "BattleId" field.</summary>
    public const int BattleIdFieldNumber = 1;
    private ulong battleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong BattleId {
      get { return battleId_; }
      set {
        battleId_ = value;
      }
    }

    /// <summary>Field number for the "Message" field.</summary>
    public const int MessageFieldNumber = 2;
    private string message_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Message {
      get { return message_; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C2S_BattleCommandMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C2S_BattleCommandMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (BattleId != other.BattleId) return false;
      if (Message != other.Message) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (BattleId != 0UL) hash ^= BattleId.GetHashCode();
      if (Message.Length != 0) hash ^= Message.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (BattleId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(BattleId);
      }
      if (Message.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Message);
      }
      if (RpcId != 0) {
        output.WriteRawTag(208, 5);
        output.WriteInt32(RpcId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (RpcId != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(RpcId);
      }
      if (BattleId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(BattleId);
      }
      if (Message.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C2S_BattleCommandMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.BattleId != 0UL) {
        BattleId = other.BattleId;
      }
      if (other.Message.Length != 0) {
        Message = other.Message;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            BattleId = input.ReadUInt64();
            break;
          }
          case 18: {
            Message = input.ReadString();
            break;
          }
          case 720: {
            RpcId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///物体生成消息
  /// </summary>
  public sealed partial class S2C_BodyInitBattleMessage : pb::IMessage<S2C_BodyInitBattleMessage> {
    private static readonly pb::MessageParser<S2C_BodyInitBattleMessage> _parser = new pb::MessageParser<S2C_BodyInitBattleMessage>(() => new S2C_BodyInitBattleMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S2C_BodyInitBattleMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.BattleMessageReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_BodyInitBattleMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_BodyInitBattleMessage(S2C_BodyInitBattleMessage other) : this() {
      rpcId_ = other.rpcId_;
      battleId_ = other.battleId_;
      playerId_ = other.playerId_;
      bodyType_ = other.bodyType_;
      body_ = other.body_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_BodyInitBattleMessage Clone() {
      return new S2C_BodyInitBattleMessage(this);
    }

    /// <summary>Field number for the "RpcId" field.</summary>
    public const int RpcIdFieldNumber = 90;
    private int rpcId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RpcId {
      get { return rpcId_; }
      set {
        rpcId_ = value;
      }
    }

    /// <summary>Field number for the "BattleId" field.</summary>
    public const int BattleIdFieldNumber = 1;
    private ulong battleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong BattleId {
      get { return battleId_; }
      set {
        battleId_ = value;
      }
    }

    /// <summary>Field number for the "PlayerId" field.</summary>
    public const int PlayerIdFieldNumber = 2;
    private string playerId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlayerId {
      get { return playerId_; }
      set {
        playerId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "BodyType" field.</summary>
    public const int BodyTypeFieldNumber = 3;
    private string bodyType_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string BodyType {
      get { return bodyType_; }
      set {
        bodyType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Body" field.</summary>
    public const int BodyFieldNumber = 4;
    private pb::ByteString body_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Body {
      get { return body_; }
      set {
        body_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S2C_BodyInitBattleMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S2C_BodyInitBattleMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (BattleId != other.BattleId) return false;
      if (PlayerId != other.PlayerId) return false;
      if (BodyType != other.BodyType) return false;
      if (Body != other.Body) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (BattleId != 0UL) hash ^= BattleId.GetHashCode();
      if (PlayerId.Length != 0) hash ^= PlayerId.GetHashCode();
      if (BodyType.Length != 0) hash ^= BodyType.GetHashCode();
      if (Body.Length != 0) hash ^= Body.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (BattleId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(BattleId);
      }
      if (PlayerId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(PlayerId);
      }
      if (BodyType.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(BodyType);
      }
      if (Body.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Body);
      }
      if (RpcId != 0) {
        output.WriteRawTag(208, 5);
        output.WriteInt32(RpcId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (RpcId != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(RpcId);
      }
      if (BattleId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(BattleId);
      }
      if (PlayerId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PlayerId);
      }
      if (BodyType.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BodyType);
      }
      if (Body.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Body);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S2C_BodyInitBattleMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.BattleId != 0UL) {
        BattleId = other.BattleId;
      }
      if (other.PlayerId.Length != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.BodyType.Length != 0) {
        BodyType = other.BodyType;
      }
      if (other.Body.Length != 0) {
        Body = other.Body;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            BattleId = input.ReadUInt64();
            break;
          }
          case 18: {
            PlayerId = input.ReadString();
            break;
          }
          case 26: {
            BodyType = input.ReadString();
            break;
          }
          case 34: {
            Body = input.ReadBytes();
            break;
          }
          case 720: {
            RpcId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
