namespace MultiDownloader.TelegramHost.Models.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldUser">User we have in db</param>
        /// <param name="user">User that sends update</param>
        /// <returns>true if user changes</returns>
        public static bool IsChanged(this User oldUser, User user) =>
            oldUser.Username != user.Username ||
            oldUser.FirstName != user.FirstName ||
            oldUser.LastName != user.LastName ||
            oldUser.LastActivityDate.Date != DateTime.Today;
    }
}
