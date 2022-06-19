using System;
using System.Collections.Generic;
using System.Text;

namespace HuoHuan.Glue
{
    public interface IPlugin : ISpider, IDisposable
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 是否需要设置
        /// </summary>
        public bool CanConfig { get; }

        #region [Methods]
        /// <summary>
        /// 当前插件是否有效
        /// </summary>
        public bool IsValid();
        #endregion
    }
}
