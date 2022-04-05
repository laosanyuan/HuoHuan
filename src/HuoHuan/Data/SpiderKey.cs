using HuoHuan.Models;
using HuoHuan.Utils;
using System.Collections.Generic;
using System.IO;

namespace HuoHuan.Data
{
    internal class SpiderKey
    {
        internal static List<SpiderData> TiebaDatas => YamlUtil.Deserializer<List<SpiderData>>(File.ReadAllText("Data//tieba_spider_key.yaml"));
    }
}
