using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Activities.Dto;

namespace SummitDiary.Core.Endpoints.Activities.Commands
{
    public class CreateActivityCommand : IRequest<ActivityDto>
    {
        public string Title { get; set; } = string.Empty;
        public DateTime HikeDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int Rating { get; set; }
        public double ElevationUp { get; set; }
        public double ElevationDown { get; set; }
        public double Distance { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int Duration { get; set; }
        public List<int> SummitIds { get; set; } = new();
    }

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, ActivityDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateActivityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ActivityDto> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime? startTime = null;
            DateTime? endTime = null;
            request.HikeDate = request.HikeDate.Date;
            if (request.StartTime != null && request.EndTime != null)
            {
                startTime = ParseTime(request.StartTime, request.HikeDate);
                endTime = ParseTime(request.EndTime, request.HikeDate);

                if (startTime != null && endTime != null)
                {

                    if (endTime < startTime)
                        (startTime, endTime) = (endTime, startTime);

                    request.Duration = (int) (endTime - startTime).Value.TotalSeconds;
                }
            }

            var activity = new Activity
            {
                Title = request.Title,
                Distance = request.Distance,
                Duration = request.Duration,
                Notes = request.Notes,
                Rating = request.Rating,
                ElevationDown = request.ElevationDown,
                ElevationUp = request.ElevationUp,
                EndTime = endTime,
                StartTime = startTime,
                HikeDate = request.HikeDate,
                Summits = await GetSummits(request.SummitIds, cancellationToken)
            };

            await _context.Activities.AddAsync(activity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ActivityDto>(activity);
        }

        private DateTime? ParseTime(string time, DateTime date)
        {
            var parts = time.Split(":");
            if (parts.Length != 2)
                return null;

            if (!int.TryParse(parts[0], out var hour))
                return null;
            if (!int.TryParse(parts[1], out var minute))
                return null;
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
        }

        private async Task<List<Summit>> GetSummits(List<int> summitIDs, CancellationToken cancellationToken)
        {
            var summits = new List<Summit>();
            foreach (var summitId in summitIDs)
            {
                var summit = await _context.Summits.FirstOrDefaultAsync(x => x.Id == summitId, cancellationToken);
                if (summit == null)
                    throw new NotFoundException(nameof(Summit), summitId);
                
                summits.Add(summit);
            }

            return summits;
        }
    }
}