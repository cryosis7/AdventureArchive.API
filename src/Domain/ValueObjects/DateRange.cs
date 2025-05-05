namespace AdventureArchive.Api.Domain.ValueObjects;

public class DateRange : ValueObject
{
    public DateTime Start { get; }
    public DateTime End { get; private set; }

    public TimeSpan Duration
    {
        get => End - Start;
        set => End = Start + value;
    }

    public DateRange(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be less than or equal to end date.");
        }

        Start = start;
        End = end;
    }
    
    public DateRange(DateTime start, TimeSpan duration)
    {
        Start = start;
        End = start + duration;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Start;
        yield return End;
    }

    public bool Contains(DateTime date)
    {
        return date >= Start && date <= End;
    }
    
    public bool ContainsDay(DateOnly date)
    {
        return date >= DateOnly.FromDateTime(Start) && date <= DateOnly.FromDateTime(End);
    }
    
    public bool Overlaps(DateRange other)
    {
        return Start < other.End && End > other.Start;
    }

    public int GetNumberOfDays()
    {
        return (End - Start).Days + 1;
    }
}