using GBX.NET.Engines.Game;
using System.Text.Json.Serialization;

namespace GbxNetApi.Classes
{
    public class AnchoredObject
    {
        [JsonConstructor]
        public AnchoredObject() { }

        public AnchoredObject(CGameCtnAnchoredObject anchoredObject)
        {
            name = anchoredObject.ItemModel.Id;

            pos = new List<float>()
            {
                anchoredObject.AbsolutePositionInMap.X,
                anchoredObject.AbsolutePositionInMap.Y,
                anchoredObject.AbsolutePositionInMap.Z
            };

            pitch = anchoredObject.PitchYawRoll.X;
            yaw = anchoredObject.PitchYawRoll.Y;
            roll = anchoredObject.PitchYawRoll.Z;
        }

        public string name { get; set; }
        public List<float> pos { get; set; }
        public float pitch { get; set; }
        public float yaw { get; set; }
        public float roll { get; set; }
    }
}
