using Application.Dtos;

namespace Application.DocumentsService;

public interface IDocumentService
{
    Task<DocumentDto?> GetDocumentByIdAsync(Guid id);
    Task<IEnumerable<DocumentDto>?> GetDocumentsAsync();
    Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto document);
    Task UpdateDocumentAsync(Guid id, UpdateDocumentDto document);
    Task DeleteDocumentAsync(Guid id);
}
