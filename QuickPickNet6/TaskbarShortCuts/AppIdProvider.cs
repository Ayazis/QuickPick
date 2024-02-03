using System;
using System.Runtime.InteropServices;
using System.Text;

public class AppIdProvider
{
    [DllImport("Shlwapi.dll", SetLastError = true)]
    static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra,[Out] StringBuilder pszOut, [In][Out] ref uint pcchOut);

    private enum AssocF
    {
        Init_NoRemapCLSID = 0x1,
        Init_ByExeName = 0x2,
        Open_ByExeName = 0x2,
        Init_DefaultToStar = 0x4,
        Init_DefaultToFolder = 0x8,
        NoUserSettings = 0x10,
        NoTruncate = 0x20,
        Verify = 0x40,
        RemapRunDll = 0x80,
        NoFixUps = 0x100,
        IgnoreBaseClass = 0x200
    }

    private enum AssocStr
    {
        AppId = 18
    }

    // check: https://stackoverflow.com/questions/46589834/get-appname-from-assocquerystring-c-sharp
    public string GetAppId(string filePath)
    {
        uint cchOut = 0;
        if (AssocQueryString(AssocF.Verify, AssocStr.AppId, filePath, null, null, ref cchOut) != 1)
            throw new InvalidOperationException("Could not determine associated string");

        StringBuilder pszOut = new StringBuilder((int)cchOut);
        if (AssocQueryString(AssocF.Verify, AssocStr.AppId, filePath, null, pszOut, ref cchOut) != 0)
            throw new InvalidOperationException("Could not determine associated string");

        return pszOut.ToString();
    }
}
