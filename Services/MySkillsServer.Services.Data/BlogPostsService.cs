namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.BlogPosts;
    using MySkillsServer.Web.ViewModels.Comments;

    public class BlogPostsService : IBlogPostService
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPostsRepository;
        private readonly IRepository<BlogPostCategory> blogPostCategoriesRepository;
        private readonly IRepository<Comment> commentsRepository;
        private readonly BlobServiceClient blobServiceClient;

        public BlogPostsService(
            IDeletableEntityRepository<BlogPost> blogPostsRepository,
            IRepository<BlogPostCategory> blogPostCategoriesRepository,
            IRepository<Comment> commentsRepository,
            BlobServiceClient blobServiceClient)
        {
            this.blogPostsRepository = blogPostsRepository;
            this.blogPostCategoriesRepository = blogPostCategoriesRepository;
            this.commentsRepository = commentsRepository;
            this.blobServiceClient = blobServiceClient;
        }

        public int GetCount()
        {
            return this.blogPostsRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.blogPostsRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.blogPostsRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.blogPostsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.PublishDate)
                .ThenBy(x => x.Title)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.blogPostsRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.PublishDate)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.blogPostsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(BlogPostCreateInputModel input, string userId, string imageFilesDirectory)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //   .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //// take the user and record its id in the article, product, conformity, etc.
            var entity = new BlogPost
            {
                Title = input.Title.Trim(),
                Author = input.Author.Trim(),
                Details = input.Details.Trim(),
                PublishDate = input.PublishDate,
                ExternalPostUrl = input.ExternalPostUrl,
                UserId = userId,
            };

            await this.blogPostsRepository.AddAsync(entity);

            Stream fileStream = await this.UploadFileAsync(input, imageFilesDirectory, entity);

            await this.blogPostsRepository.SaveChangesAsync();

            await this.AddCategoriesAsync(entity, input.Categories);

            return entity.Id;
        }

        public async Task<string> EditAsync(BlogPostEditInputModel input, string userId, string imageFilesDirectory)
        {
            var entity = await this.blogPostsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.\\
            entity.Title = input.Title.Trim();
            entity.Author = input.Author.Trim();
            entity.Details = input.Details.Trim();
            entity.PublishDate = input.PublishDate;
            entity.ExternalPostUrl = input.ExternalPostUrl;
            entity.UserId = userId;
            entity.Categories = null;
            entity.Likes = input.Likes;

            // Stream fileStream = await this.UploadFileAsync(input, imageFilesDirectory, entity);
            await this.blogPostsRepository.SaveChangesAsync();

            // await this.AddCategoriesAsync(entity, input.Categories);
            return entity.Id;
        }

        public async Task<int> EditLikesAsync(BlogPostEditLikesInputModel input)
        {
            var entity = await this.blogPostsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            entity.Likes = input.Likes;

            await this.blogPostsRepository.SaveChangesAsync();

            return entity.Likes;
        }

        //public async Task<IEnumerable<CommentExportModel>> GetAllCommentsAsync(string blogPostId)
        //{
        //    var entities = await this.commentsRepository
        //        .AllAsNoTracking()
        //        .Where(x => x.BlogPostId == blogPostId)
        //        .To<CommentExportModel>()
        //        .ToListAsync();

        //    return entities;
        //}

        public async Task<BlogPostExportModel> AddCommentAsync(CommentInputModel input, string userId)
        {
            var entity = await this.blogPostsRepository
                                    .All()
                                    .Where(x => x.Id == input.BlogPostId)
                                    .Include(x => x.Comments)
                                    .FirstOrDefaultAsync();

            var comment = new Comment
            {
                BlogPostId = input.BlogPostId,
                Content = input.Content.Trim(),
                UserId = userId,
            };

            entity.Comments.Add(comment);

            await this.blogPostsRepository.SaveChangesAsync();

            return entity.To<BlogPostExportModel>();
        }

        public async Task<BlogPostExportModel> AddCommentLikeAsync(string blogPostId, int commentId)
        {
            var entity = await this.blogPostsRepository
                                    .All()
                                    .Where(x => x.Id == blogPostId)
                                    .Include(x => x.Comments)
                                    .FirstOrDefaultAsync();

            entity.Comments.Where(x => x.Id == commentId).FirstOrDefault().Likes++;

            await this.blogPostsRepository.SaveChangesAsync();

            return entity.To<BlogPostExportModel>();
        }

        public async Task<int> DeleteAsync(string id, string userId)
        {
            var entity = await this.blogPostsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            this.blogPostsRepository.Delete(entity);

            return await this.blogPostsRepository.SaveChangesAsync();
        }

        private async Task AddCategoriesAsync(BlogPost blogPost, IEnumerable<int> categories)
        {
            foreach (var category in categories)
            {
                var blgoPostCategory = await this.blogPostCategoriesRepository
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(x => x.BlogPostId == blogPost.Id
                                            && x.CategoryId == category);

                if (blgoPostCategory != null)
                {
                    // This category is already asigned to the blogPost
                    continue;
                }

                await this.blogPostCategoriesRepository.AddAsync(new BlogPostCategory
                {
                    BlogPostId = blogPost.Id,
                    CategoryId = category,
                });
            }

            await this.blogPostCategoriesRepository.SaveChangesAsync();
        }

        private async Task<Stream> UploadFileAsync(BlogPostCreateInputModel input, string imageFilesDirectory, BlogPost entity)
        {
            // upload to local wwwroot/images dir:
            Directory.CreateDirectory(imageFilesDirectory);
            var extension = Path.GetExtension(input.InputFile.FileName).ToLower().TrimStart('.');
            entity.ImageFileExtension = extension;
            var fileName = Path.GetFileName(input.InputFile.FileName).ToLower();
            entity.ImageFileName = fileName;
            var physicalPath = $"{imageFilesDirectory}/{fileName}.{extension}";
            Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await input.InputFile.CopyToAsync(fileStream);

            // upload to Azure Blob variant 2:
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants.AzureStorageBlobContainerNameImages);
            var blobClient = container.GetBlobClient($"{fileName}.{extension}");

            // fileStream pointer must be returned at its 0 byte, because it is at the last byte at the moment:
            fileStream.Position = 0;
            await blobClient.UploadAsync(fileStream);
            entity.ImageRemoteFileUrl = blobClient.Uri.AbsoluteUri;
            return fileStream;
        }
    }
}
