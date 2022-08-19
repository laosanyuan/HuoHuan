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
        public string? GroupName { get; set; }
        /// <summary>
        /// 二维码解析内容
        /// </summary>
        public string? QRText { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime InvalidateDate { get; init; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; init; } = null!;
        /// <summary>
        /// 本地存储路径
        /// </summary>
        public string? LocalPath { get; set; }

        [Computed]
        public string FullName => Path.Combine(LocalPath ?? "", FileName);
    }
}
