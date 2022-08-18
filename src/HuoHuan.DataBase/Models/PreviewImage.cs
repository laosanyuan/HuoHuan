using Dapper.Contrib.Extensions;

namespace HuoHuan.DataBase.Models
{
    /// <summary>
    /// 预识别图片
    /// </summary>
    [Table("urls")]
    public class PreviewImage
    {
        [ExplicitKey]
        public string Url { get; init; } = null!;
        public DateTime DateTime { get; set; }
    }
}
