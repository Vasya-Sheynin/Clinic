using DocumentsRepo;
using Documents;
using MongoDB.Driver.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DocumentRepo : IDocumentRepo
{
    private readonly DocumentsDbContext _documentsDbContext;

    public DocumentRepo(DocumentsDbContext documentsDbContext)
    {
        _documentsDbContext = documentsDbContext;
    }

    public async Task<Document> CreateDocumentAsync(Document document)
    {
        await _documentsDbContext.AddAsync(document);

        return document;
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        var docToRemove = await GetDocumentByIdAsync(id);

        if (docToRemove != null)
        {
            _documentsDbContext.Remove(docToRemove);
        }
    }

    public async Task<Document?> GetDocumentByIdAsync(Guid id)
    {
        var doc = _documentsDbContext.Documents.AsNoTracking().AsEnumerable().FirstOrDefault(d => d.Id == id);
        
        return doc;
    }

    public async Task<IEnumerable<Document>?> GetDocumentsAsync()
    {
        var docs = _documentsDbContext.Documents.AsNoTracking().AsEnumerable();

        return docs;
    }

    public async Task SaveChanges()
    {
        await _documentsDbContext.SaveChangesAsync();
    }

    public async Task UpdateDocumentAsync(Document document)
    {
        _documentsDbContext.Update(document);
    }
}
