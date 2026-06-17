/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPI
Date: [n.d]
Date Accessed: 16 May 2026
*/
namespace TechMoveAPI.Models
{
    public class FirebaseErrorModel
    {
        public FirebaseErrorDetails error { get; set; }
    }

    public class FirebaseErrorDetails
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<FirebaseErrorDetails> errors { get; set; }
    }
}