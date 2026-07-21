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
