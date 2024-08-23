﻿using FluentValidation;

namespace Freelancers.Core.Contracts.Projects;
public class ProjectRequestValidator : AbstractValidator<ProjectRequest>
{
    public ProjectRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .Length(5, 100)
            .WithMessage("Title must be between 5 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .Length(5, 1000)
            .WithMessage("Description must be between 5 and 1000 characters");

        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("Summary is required")
            .Length(5, 500)
            .WithMessage("Summary must be between 5 and 200 characters");

        RuleFor(x => x.ImageFile)
            .NotNull()
            .WithMessage("Image is required")
            .Must(x => IsValidImageExtension(x.FileName))
            .WithMessage("Image must be a valid image file (jpg, jpeg, png)")
            .Must(x => x.Length < (2 * 1024 * 1024))
            .WithMessage("Image must be less than 2MB");

        RuleFor(x => x.ProjectUrl)
            .NotEmpty()
            .WithMessage("ProjectUrl is required")
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("ProjectUrl must be a valid URL");

        RuleFor(x => x.VideoFile)
            .Must(x => x is null || IsValidVideoExtension(x.FileName))
            .WithMessage("Video must be a valid video file (mp4, avi, mov, mkv)")
            .Must(x => x is null || x.Length < (50 * 1024 * 1024))
            .WithMessage("Video must be less than 50MB");

        RuleFor(x => x.Technologies)
            .NotEmpty()
            .Must(x => x.Count > 5 && x.Distinct().Count() == x.Count)
            .WithMessage("Technologies must be more than 5");

        RuleFor(x => x.Freelancer)
            .NotEmpty()
            .Must(x => x.Count > 1)
            .WithMessage("Freelancer must be more than 1");

        RuleFor(x => x.SubImages)
            .Must(x => x is null || x.All(y => IsValidImageExtension(y.FileName)))
            .WithMessage("SubImages must be valid image files (jpg, jpeg, png)")
            .Must(x => x is null || x.All(y => y.Length < (2 * 1024 * 1024)))
            .WithMessage("SubImages must be less than 2MB");
    }

    private bool IsValidVideoExtension(string fileName)
    {
        var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
        var extension = Path.GetExtension(fileName)?.ToLower();
        return allowedExtensions.Contains(extension);
    }

    private bool IsValidImageExtension(string fileName)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(fileName)?.ToLower();
        return allowedExtensions.Contains(extension);
    }
}