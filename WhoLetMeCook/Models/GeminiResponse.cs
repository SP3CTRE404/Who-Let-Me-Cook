// These classes are designed to match the JSON structure of the Gemini API response.
public class GeminiResponse
{
    public Candidate[] candidates { get; set; }
}

public class Candidate
{
    public Content content { get; set; }
}

public class Content
{
    public Part[] parts { get; set; }
    public string role { get; set; }
}

public class Part
{
    public string text { get; set; }
}
