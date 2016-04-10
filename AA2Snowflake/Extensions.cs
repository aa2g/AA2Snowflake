using SB3Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AA2Snowflake
{
    public static class Extensions
    {
        public static string GetFilename(this string path, char seperator = '\\')
        {
            return path.Remove(0, path.LastIndexOf(seperator) + 1);
        }

        public static string RemoveFilename(this string path, char seperator = '\\')
        {
            return path.Remove(path.LastIndexOf(seperator));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static int IndexOfKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            for (int index = 0; index < dictionary.Count; index++)
                if (Equals(dictionary.ElementAt(index).Key, key))
                    return index;

            return -1;
        }

        public static byte[] ToByteArray(this Stream str)
        {
            str.Position = 0;
            byte[] buffer = new byte[str.Length];
            str.Read(buffer, 0, (int)str.Length);
            return buffer;
        }
        

        /// <summary>
        /// Serialize an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="toSerialize">Object to serialize.</param>
        /// <returns>Serialized object.</returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        /// <summary>
        /// Deserialize an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="toDeserialize">String to deserialize.</param>
        /// <returns>Deserialized object</returns>
        public static T DeserializeObject<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static MemoryStream ToStream(this IWriteFile iw)
        {
            MemoryStream mem = new MemoryStream();
            iw.WriteTo(mem);
            mem.Position = 0;
            return mem;
        }
    }
}
