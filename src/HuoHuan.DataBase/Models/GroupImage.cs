using Dapper.Contrib.Extensions;

namespace HuoHuan.DataBase.Models
{
    /// <summary>
    /// 群图片
    /// </summary>
    [Table("groups")]
    public record GroupImage
    {
        /// <summary>
        /// 资源url
        /// </summary>
        [ExplicitKey]
        public string Url { get; init; } = null!;
        /// <summary>
        /// 群名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 二维码解析内容
        /// </summary>
        public string? QRText { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateOnly InvalidateDate { get; init; }
        /// <summary>
        /// 本地存储文件
        /// </summary>
        public string? LocalFile { get; set; }
    }
}
