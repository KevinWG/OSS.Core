namespace OSS.Core.Domain
{
    /// <summary>
    ///  租户id
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public interface IDomainTenantId<IdType>
    {
        public IdType tenant_id { get; set; }
    }

    /// <inheritdoc />
    public interface IDomainTenantId: IDomainTenantId<long>
    {
    }
}
