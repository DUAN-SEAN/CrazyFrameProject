using System;
public class ObjectBase
{
    /// <summary>
    /// 编号
    /// </summary>
    public Id Id { get; private set; } = Id.Create();

    /// <summary>
    /// 标签
    /// </summary>
    public Label Label { set; get; }

    /// <summary>
    /// you know that meaning.
    /// </summary>
    /// <value><c>true</c> if enable; otherwise, <c>false</c>.</value>
    public bool Enable { set; get; }

}

[Flags]
public enum Label
{
    NONE = 0,
    RED = 0x0001,
    BLUE = 0x0002,
    WEAPON = 0x0004,
    Environment = 0x0008
}