using FinalProject.Model;
using FinalProject.Model.Dto;
using FinalProject.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PieceController : ControllerBase
    {
        private readonly IPieceService _pieceService;

        public PieceController(IPieceService pieceService)
        {
            _pieceService = pieceService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPieces()
        {
            var pieces = await _pieceService.GetAllPieces();
            return Ok(pieces);
        }
        
        [HttpGet("getAllGroup/{groupId:int}")]
        public async Task<IActionResult> GetAllPiecesByGroup(int groupId)
        {
            var pieces = await _pieceService.GetAllPiecesByGroup(groupId);
            return Ok(pieces);
        }
        

        [HttpPost("create-piece")]
        // [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreatePiece([FromBody] PieceDto? piece)
        {
            Console.WriteLine("HERE");
            var result = await _pieceService.CreatePiece(piece);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        [HttpGet("get/{pieceName}")]
        public async Task<IActionResult> GetPieceByName(string pieceName)
        {
            var result = await _pieceService.GetPieceByName(pieceName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }
        
        [HttpGet("getId/{pieceId}")]
        public async Task<IActionResult> GetPieceByName(int pieceId)
        {
            var result = await _pieceService.GetPieceById(pieceId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }
        
    }
}