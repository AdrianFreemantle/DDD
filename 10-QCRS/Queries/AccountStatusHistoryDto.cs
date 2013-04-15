using PersistenceModel.Reporting;

namespace Queries
{
    public class AccountStatusHistoryDto
    {
        public string AccountStatus { get; set; }
        public string ChangedDate { get; set; }

        public static AccountStatusHistoryDto BuildFromModel(AccountStatusHistoryView view)
        {
            return new AccountStatusHistoryDto
            {
                AccountStatus = view.AccountStatus.Description,
                ChangedDate = view.ChangedDate.ToShortDateString()
            };
        }
    }
}