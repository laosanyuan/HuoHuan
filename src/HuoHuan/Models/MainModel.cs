using System.Drawing;

namespace HuoHuan.Models
{
    /// <summary>
    /// 展示图片
    /// </summary>
    public record DisplayImageInfo
    {
        public Bitmap Image { get; set; } = null!;
        public bool IsValid { get; set; }
    }
}
