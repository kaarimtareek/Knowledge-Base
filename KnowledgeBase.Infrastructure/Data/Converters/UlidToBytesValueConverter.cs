using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace KnowledgeBase.Infrastructure.Data.Converters;

public class UlidToBytesValueConverter : ValueConverter<Ulid, byte[]>
{
    public UlidToBytesValueConverter() : base(
        // The function to convert from the C# Ulid type TO the database byte[]
        // This uses the public .ToByteArray() method on the Ulid struct.
        ulid => ulid.ToByteArray(),
        
        // The function to convert from the database byte[] back TO the C# Ulid type
        // This uses the public Ulid constructor that takes a byte array.
        bytes => new Ulid(bytes))
    {
    }
}