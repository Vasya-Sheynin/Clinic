using Application.AzureBlobService;
using Application.DocumentsService;
using Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DocumentsController.Controllers;

[Route("api/document")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _docService;
    private readonly IBlobService _blobService;
    private readonly IOptions<BlobServiceOptions> _blobServiceOptions;

    public DocumentsController(
        IDocumentService docService,
        IBlobService blobService,
        IOptions<BlobServiceOptions> blobServiceOptions)
    {
        _docService = docService;
        _blobService = blobService;
        _blobServiceOptions = blobServiceOptions;
    }

    [Authorize(Roles = "Patient, Doctor, Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var doc = await _docService.GetDocumentByIdAsync(id);
        if (doc == null)
            return NotFound();

        var blob = await _blobService.ReadBlobAsync(
            _blobServiceOptions.Value.AzureBlobContainerName, 
            doc.Name);

        return File(blob.Stream, blob.ContentType);
    }

    [Authorize(Roles = "Patient, Doctor, Receptionist")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetFilesData()
    {
        var docs = await _docService.GetDocumentsAsync();
        if (docs == null)
            return NotFound();
        else
            return Ok(docs);
    }

    [Authorize(Roles = "Doctor, Receptionist")]
    [HttpPost]
    public async Task<ActionResult> Create([FromForm] CreateDocumentDto createDocumentDto)
    {
        var createdDoc = await _docService.CreateDocumentAsync(createDocumentDto);

        await _blobService.UploadBlobAsync(
            _blobServiceOptions.Value.AzureBlobContainerName, 
            createdDoc.Name,
            createDocumentDto.File.OpenReadStream(),
            createDocumentDto.File.ContentType);

        return CreatedAtAction(nameof(GetById), new { createdDoc.Id }, createdDoc);
    }

    [Authorize(Roles = "Doctor, Receptionist")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromForm] UpdateDocumentDto updateDocumentDto)
    {
        var docToUpdate = await _docService.GetDocumentByIdAsync(id);
        if (docToUpdate == null)
            return NotFound();

        await _docService.UpdateDocumentAsync(id, updateDocumentDto);
        await _blobService.UpdateBlobAsync(
            _blobServiceOptions.Value.AzureBlobContainerName, 
            docToUpdate.Name,
            updateDocumentDto.File.FileName,
            updateDocumentDto.File.OpenReadStream(),
            updateDocumentDto.File.ContentType);

        return Ok();
    }

    [Authorize(Roles = "Doctor, Receptionist")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var docToDelete = await _docService.GetDocumentByIdAsync(id);
        if (docToDelete == null) 
            return NotFound();

        await _docService.DeleteDocumentAsync(id);
        await _blobService.DeleteBlobAsync(
            _blobServiceOptions.Value.AzureBlobContainerName, 
            docToDelete.Name);

        return Ok();
    }
}
