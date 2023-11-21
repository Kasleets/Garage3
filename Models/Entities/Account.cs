namespace Garage3.Models.Entities
{
    public class Account
    {
        public int AccountID { get; set; }
        public int MemberID { get; set; }

        // Navigation property
        public virtual Member Member { get; set; }
    }

}
