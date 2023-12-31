﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Model.Dto;
using FinalProject.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PieceController : ControllerBase
    {
        private readonly IPieceService _pieceService;
        private readonly DataContext _dataContext;

        public PieceController(IPieceService pieceService, DataContext dataContext)
        {
            _dataContext = dataContext;
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
        
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var userId))
                return userId;
            return -1;
        }

        [HttpPost("create-piece")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreatePiece([FromBody] PieceDto? piece)
        {
            var result = await _pieceService.CreatePiece(piece!);
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
        
        [HttpPost("rate-piece/{pieceId}/{score}")]
        [Authorize]
        public async Task<IActionResult> RatePiece(int pieceId, int score)
        {
            var userId = GetCurrentUserId();  
            var piece = await _dataContext.Pieces.FindAsync(pieceId);
            if (piece == null) return NotFound("Piece not found.");
            _pieceService.RatePiece(piece, score, userId);
            return Ok("Piece rated successfully");
        }

        [HttpGet("get-rating/{pieceId}")]
        [Authorize]
        public async Task<IActionResult> GetRating(int pieceId)
        {
            var userId = GetCurrentUserId();  
            var rating = await _dataContext.Ratings.FirstOrDefaultAsync(r => r.PieceId == pieceId && r.UserId == userId);
            if (rating == null) return NotFound("Rating not found.");
            return Ok(rating.Score);
        }
        
    }
}