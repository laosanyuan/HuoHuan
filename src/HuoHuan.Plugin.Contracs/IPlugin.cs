namespace HuoHuan.Plugin.Contracs
{
    public interface IPlugin
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public ISpider Spider { get; init; }
        /// <summary>
        /// 是否需要配置
        /// </summary>
        public bool IsNeedConfig { get; }
        /// <summary>
        /// 插件配置
        /// </summary>
        public IConfig Config { get; init; }

        #region [Methods]
        /// <summary>
        /// 当前插件是否有效
        /// </summary>
        public bool IsValid();
        /// <summary>
        /// 初始化
        /// </summary>
        public Task Init();
        #endregion
    }
}
