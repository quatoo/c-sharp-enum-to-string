using enum_example.helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace enum_example.context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {}

    public DbSet<Todo>? Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var tagsConverter = new ValueConverter<ICollection<Tags>?, string>(
            v => fromTags(v),
            v => toTags(v));

        mb.Entity<Todo>()
            .HasKey(t => t.Id);

        mb.Entity<Todo>()
            .Property(t => t.Name)
            .HasMaxLength(250);

        mb.Entity<Todo>()
            .Property(t => t.Tags)
            .HasConversion(tagsConverter);
        
        base.OnModelCreating(mb);
    }

    private string fromTags(ICollection<Tags>? tags)
    {
        return string.Join(',', tags.Select(e => EnumHelper.GetEnumDescription(e)).ToArray());
    }

    private ICollection<Tags> toTags(string value)
    {
        return value.Split(new[] { ',' })
                    .Select(e => EnumHelper.GetEnumValueFromDescription<Tags>(e))
                    .Cast<Tags>()
                    .ToList();
    }
}