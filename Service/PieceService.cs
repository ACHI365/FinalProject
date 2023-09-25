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
        string normalizedPieceName = pieceDto.Name.ToLower();
        Piece? pieceExist = await _context.Pieces.FirstOrDefaultAsync(p =>
            p.Name.ToLower() == normalizedPieceName && p.Group == pieceDto.Group);
        if (pieceExist != null)
            return Result<Piece>.Success(pieceExist);
        try
        {
            Piece piece = MapDto(pieceDto);
            _context.Pieces.Add(piece);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return Result<Piece?>.Fail("A piece with the same name already exists.");
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

    private Piece MapDto(PieceDto pieceDto)
    {
        return new Piece
        {
            Name = pieceDto.Name,
            Group = pieceDto.Group
        };
    }
}