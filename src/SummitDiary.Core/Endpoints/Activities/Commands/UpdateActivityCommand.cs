using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Exceptions;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Activities.Dto;

namespace SummitDiary.Core.Endpoints.Activities.Commands
{
    public class UpdateActivityCommand : IRequest<ActivityDto>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime HikeDate { get; set; }
        public string Notes { get; set; }
        public int Rating { get; set; }
        public double ElevationUp { get; set; }
        public double ElevationDown { get; set; }
        public double Distance { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        public List<int> SummitIds { get; set; }
    }
    
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, ActivityDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateActivityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ActivityDto> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
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

            var existing = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (existing == null)
                throw new NotFoundException(nameof(Activity), request.Id);

            existing.Distance = request.Distance;
            existing.Duration = request.Duration;
            existing.Notes = request.Notes;
            existing.Rating = request.Rating;
            existing.ElevationDown = request.ElevationDown;
            existing.ElevationUp = request.ElevationUp;
            existing.EndTime = endTime;
            existing.StartTime = endTime;
            existing.HikeDate = request.HikeDate;
            existing.Title = request.Title;
            
            _context.Activities.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ActivityDto>(existing);
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
    }
}