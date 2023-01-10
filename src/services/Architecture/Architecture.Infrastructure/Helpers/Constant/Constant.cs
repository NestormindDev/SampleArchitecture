using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.Infrastructure.Helpers
{
    /// <summary>
    /// This class has all the constants used across the code namespace.
    /// </summary>
    public static class GenericConstant
    {
        #region --Claim--

        public const string CLAIM_PERMISSION = @"Permission";

        /// <summary>
        /// Claim key for the current review cycle id.
        /// </summary>
        public const string CLAIM_CURRENT_REVIEW_CYCLE_ID = @"CurrentReviewCycleId";

        /// <summary>
        /// Claim key for the open review period's cycle id.
        /// </summary>
        public const string CLAIM_OPEN_REVIEW_PERIOD_REVIEW_CYCLE_ID = @"OpenReviewPeriodReviewCycleId";

        public const string CLAIM_EMPLOYEE_ID = @"EmployeeId";
        public const string CLAIM_FIRST_NAME = @"FirstName";
        public const string CLAIM_LAST_NAME = @"LastName";
        public const string CLAIM_PREFIX = @"Prefix";
        public const string CLAIM_SUFFIX = @"Suffix";
        public const string CLAIM_MIDDLE_NAME = @"MiddleName";
        public const string CLAIM_ON_PROBATION = @"OnProbation";
        #endregion --Claim--

        #region --Authorization policy--
        public const string POLICY_VIEW_DASHBOARD = @"PolicyViewDashboard";
        public const string POLICY_MANAGE_MY_REVIEW = @"PolicyManageMyReview";
        public const string POLICY_RECEIVE_REVIEW_REQUEST = @"PolicyReceiveReviewRequest";
        public const string POLICY_MANAGE_SUBORDINATE_REVIEW = @"PolicyManageSubordinateReview";
        public const string POLICY_REQUEST_REVIEW = @"PolicyRequestReview";
        public const string POLICY_EDIT_REVIEW = @"PolicyEditReview";
        public const string POLICY_VIEW_REVIEW = @"PolicyViewReview";
        public const string POLICY_VIEW_OWN_PEER_REVIEW = @"PolicyViewOwnPeerReview";
        #endregion --Authorization policy--

        #region --Permission Code--
        public const string PERMISSION_CODE_VIEW_DASHBOARD = @"VIEW_DASHBOARD";
        public const string PERMISSION_CODE_MANAGE_MY_REVIEW = @"MANAGE_MY_REVIEW";
        public const string PERMISSION_CODE_RECEIVE_REVIEW_REQUEST = @"RECEIVE_REVIEW_REQUEST";
        public const string PERMISSION_CODE_MANAGE_SUBORDINATE_REVIEW = @"MANAGE_SUBORDINATE_REVIEW";
        public const string PERMISSION_CODE_REQUEST_REVIEW = @"REQUEST_REVIEW";
        public const string PERMISSION_CODE_EDIT_REVIEW = @"EDIT_REVIEW";
        public const string PERMISSION_CODE_VIEW_REVIEW = @"VIEW_REVIEW";
        public const string PERMISSION_CODE_VIEW_OWN_PEER_REVIEW = @"VIEW_OWN_PEER_REVIEW";
        #endregion --Permission Code--
    }
}
