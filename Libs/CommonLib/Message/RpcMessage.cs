// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: RpcMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Crazy.Common {

  /// <summary>Holder for reflection information generated from RpcMessage.proto</summary>
  public static partial class RpcMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for RpcMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RpcMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChBScGNNZXNzYWdlLnByb3RvEgxDcmF6eS5Db21tb24iRAoQQzJTX0xvZ2lu",
            "TWVzc2FnZRINCgVScGNJZBhaIAEoBRIPCgdhY2NvdW50GAEgASgJEhAKCHBh",
            "c3N3b3JkGAIgASgJIqcBChBTMkNfTG9naW5NZXNzYWdlEg0KBVJwY0lkGFog",
            "ASgFEg0KBUVycm9yGFsgASgFEg8KB01lc3NhZ2UYXCABKAkSFAoMcGxheWVy",
            "R2FtZUlkGAEgASgJEjMKBXN0YXRlGAIgASgOMiQuQ3JhenkuQ29tbW9uLlMy",
            "Q19Mb2dpbk1lc3NhZ2UuU3RhdGUiGQoFU3RhdGUSCAoERmFpbBAAEgYKAk9L",
            "EAEiRwoTQzJTX1JlZ2lzdGVyTWVzc2FnZRINCgVScGNJZBhaIAEoBRIPCgdh",
            "Y2NvdW50GAEgASgJEhAKCHBhc3N3b3JkGAIgASgJIpcBChNTMkNfUmVnaXN0",
            "ZXJNZXNzYWdlEg0KBVJwY0lkGFogASgFEg0KBUVycm9yGFsgASgFEg8KB01l",
            "c3NhZ2UYXCABKAkSNgoFc3RhdGUYAiABKA4yJy5DcmF6eS5Db21tb24uUzJD",
            "X1JlZ2lzdGVyTWVzc2FnZS5TdGF0ZSIZCgVTdGF0ZRIICgRGYWlsEAASBgoC",
            "T0sQAWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.C2S_LoginMessage), global::Crazy.Common.C2S_LoginMessage.Parser, new[]{ "RpcId", "Account", "Password" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.S2C_LoginMessage), global::Crazy.Common.S2C_LoginMessage.Parser, new[]{ "RpcId", "Error", "Message", "PlayerGameId", "State" }, null, new[]{ typeof(global::Crazy.Common.S2C_LoginMessage.Types.State) }, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.C2S_RegisterMessage), global::Crazy.Common.C2S_RegisterMessage.Parser, new[]{ "RpcId", "Account", "Password" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Crazy.Common.S2C_RegisterMessage), global::Crazy.Common.S2C_RegisterMessage.Parser, new[]{ "RpcId", "Error", "Message", "State" }, null, new[]{ typeof(global::Crazy.Common.S2C_RegisterMessage.Types.State) }, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /////��֤��½��Ϣ
  /// </summary>
  public sealed partial class C2S_LoginMessage : pb::IMessage<C2S_LoginMessage> {
    private static readonly pb::MessageParser<C2S_LoginMessage> _parser = new pb::MessageParser<C2S_LoginMessage>(() => new C2S_LoginMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C2S_LoginMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.RpcMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_LoginMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_LoginMessage(C2S_LoginMessage other) : this() {
      rpcId_ = other.rpcId_;
      account_ = other.account_;
      password_ = other.password_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_LoginMessage Clone() {
      return new C2S_LoginMessage(this);
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

    /// <summary>Field number for the "account" field.</summary>
    public const int AccountFieldNumber = 1;
    private string account_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Account {
      get { return account_; }
      set {
        account_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "password" field.</summary>
    public const int PasswordFieldNumber = 2;
    private string password_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Password {
      get { return password_; }
      set {
        password_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C2S_LoginMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C2S_LoginMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (Account != other.Account) return false;
      if (Password != other.Password) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (Account.Length != 0) hash ^= Account.GetHashCode();
      if (Password.Length != 0) hash ^= Password.GetHashCode();
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
      if (Account.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Account);
      }
      if (Password.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Password);
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
      if (Account.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Account);
      }
      if (Password.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Password);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C2S_LoginMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.Account.Length != 0) {
        Account = other.Account;
      }
      if (other.Password.Length != 0) {
        Password = other.Password;
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
          case 10: {
            Account = input.ReadString();
            break;
          }
          case 18: {
            Password = input.ReadString();
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

  public sealed partial class S2C_LoginMessage : pb::IMessage<S2C_LoginMessage> {
    private static readonly pb::MessageParser<S2C_LoginMessage> _parser = new pb::MessageParser<S2C_LoginMessage>(() => new S2C_LoginMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S2C_LoginMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.RpcMessageReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_LoginMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_LoginMessage(S2C_LoginMessage other) : this() {
      rpcId_ = other.rpcId_;
      error_ = other.error_;
      message_ = other.message_;
      playerGameId_ = other.playerGameId_;
      state_ = other.state_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_LoginMessage Clone() {
      return new S2C_LoginMessage(this);
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

    /// <summary>Field number for the "Error" field.</summary>
    public const int ErrorFieldNumber = 91;
    private int error_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Error {
      get { return error_; }
      set {
        error_ = value;
      }
    }

    /// <summary>Field number for the "Message" field.</summary>
    public const int MessageFieldNumber = 92;
    private string message_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Message {
      get { return message_; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "playerGameId" field.</summary>
    public const int PlayerGameIdFieldNumber = 1;
    private string playerGameId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlayerGameId {
      get { return playerGameId_; }
      set {
        playerGameId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "state" field.</summary>
    public const int StateFieldNumber = 2;
    private global::Crazy.Common.S2C_LoginMessage.Types.State state_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Crazy.Common.S2C_LoginMessage.Types.State State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S2C_LoginMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S2C_LoginMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (Error != other.Error) return false;
      if (Message != other.Message) return false;
      if (PlayerGameId != other.PlayerGameId) return false;
      if (State != other.State) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (Error != 0) hash ^= Error.GetHashCode();
      if (Message.Length != 0) hash ^= Message.GetHashCode();
      if (PlayerGameId.Length != 0) hash ^= PlayerGameId.GetHashCode();
      if (State != 0) hash ^= State.GetHashCode();
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
      if (PlayerGameId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(PlayerGameId);
      }
      if (State != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) State);
      }
      if (RpcId != 0) {
        output.WriteRawTag(208, 5);
        output.WriteInt32(RpcId);
      }
      if (Error != 0) {
        output.WriteRawTag(216, 5);
        output.WriteInt32(Error);
      }
      if (Message.Length != 0) {
        output.WriteRawTag(226, 5);
        output.WriteString(Message);
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
      if (Error != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(Error);
      }
      if (Message.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (PlayerGameId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PlayerGameId);
      }
      if (State != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) State);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S2C_LoginMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.Error != 0) {
        Error = other.Error;
      }
      if (other.Message.Length != 0) {
        Message = other.Message;
      }
      if (other.PlayerGameId.Length != 0) {
        PlayerGameId = other.PlayerGameId;
      }
      if (other.State != 0) {
        State = other.State;
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
          case 10: {
            PlayerGameId = input.ReadString();
            break;
          }
          case 16: {
            state_ = (global::Crazy.Common.S2C_LoginMessage.Types.State) input.ReadEnum();
            break;
          }
          case 720: {
            RpcId = input.ReadInt32();
            break;
          }
          case 728: {
            Error = input.ReadInt32();
            break;
          }
          case 738: {
            Message = input.ReadString();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the S2C_LoginMessage message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum State {
        [pbr::OriginalName("Fail")] Fail = 0,
        [pbr::OriginalName("OK")] Ok = 1,
      }

    }
    #endregion

  }

  public sealed partial class C2S_RegisterMessage : pb::IMessage<C2S_RegisterMessage> {
    private static readonly pb::MessageParser<C2S_RegisterMessage> _parser = new pb::MessageParser<C2S_RegisterMessage>(() => new C2S_RegisterMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C2S_RegisterMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.RpcMessageReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_RegisterMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_RegisterMessage(C2S_RegisterMessage other) : this() {
      rpcId_ = other.rpcId_;
      account_ = other.account_;
      password_ = other.password_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C2S_RegisterMessage Clone() {
      return new C2S_RegisterMessage(this);
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

    /// <summary>Field number for the "account" field.</summary>
    public const int AccountFieldNumber = 1;
    private string account_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Account {
      get { return account_; }
      set {
        account_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "password" field.</summary>
    public const int PasswordFieldNumber = 2;
    private string password_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Password {
      get { return password_; }
      set {
        password_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C2S_RegisterMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C2S_RegisterMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (Account != other.Account) return false;
      if (Password != other.Password) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (Account.Length != 0) hash ^= Account.GetHashCode();
      if (Password.Length != 0) hash ^= Password.GetHashCode();
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
      if (Account.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Account);
      }
      if (Password.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Password);
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
      if (Account.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Account);
      }
      if (Password.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Password);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C2S_RegisterMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.Account.Length != 0) {
        Account = other.Account;
      }
      if (other.Password.Length != 0) {
        Password = other.Password;
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
          case 10: {
            Account = input.ReadString();
            break;
          }
          case 18: {
            Password = input.ReadString();
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

  public sealed partial class S2C_RegisterMessage : pb::IMessage<S2C_RegisterMessage> {
    private static readonly pb::MessageParser<S2C_RegisterMessage> _parser = new pb::MessageParser<S2C_RegisterMessage>(() => new S2C_RegisterMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S2C_RegisterMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Crazy.Common.RpcMessageReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_RegisterMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_RegisterMessage(S2C_RegisterMessage other) : this() {
      rpcId_ = other.rpcId_;
      error_ = other.error_;
      message_ = other.message_;
      state_ = other.state_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S2C_RegisterMessage Clone() {
      return new S2C_RegisterMessage(this);
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

    /// <summary>Field number for the "Error" field.</summary>
    public const int ErrorFieldNumber = 91;
    private int error_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Error {
      get { return error_; }
      set {
        error_ = value;
      }
    }

    /// <summary>Field number for the "Message" field.</summary>
    public const int MessageFieldNumber = 92;
    private string message_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Message {
      get { return message_; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "state" field.</summary>
    public const int StateFieldNumber = 2;
    private global::Crazy.Common.S2C_RegisterMessage.Types.State state_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Crazy.Common.S2C_RegisterMessage.Types.State State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S2C_RegisterMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S2C_RegisterMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RpcId != other.RpcId) return false;
      if (Error != other.Error) return false;
      if (Message != other.Message) return false;
      if (State != other.State) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RpcId != 0) hash ^= RpcId.GetHashCode();
      if (Error != 0) hash ^= Error.GetHashCode();
      if (Message.Length != 0) hash ^= Message.GetHashCode();
      if (State != 0) hash ^= State.GetHashCode();
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
      if (State != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) State);
      }
      if (RpcId != 0) {
        output.WriteRawTag(208, 5);
        output.WriteInt32(RpcId);
      }
      if (Error != 0) {
        output.WriteRawTag(216, 5);
        output.WriteInt32(Error);
      }
      if (Message.Length != 0) {
        output.WriteRawTag(226, 5);
        output.WriteString(Message);
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
      if (Error != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(Error);
      }
      if (Message.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (State != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) State);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S2C_RegisterMessage other) {
      if (other == null) {
        return;
      }
      if (other.RpcId != 0) {
        RpcId = other.RpcId;
      }
      if (other.Error != 0) {
        Error = other.Error;
      }
      if (other.Message.Length != 0) {
        Message = other.Message;
      }
      if (other.State != 0) {
        State = other.State;
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
          case 16: {
            state_ = (global::Crazy.Common.S2C_RegisterMessage.Types.State) input.ReadEnum();
            break;
          }
          case 720: {
            RpcId = input.ReadInt32();
            break;
          }
          case 728: {
            Error = input.ReadInt32();
            break;
          }
          case 738: {
            Message = input.ReadString();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the S2C_RegisterMessage message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum State {
        [pbr::OriginalName("Fail")] Fail = 0,
        [pbr::OriginalName("OK")] Ok = 1,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
