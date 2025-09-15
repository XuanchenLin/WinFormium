using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormium
{
    public partial class Formium : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            HostWindow.Dispose();
            GC.SuppressFinalize(this);

        }
    }
}
