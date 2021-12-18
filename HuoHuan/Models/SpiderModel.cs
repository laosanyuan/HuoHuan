using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Key { get; set; }
        /// <summary>
        /// 爬取页数
        /// </summary>
        public int Page { get; set; }
    }
}
