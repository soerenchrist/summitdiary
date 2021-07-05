using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Countries.Dto;

namespace SummitDiary.Core.Endpoints.Countries.Commands
{
    public class CreateCountryCommand : IRequest<CountryDto>
    {
        public string Name { get; set; }
    }

    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCountryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<CountryDto> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Countries.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"Country named {request.Name} does already exist");

            var country = new Country
            {
                Name = request.Name
            };

            await _context.Countries.AddAsync(country, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CountryDto>(country);
        }
    }
}