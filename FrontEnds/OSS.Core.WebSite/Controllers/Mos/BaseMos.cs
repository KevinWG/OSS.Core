namespace OSS.Core.WebSite.Controllers.Mos
{
    public class BaseMo
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long create_time { get; set; }
    }


    public class BaseAutoMo : BaseMo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
    }

    public class BaseMo<MoType> : BaseMo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public MoType Id { get; set; }
    }

    

}
