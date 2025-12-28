namespace SchoolHealthSystem.Models
{
    public enum VaccinationStatus
    {
        Scheduled = 0,    // Đã lên lịch
        Completed = 1,    // Đã tiêm xong
        Cancelled = 2,    // Đã hủy
        Postponed = 3,    // Hoãn lại
        InProgress = 4    // Đang tiêm (cho trường hợp vaccine nhiều liều)
    }
}
