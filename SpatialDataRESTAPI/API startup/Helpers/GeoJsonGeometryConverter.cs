using NavSpatialData.GeoJsonDTO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GeoJsonGeometryConverter : JsonConverter<GeoJsonGeometry>
{
    public override GeoJsonGeometry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement deserialization logic here if needed, or throw if not supported.
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, GeoJsonGeometry value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
