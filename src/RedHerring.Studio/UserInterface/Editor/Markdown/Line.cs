namespace RedHerring.Studio.UserInterface.Editor.Markdown;

internal struct Line
{
    public bool IsHeading;
    public bool IsEmphasis;
    public bool IsUnorderedListStart;
    public bool IsLeadingSpace;
    public int LeadingSpaceCount;
    public int HeadingCount;
    public int EmphasisCount;
    public int LineStart;
    public int LineEnd;
    public int LastRenderPosition;
}