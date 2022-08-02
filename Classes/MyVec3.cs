using GBX.NET;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GbxNetApi.Classes
{
    public class MyVec3 : JsonConverter<MyVec3>
    {
        public MyVec3(Vec3 vec)
        {
            x = vec.X;
            y = vec.Y;
            z = vec.Z;
        }

        public MyVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public override MyVec3? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, MyVec3 value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(x);
            writer.WriteNumberValue(y);
            writer.WriteNumberValue(z);
            writer.WriteEndArray();
        }
    }
}
