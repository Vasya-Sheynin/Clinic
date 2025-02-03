using Application.Dtos;
using AutoMapper;
using Documents;
using DocumentsRepo;

namespace Application.DocumentsService;

public class DocumentsService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;

    public DocumentsService(IDocumentRepo documentRepo, IMapper mapper)
    {
        _documentRepo = documentRepo;
        _mapper = mapper;
    }

    public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto document)
    {
        var doc = _mapper.Map<Document>(document);
        var createdDoc = await _documentRepo.CreateDocumentAsync(doc);
        await _documentRepo.SaveChanges();

        return _mapper.Map<DocumentDto>(createdDoc);
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        await _documentRepo.DeleteDocumentAsync(id);
        await _documentRepo.SaveChanges();
    }

    public async Task<DocumentDto?> GetDocumentByIdAsync(Guid id)
    {
        var doc = await _documentRepo.GetDocumentByIdAsync(id);
        return _mapper.Map<DocumentDto>(doc);
    }

    public async Task<IEnumerable<DocumentDto>?> GetDocumentsAsync()
    {
        var docs = await _documentRepo.GetDocumentsAsync();
        return _mapper.Map<IEnumerable<DocumentDto>>(docs);
    }

    public async Task UpdateDocumentAsync(Guid id, UpdateDocumentDto document)
    {
        var doc = _mapper.Map<Document>(document);
        doc.Id = id;

        await _documentRepo.UpdateDocumentAsync(doc);
        await _documentRepo.SaveChanges();
    }
}
