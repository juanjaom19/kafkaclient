namespace kafkaclient.web.Core.Dto;

public class FilterGroup
{
    public string PropertyName { get; set; }
    public object Value { get; set; }
    public string Operator { get; set; }
    public string LogicalOperator { get; set; } = "And";
    public int Group { get; set; } = 1;
    public string[]? MultipleProperties { get; set; } = null; 
}