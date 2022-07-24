using System;

#nullable disable

namespace HuoHuan.Models
{
    /// <summary>
    /// 群数据
    /// </summary>
    internal record GroupData
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string SourceUrl { get; init; }
        /// <summary>
        /// 群名
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// 二维码转换内容
        /// </summary>
        public string QRText { get; init; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime InvalidateDate { get; init; }
        /// <summary>
        /// 本地存储路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; init; }
    }
}
