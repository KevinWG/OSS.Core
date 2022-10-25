namespace OSS.Core.Domain;

/// <summary>
///  租户Id
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface ITenantId<TType>
{
    /// <summary>
    ///  租户
    /// </summary>
    public TType tenant_id { get; set; }
}