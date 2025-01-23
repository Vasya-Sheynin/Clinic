namespace DocumentsRepo;
using Documents;

public interface IDocumentRepo
{
    Task<Document?> GetDocumentByIdAsync(Guid id);
    Task<IEnumerable<Document>?> GetDocumentsAsync();
    Task<Document> CreateDocumentAsync(Document document);
    Task UpdateDocumentAsync(Document document);
    Task DeleteDocumentAsync(Guid id);
    Task SaveChanges();
}
