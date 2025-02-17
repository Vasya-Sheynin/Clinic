using Offices;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Util;

internal class OfficeComparer : IEqualityComparer<Office>
{
    public bool Equals(Office? x, Office? y)
    {
        if (x is null || y is null)
            return false;

        return x.Id == y.Id &&
            x.PhotoId == y.PhotoId &&
            x.Address == y.Address &&
            x.RegistryPhoneNumber == y.RegistryPhoneNumber &&
            x.IsActive == y.IsActive;
    }

    public int GetHashCode([DisallowNull] Office obj)
    {
        return obj.GetHashCode();
    }
}
