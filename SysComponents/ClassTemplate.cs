#region References
using System;
using System.Reflection;
#endregion References

namespace SysComponents
{
    public class ClassTemplate
    {
        # region Data
        # endregion Data

        # region Properties
        # endregion Properties

        # region Constructors
        # endregion Constructors

        # region Methods
        public void GenericMethod()
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod()); ;
            try
            {

            }
            catch (Exception e) { CLog.Exception(Module, e); }
        }
        # endregion Methods
    }
 }
