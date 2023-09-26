using System.Diagnostics.CodeAnalysis;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Model.Dto;
using FinalProject.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Service;

public class PieceService : IPieceService
{
    private readonly DataContext _context;

    public PieceService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Piece>> GetAllPieces()
    {
        return await _context.Pieces.ToListAsync();
    }

    public async Task<IEnumerable<Piece>> GetAllPiecesByGroup(int group)
    {
        return await _context.Pieces.Where(p => (int)(p.Group) == group).ToListAsync();
    }

    public async Task<Result<Piece?>> CreatePiece(PieceDto pieceDto)
    {
        var pieceResult = GetPieceByName(pieceDto.Name).Result;
        if (pieceResult.IsSuccess)
            return Result<Piece>.Success(pieceResult.Data);
        try
        {
            Piece piece = MapDto(pieceDto);
            _context.Pieces.Add(piece);
            var result = await _context.SaveChangesAsync();
            if (result == 0) return Result<Piece?>.Fail("A piece with the same name already exists.");
            return Result<Piece?>.Success(piece);
        }
        catch (DbUpdateException ex)
        {
            return Result<Piece?>.Fail("A piece with the same name already exists.");
        }
    }

    public async Task<Result<Piece>> GetPieceByName(string pieceName)
    {
        Piece? piece = await _context.Pieces.SingleOrDefaultAsync(u => u!.Name == pieceName);
        if (piece != null) return Result<Piece>.Success(piece);
        return Result<Piece>.Fail("Piece with such name does not exist");
    }

    public async Task<Result<Piece>> GetPieceById(int pieceId)
    {
        Piece? piece = await _context.Pieces.SingleOrDefaultAsync(u => u!.PieceId == pieceId);
        if (piece != null) return Result<Piece>.Success(piece);
        return Result<Piece>.Fail("Piece with such name does not exist");
    }

    public async void RatePiece(Piece piece, int score, int userId)
    {
        var existingRating =
            await _context.Ratings.FirstOrDefaultAsync(r => r.PieceId == piece.PieceId && r.UserId == userId);
        if (existingRating != null)
            RateUpdate(existingRating, score);
        else
            CreateRating(score, piece, userId);
        UpdateRatings(piece, score);
    }

    private async void UpdateRatings(Piece piece, int score)
    {
        var ratings = await _context.Ratings.Where(r => r.PieceId == piece.PieceId).Select(r => r.Score).ToListAsync();
        ratings.Add(score);
        piece.AverageRating = ratings.Count > 0 ? ratings.Average() : 0;
        _context.Pieces.Update(piece);
        await _context.SaveChangesAsync();
    }

    private void RateUpdate(Rating existingRating, int score)
    {
        existingRating.Score = score;
        existingRating.DateRated = DateTime.Now;
        _context.Ratings.Update(existingRating);
    }

    private void CreateRating(int score, Piece piece, int userId)
    {
        Rating newRating = new Rating
        {
            PieceId = piece.PieceId,
            UserId = userId,
            Score = score,
            DateRated = DateTime.Now
        };
        _context.Ratings.Add(newRating);
    }

    private Piece MapDto(PieceDto pieceDto)
    {
        return new Piece
        {
            Name = pieceDto.Name,
            Group = pieceDto.Group
        };
    }
}