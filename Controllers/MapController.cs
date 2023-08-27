using Microsoft.AspNetCore.Mvc;
using GBX.NET;
using GBX.NET.Engines.Game;
using GbxNetApi.Classes;
using System.Text.Json;
using Amazon.S3.Model;
using System.Text.Json.Serialization;

namespace GbxNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<MapBlocksData> ExtractBlocksFromMapUid(string mapUid)
        {
            List<string> blackListedBlocks = new List<string>()
            {
                "VoidBlock1x1",
                "Misc\\VoidFull.Block.Gbx_CustomBlock",
                "Grass"
            };

            Stream stream = await NadeoService.GetMapFileStreamFromUrlAsync(mapUid);

            // Load block offsets json file
            string json = System.IO.File.ReadAllText("./AllBlockOffsets.json");

            List<BlockOffset> allBlockOffsets = JsonSerializer.Deserialize<List<BlockOffset>>(json);

            // Open and parse map file
            var node = GameBox.ParseNode(stream);

            if (node is CGameCtnChallenge map)
            {
                // Store regular NADEO blocks
                List<NadeoBlock> nadeoBlocks = new List<NadeoBlock>();
                if (map.Blocks != null)
                {
                    nadeoBlocks = map.Blocks
                        .Where(block => block.Flags != -1 && !block.IsFree && !blackListedBlocks.Contains(block.Name))
                        .Select(block => new NadeoBlock(block, allBlockOffsets))
                        .Distinct()
                        .ToList();
                }

                // Store anchored objects
                List<AnchoredObject> anchoredObjects = new List<AnchoredObject>();
                if (map.AnchoredObjects != null)
                {
                    anchoredObjects = map.AnchoredObjects
                        .Where(block => !blackListedBlocks.Contains(block.ItemModel.Id))
                        .Select(anchoredObject => new AnchoredObject(anchoredObject))
                        .Distinct()
                        .ToList();
                }

                List<FreeModeBlock> freeModeBlocks = new List<FreeModeBlock>();

                freeModeBlocks = map.Blocks
                    .Where(block => block.IsFree && !blackListedBlocks.Contains(block.Name))
                    .Select(block => new FreeModeBlock(block))
                    .ToList();

                MapBlocksData mapBlocksData = new MapBlocksData(
                    nadeoBlocks,
                    anchoredObjects,
                    freeModeBlocks
                );

                return mapBlocksData;
            }

            return null;
        }

        [HttpPost("blocks/{mapUid=mapUid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MapBlocksData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMapBlocksAsync([FromHeader(Name = "secret")] string secret, string mapUid)
        {
            if (secret != _configuration.GetSection("MapBlocksSecret").Value) {
                return Unauthorized("Invalid secret");
            }

            MapBlocksData mapBlocks = await S3.GetMapBlockFromS3(mapUid);

            if (mapBlocks == null)
            {
                mapBlocks = await ExtractBlocksFromMapUid(mapUid);

                if (mapBlocks != null)
                {
                    // Store result object in S3
                    PutObjectResponse putObjectResponse = await S3.UploadBlocksJson(mapBlocks, mapUid);

                    return Ok(mapBlocks);
                }
                else
                {
                    return BadRequest("Could not extract block informations");
                }
            }

            return Ok(mapBlocks);
        }
    }
}
