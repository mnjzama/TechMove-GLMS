/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPIClient
Date: [n.d]
Date Accessed: 16 May 2026
*/
namespace TechMoveClient.Models
{
    public class FirebaseErrorModel
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Error> errors { get; set; }
    }
}