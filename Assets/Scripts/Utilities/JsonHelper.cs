using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// JSON序列化工具类，替代Unity的JsonUtility
/// 使用Newtonsoft.Json提供更强大的序列化功能
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// JSON序列化设置
    /// </summary>
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        // 忽略空值
        NullValueHandling = NullValueHandling.Ignore,
        // 格式化输出（开发时使用，发布时可设为None）
        Formatting = Formatting.Indented,
        // 处理循环引用
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        // 日期时间格式
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        // 自定义转换器
        Converters = new List<JsonConverter>
        {
            new Vector3Converter(),
            new Vector2Converter(),
            new QuaternionConverter(),
            new ColorConverter()
        }
    };

    /// <summary>
    /// 序列化对象为JSON字符串
    /// </summary>
    /// <param name="obj">要序列化的对象</param>
    /// <returns>JSON字符串</returns>
    public static string ToJson(object obj)
    {
        try
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON序列化失败: {e.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// 反序列化JSON字符串为指定类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="json">JSON字符串</param>
    /// <returns>反序列化的对象</returns>
    public static T FromJson<T>(string json)
    {
        try
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON反序列化失败: {e.Message}");
            return default(T);
        }
    }

    /// <summary>
    /// 反序列化JSON字符串为指定类型（非泛型版本）
    /// </summary>
    /// <param name="json">JSON字符串</param>
    /// <param name="type">目标类型</param>
    /// <returns>反序列化的对象</returns>
    public static object FromJson(string json, Type type)
    {
        try
        {
            if (string.IsNullOrEmpty(json))
                return null;
            
            return JsonConvert.DeserializeObject(json, type, Settings);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON反序列化失败: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 尝试反序列化JSON字符串
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="json">JSON字符串</param>
    /// <param name="result">输出结果</param>
    /// <returns>是否成功</returns>
    public static bool TryFromJson<T>(string json, out T result)
    {
        result = default(T);
        try
        {
            if (string.IsNullOrEmpty(json))
                return false;
            
            result = JsonConvert.DeserializeObject<T>(json, Settings);
            return result != null;
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON反序列化失败: {e.Message}");
            return false;
        }
    }
}

/// <summary>
/// Vector3的JSON转换器
/// </summary>
public class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return Vector3.zero;

        var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
        return new Vector3(
            obj["x"]?.Value<float>() ?? 0f,
            obj["y"]?.Value<float>() ?? 0f,
            obj["z"]?.Value<float>() ?? 0f
        );
    }
}

/// <summary>
/// Vector2的JSON转换器
/// </summary>
public class Vector2Converter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WriteEndObject();
    }

    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return Vector2.zero;

        var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
        return new Vector2(
            obj["x"]?.Value<float>() ?? 0f,
            obj["y"]?.Value<float>() ?? 0f
        );
    }
}

/// <summary>
/// Quaternion的JSON转换器
/// </summary>
public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("w");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }

    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return Quaternion.identity;

        var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
        return new Quaternion(
            obj["x"]?.Value<float>() ?? 0f,
            obj["y"]?.Value<float>() ?? 0f,
            obj["z"]?.Value<float>() ?? 0f,
            obj["w"]?.Value<float>() ?? 1f
        );
    }
}

/// <summary>
/// Color的JSON转换器
/// </summary>
public class ColorConverter : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("r");
        writer.WriteValue(value.r);
        writer.WritePropertyName("g");
        writer.WriteValue(value.g);
        writer.WritePropertyName("b");
        writer.WriteValue(value.b);
        writer.WritePropertyName("a");
        writer.WriteValue(value.a);
        writer.WriteEndObject();
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return Color.white;

        var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
        return new Color(
            obj["r"]?.Value<float>() ?? 1f,
            obj["g"]?.Value<float>() ?? 1f,
            obj["b"]?.Value<float>() ?? 1f,
            obj["a"]?.Value<float>() ?? 1f
        );
    }
}