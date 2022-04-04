namespace HuoHuan.Models
{
    /// <summary>
    /// 爬取参数数据
    /// </summary>
    internal class SpiderData
    {
        /// <summary>
        /// 爬取关键词
        /// </summary>
        public string Key { get; set; } = null!;
        /// <summary>
        /// 爬取页数
        /// </summary>
        public int Page { get; set; }
    }
}
