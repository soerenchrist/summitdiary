namespace SummitDiary.SharedKernel
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}