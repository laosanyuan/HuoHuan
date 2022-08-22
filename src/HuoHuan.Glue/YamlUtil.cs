using YamlDotNet.Serialization;

namespace HuoHuan.Utils
{
    public class YamlUtil
    {
        private static Deserializer deserializer = null!;
        private static Serializer serializer = null!;

        /// <summary>
        /// 反序列化Yaml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="yaml"></param>
        /// <returns></returns>
        public static T Deserializer<T>(string yaml)
        {
            deserializer ??= new();
            return deserializer.Deserialize<T>(yaml);
        }

        /// <summary>
        /// 序列化对象为Yaml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serializer<T>(T obj)
        {
            if (obj is not null)
            {
                serializer ??= new();
                return serializer.Serialize(obj);
            }
            return default!;
        }
    }
}
