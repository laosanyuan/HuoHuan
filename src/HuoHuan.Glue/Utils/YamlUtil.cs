using YamlDotNet.Serialization;

namespace HuoHuan.Glue.Utils
{
    public class YamlUtil
    {
        private static Deserializer deserializer = null!;

        /// <summary>
        /// 反序列化Yaml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yaml"></param>
        /// <returns></returns>
        public static T Deserializer<T>(string yaml)
        {
            if (deserializer == null)
            {
                deserializer = new Deserializer();
            }
            return deserializer.Deserialize<T>(yaml);
        }
    }
}
