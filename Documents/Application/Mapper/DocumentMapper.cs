using Application.Dtos;
using AutoMapper;
using Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper;

public class DocumentMapper : Profile
{
    public DocumentMapper()
    {
        CreateMap<CreateDocumentDto, Document>()
            .AfterMap((src, dest) => dest.Id = Guid.NewGuid())
            .AfterMap((src, dest) => dest.Name = src.File.FileName);
        CreateMap<UpdateDocumentDto, Document>()
            .AfterMap((src, dest) => dest.Name = src.File.FileName);
        CreateMap<DocumentDto, Document>();
        CreateMap<Document, DocumentDto>();
    }
}
