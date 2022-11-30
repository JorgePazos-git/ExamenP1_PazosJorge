namespace ExamenP1_PazosJorge.Detect_Objetcs
{
    public class detect_response
    {
        public @object[] objects { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
        public string modelVersion { get; set; }
    }
}
