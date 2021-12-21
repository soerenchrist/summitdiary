using Ardalis.Specification;

namespace SummitDiary.Core.Models.SummitAggregate.Specs;

public sealed class GetSummitsWithDiaryEntriesSpec : Specification<Summit>
{
    public GetSummitsWithDiaryEntriesSpec()
    {
        Query
            .Include(x => x.DiaryEntries)
            .Where(x => x.DiaryEntries!.Any());
    }
}