namespace OSS.Core.Domain
{
    /// <summary>
    ///  基础实体
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public abstract class BaseMo<IdType>:IDomainId<IdType>
    {
        /// <inheritdoc />
        public IdType id { get; set; } = default!;
    }
}