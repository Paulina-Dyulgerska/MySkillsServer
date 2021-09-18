namespace MySkillsServer.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MySkillsServer.Data.Models;

    public class BlogPostCategoryConfiguration : IEntityTypeConfiguration<BlogPostCategory>
    {
        public void Configure(EntityTypeBuilder<BlogPostCategory> blogPostCategory)
        {
            blogPostCategory
                .HasQueryFilter(bc => !bc.BlogPost.IsDeleted);
        }
    }
}
