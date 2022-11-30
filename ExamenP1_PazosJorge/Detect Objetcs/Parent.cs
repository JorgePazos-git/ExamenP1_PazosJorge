namespace ExamenP1_PazosJorge.Detect_Objetcs
{
    public class Parent
    {
        public string Object { get; set; }
        public string confidence { get; set; }
        public Parent parent { get; set; }
    }
}
