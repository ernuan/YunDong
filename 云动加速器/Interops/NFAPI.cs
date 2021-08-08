using System.Runtime.InteropServices;

namespace CloudsMove.Interops
{
    public static class NFAPI
    {
        private const string nfapinet_bin = "\\bin\\nfapinet.dll";

        [DllImport(nfapinet_bin, CallingConvention = CallingConvention.Cdecl)]
        public static extern NF_STATUS nf_registerDriver(string driverName);

        [DllImport(nfapinet_bin, CallingConvention = CallingConvention.Cdecl)]
        public static extern NF_STATUS nf_unRegisterDriver(string driverName);
    }
}