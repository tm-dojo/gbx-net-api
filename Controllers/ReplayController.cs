using GBX.NET;
using GBX.NET.Engines.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using static GBX.NET.Engines.Game.CGameGhost.Data;

namespace GbxNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplayController : ControllerBase
    {
        [HttpPost("ghostGbx")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK, "application/octet-stream")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ProcessGhost(IFormFile file)
        {
            // Open and parse file
            Stream fileStream = file.OpenReadStream();
            var node = GameBox.ParseNode(fileStream);

            // Prepare stream writer
            Stream outputStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(outputStream);

            if (node is CGameCtnGhost ghost)
            {
                if (ghost.RecordData != null && ghost.SampleData?.Samples != null)
                {
                    List<Sample> validSamples = ghost.SampleData.Samples
                        .Where(sample => sample?.Time != null)
                        .ToList();

                    // Write sample data to output stream
                    WriteSamplesToTmDojoFile(writer, validSamples);
                }
            }
               
            // Respond with output stream
            outputStream.Position = 0;
            return File(outputStream, "application/octet-stream");
        }

        private static void WriteSamplesToTmDojoFile(
            BinaryWriter writer,
            List<Sample> samples,
            bool useExperimentalData = false)
        {
            foreach (Sample sample in samples)
            {
                // Timestamp
                writer.Write((int)sample.Time.TotalMilliseconds);

                // Position
                writer.Write((float)sample.Position.X);
                writer.Write((float)sample.Position.Y);
                writer.Write((float)sample.Position.Z);

                // Velocity
                writer.Write((float)sample.Velocity.X);
                writer.Write((float)sample.Velocity.Y);
                writer.Write((float)sample.Velocity.Z);

                // Speed
                writer.Write((float)sample.VelocitySpeed);

                // Steer
                if (useExperimentalData)
                {
                    // TODO: use steer angle
                    //byte steerRaw = sample.Unknown[14];
                    //float steer = ((steerRaw / 255.0f) - 0.5f) * 2.0f;
                    //writer.Write(steer);
                    writer.Write((float)0.0f);
                }
                else
                {
                    writer.Write((float)0.0f);
                }

                // Wheelangle default 0
                // TODO: wheel angle
                writer.Write((float)1.0f);

                // Gas and brake default 0
                // TODO: use gas and brake
                writer.Write((int)2);

                // Engine RPM default 0
                if (useExperimentalData)
                {
                    // TODO: use gear
                    //byte gearRaw = sample.Unknown[91];
                    //int gear = (int)gearRaw / 5;
                    //writer.Write(gear);
                    writer.Write((int)0);
                }
                else
                {
                    writer.Write((int)0);
                }

                // Gear default 0
                writer.Write((int)0);

                // Up vector
                // TODO: Get up vector somehow form the sample data, default 0
                writer.Write((float)0.0f);
                writer.Write((float)0.0f);
                writer.Write((float)0.0f);

                // Rotation vector
                Vec3 rotation = sample.Rotation.ToPitchYawRoll();
                writer.Write((float)rotation.X);
                writer.Write((float)rotation.Y);
                writer.Write((float)rotation.Z);

                // Ground material for 4 wheel, all default 0, damperlength around 30
                for (int i = 0; i < 4; i++)
                {
                    writer.Write((byte)0);
                    writer.Write(0.0f);
                    writer.Write(0.0f);
                }
            }
        }
    }

}