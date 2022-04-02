namespace CuriousReadersData
{
    public static class Enumerators
    {
        public enum BookStatus
        {
            Enabled = 0,
            Disabled = 1,
            Deleted = 2,
        };

        public enum ReservationStatus
        {
            PendingReservationApproval = 0,
            Reserved = 1,
            Borrowed = 2,
            Rejected = 3,
            Returned = 4,
            PendingProlongationApproval = 5,
        };
    }
}
