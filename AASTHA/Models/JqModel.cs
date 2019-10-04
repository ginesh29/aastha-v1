using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASTHA.Models
{
    public class JqModel
    {
        /// <summary>
        /// The number of pages which should be displayed in the paging controls at the bottom of the grid.
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// The current page number which should be highlighted in the paging controls at the bottom of the grid.
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// The total number of records in the entire data set, not just the portion returned in Rows.
        /// </summary>
        public int Records { get; set; }
        /// <summary>
        /// The data that will actually be displayed in the grid.
        /// </summary>
        public IEnumerable Rows { get; set; }
        /// <summary>
        /// Arbitrary data to be returned to the grid along with the row data. Leave null if not using. Must be serializable to JSON!
        /// </summary>
        public object UserData { get; set; }
    }
}