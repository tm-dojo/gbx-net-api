using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GBX.NET;
using GBX.NET.Engines.Game;
using GbxNetApi.Classes;
using System.Text.Json;

namespace GbxNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        [HttpPost("blocks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MapBlocksData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ProcessMap(IFormFile file)
        {
            // Open and parse map file
            Stream stream = file.OpenReadStream();
            var node = GameBox.ParseNode(stream);

            if (node is CGameCtnChallenge map)
            {
                // Store regular NADEO blocks
                List<NadeoBlock> nadeoBlocks = new List<NadeoBlock>();
                if (map.Blocks != null)
                {
                    nadeoBlocks = map.Blocks
                        .Where(block => block.Flags != -1)
                        .Select(block => new NadeoBlock(block))
                        .ToList();
                }

                // Store anchored objects
                List<AnchoredObject> anchoredObjects = new List<AnchoredObject>();
                if (map.AnchoredObjects != null)
                {
                    anchoredObjects = map.AnchoredObjects
                        .Select(anchoredObject => new AnchoredObject(anchoredObject))
                        .ToList();
                }

                // Store all map block data in order to return the data formatted nicely
                MapBlocksData mapBlocksData = new MapBlocksData(
                    nadeoBlocks, 
                    anchoredObjects
                );

                return Ok(mapBlocksData);
            } 

            return BadRequest("The provided file was not able to be parsed as a 'CGameCtnChallenge', please provide a '.Map.Gbx' file.");
        }
    }
}
