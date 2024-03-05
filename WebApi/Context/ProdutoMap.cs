using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Context
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutosModel>
    {
        public void Configure(EntityTypeBuilder<ProdutosModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(75);
            builder.Property(x => x.Preco).IsRequired();
            builder.Property(x => x.Categoria).IsRequired();
            builder.Property(x => x.Estoque).IsRequired();
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(300);
            builder.Property(x => x.DiretorioImg).IsRequired().HasMaxLength(75);
        }
    }
}