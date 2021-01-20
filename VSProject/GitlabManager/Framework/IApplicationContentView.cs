using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitlabManager.Enums;

namespace GitlabManager.Framework
{
    public interface IApplicationContentView
    {
        string PageName { get; }

        AppNavigationSection Section { get; }

        bool IsLoading { get; set; }

        Task Init();
    }
}
