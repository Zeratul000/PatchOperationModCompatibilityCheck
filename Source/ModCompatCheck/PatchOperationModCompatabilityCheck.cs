using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Verse;

namespace ModCompatCheck
{
    public class PatchOperationModCompatabilityCheck : PatchOperation
    {
        private string Name;
        private bool? Debug;
        private string ModName;
        private List<string> ModList;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            bool check;
            ModCheck Checker = new ModCheck();

            check = Checker.CheckMods(Name, Debug, ModName, ModList);

            if (Debug.GetValueOrDefault(false))
            {
                Log.Message(string.Format("ModCompatCheck \"{0}\" debug log ({1} lines) :\n{2}", Name, Checker.GetLog().Count((arg) => arg == '\n'), Checker.GetLog()));
            }

            return check;
        }
    }
}
