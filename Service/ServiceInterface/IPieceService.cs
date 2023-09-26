using FinalProject.Model;
using FinalProject.Model.Dto;

namespace FinalProject.Service.ServiceInterface;

public interface IPieceService
{
    Task<IEnumerable<Piece>> GetAllPieces();
    Task<Result<Piece?>> CreatePiece(PieceDto piece);
    Task<Result<Piece>> GetPieceByName(string pieceName);
    Task<Result<Piece>> GetPieceById(int pieceId);
    Task<IEnumerable<Piece>> GetAllPiecesByGroup(int group);
    void RatePiece(Piece piece, int score, int userId);
}