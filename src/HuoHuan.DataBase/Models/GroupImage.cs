namespace HuoHuan.DataBase.Models
{
    /// <summary>
    /// 群图片
    /// </summary>
    public class GroupImage
    {
        /// <summary>
        /// 资源url
        /// </summary>
        public string Url { get; init; } = null!;
        /// <summary>
        /// 群名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 二维码解析内容
        /// </summary>
        public string? QrText { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime InvalideTime { get; set; }
        /// <summary>
        /// 本地存储文件
        /// </summary>
        public string? LocalFile { get; set; }
    }
}
