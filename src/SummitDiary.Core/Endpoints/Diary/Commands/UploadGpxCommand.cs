using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Endpoints.Diary.Commands
{
    public class UploadGpxCommand : IRequest
    {
        public IFormFile File { get; set; }
        public int ActivityId { get; set; }
    }

    public class UploadGpxCommandHandler : IRequestHandler<UploadGpxCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UploadGpxCommandHandler(IApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<Unit> Handle(UploadGpxCommand request, CancellationToken cancellationToken)
        {
            var activity =
                await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);

            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.ActivityId);
            
            var basePath = _configuration.GetValue("Files:Gpx:Path", "files/gpx");
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            
            var originalFilename = request.File.FileName;

            var newFilename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".gpx";
            var path = Path.Combine(basePath, newFilename);

            var fileStream = File.Create(path);
            await request.File.CopyToAsync(fileStream, cancellationToken);
            fileStream.Close();

            var attachment = new Attachment
            {
                ActivityId = request.ActivityId,
                FileName = originalFilename,
                FilePath = path,
                FileType = FileType.Gpx
            };
            await _context.Attachments.AddAsync(attachment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}