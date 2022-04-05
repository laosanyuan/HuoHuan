using HuoHuan.Enums;
using System;

#nullable disable

namespace HuoHuan.Models
{
    /// <summary>
    /// 群数据
    /// </summary>
    internal class GroupData
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// 群名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 二维码转换内容
        /// </summary>
        public string QRText { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime InvalidateDate { get; set; }
        /// <summary>
        /// 本地存储路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 二维码类型
        /// </summary>
        public QRCodeType Type { get; set; }
    }
}
