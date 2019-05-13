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
    public string Label { set; private get; }

}
