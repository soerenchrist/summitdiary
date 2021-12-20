using System.Collections.Immutable;
using System.Text;
using System.Xml;
using MediatR;
using NetTopologySuite.IO;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models.Common;
using SummitDiary.Core.Endpoints.Gpx.Dto;

namespace SummitDiary.Core.Endpoints.Gpx.Commands
{
    public class ExportGpxCommand : IRequest<ByteFileResult>
    {
        public List<PathPointDto> Points { get; set; } = new();
    }
    
    public class ExportGpxCommandHandler : IRequestHandler<ExportGpxCommand, ByteFileResult>
    {
        private readonly IElevationService _elevationService;

        public ExportGpxCommandHandler(IElevationService elevationService)
        {
            _elevationService = elevationService;
        }
        
        public async Task<ByteFileResult> Handle(ExportGpxCommand request, CancellationToken cancellationToken)
        {
            var gpxWaypoints = new List<GpxWaypoint>();

            foreach (var waypoint in request.Points)
            {
                var elevation = await _elevationService.GetElevation(waypoint.Latitude, waypoint.Longitude, cancellationToken);
                
                gpxWaypoints.Add(new GpxWaypoint(new GpxLongitude(waypoint.Longitude),
                    new GpxLatitude(waypoint.Latitude), elevation));
            }
            
            GpxFile file = new GpxFile();
            file.Metadata = BuildMetadata();
            file.Tracks.AddRange(BuildTracks(gpxWaypoints));

            
            var memoryStream = new MemoryStream();
            using (var xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
            {
                file.WriteTo(xmlWriter, new GpxWriterSettings
                {
                    TimeZoneInfo = TimeZoneInfo.Local
                });
            }
            var bytes = memoryStream.ToArray();
            return new ByteFileResult(bytes, "application/xml+gpx", "Tour.gpx");
        }

        private List<GpxTrack> BuildTracks(IEnumerable<GpxWaypoint> gpxWaypoints)
        {
            var track = new GpxTrack("", "", "", "", ImmutableArray<GpxWebLink>.Empty, 1, "", null,
                BuildSegments(gpxWaypoints));

            return new List<GpxTrack>
            {
                track
            };
        }

        private ImmutableArray<GpxTrackSegment> BuildSegments(IEnumerable<GpxWaypoint> gpxWaypoints)
        {
            var segment = new GpxTrackSegment(new ImmutableGpxWaypointTable(gpxWaypoints), null);
            return ImmutableArray.Create(segment);
        }

        private GpxMetadata BuildMetadata()
        {
            var name = "GipfelStürmer";
            return new GpxMetadata(name, "Tour", "", null, new GpxCopyright(name),
                ImmutableArray<GpxWebLink>.Empty, DateTime.Now.ToUniversalTime(), "gpx", null, null);
        }
    }
}