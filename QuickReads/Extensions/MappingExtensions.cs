using QuickReads.Entities;
using QuickReads.Entities.DTOs;

namespace QuickReads.Extensions;

public static class MappingExtensions
{
    public static ArticleDto ToDto(this Article article)
    {
        return new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            IsDeleted = article.IsDeleted,
            Author = article.Author,
            ImageUrl = article.ImageUrl,
            CopyrightDetail = article.CopyrightDetail,
            WordCount = article.WordCount,
            ReadTimeInMinute = article.ReadTimeInMinute,
            PublishedDate = article.PublishedDate,
            Language = article.Language,
            Tags = article.Tags.Select(tag => tag.ToDto()).ToList(),
        };
    }
    
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
            IsDeleted = tag.IsDeleted,
            Name = tag.Name
        };
    }
}