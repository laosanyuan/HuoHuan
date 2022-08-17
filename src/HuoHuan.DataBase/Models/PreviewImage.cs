namespace HuoHuan.DataBase.Models
{
    /// <summary>
    /// 预识别图片
    /// </summary>
    public class PreviewImage
    {
        public string Url { get; init; } = null!;
        public DateTime DateTime { get; set; }
    }
}
