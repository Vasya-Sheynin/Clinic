using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public class TestCaseSources
{
    public static IEnumerable<TestCaseData> UpdateOfficeTestCasesNotFound
    {
        get
        {
            var nonExistingId = Guid.Parse("00000000-1234-5678-3214-000000000000");
            yield return new TestCaseData(nonExistingId,
                new UpdateOfficeDto(Guid.NewGuid(), "Addr1-1", "+6519", false));
            yield return new TestCaseData(nonExistingId,
                new UpdateOfficeDto(Guid.NewGuid(), "Addr1-2", "+29821", true));
            yield return new TestCaseData(nonExistingId,
                new UpdateOfficeDto(Guid.NewGuid(), "Addr1-3", "+89850989", false));
        }
    }

    public static IEnumerable<TestCaseData> UpdateOfficeTestCasesInvalid
    {
        get
        {
            var id = Guid.Parse("5d1b9a8a-b8e5-4b0e-81a2-f62b07a8c2c5");
            yield return new TestCaseData(id,
                new UpdateOfficeDto(Guid.NewGuid(), "Addr1-2", "29#671", true));
            yield return new TestCaseData(id,
                new UpdateOfficeDto(Guid.NewGuid(), "Addr1-3", "+898y098p", false));
        }
    }

    public static IEnumerable<TestCaseData> CreateOfficeTestCasesInvalid
    {
        get
        {
            yield return new TestCaseData(
                new CreateOfficeDto(Guid.NewGuid(), "Address123", "789mno504", true));
            yield return new TestCaseData(
                new CreateOfficeDto(Guid.NewGuid(), "Str.41", "+89xyz989", false));
        }
    }
}
