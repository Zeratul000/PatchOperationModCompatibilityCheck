using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ModCompatCheck
{
    internal class ModCheck
    {
        private StringBuilder text;
        public ModCheck()
        {
            text = new StringBuilder();
        }

        public string GetLog()
        {
            return text.ToString();
        }

        public bool CheckMods(string OperationName, bool? DebugState, string SingleModName, List<string> MultipleModNames)
        {
            bool check = false, single, multiple;

            single = !(string.IsNullOrEmpty(SingleModName) || SingleModName.Trim().Length == 0);
            multiple = !(MultipleModNames.NullOrEmpty());

            if (multiple)
            {
                MultipleModNames.Sort();
            }

            this.LogPatchInformation(OperationName, DebugState, SingleModName, MultipleModNames);

            if (single && multiple)
            {
                this.text.Append("\nThe operation may not have both the fields ModName and ModList as valid!");
            }

            if (single)
            {
                this.text.Append("\nThe the field ModName is valid!");
                check = this.CheckSingleModName(SingleModName);
            }

            if (multiple)
            {
                this.text.Append("\nThe the field ModList is valid!");
                check = this.CheckMultipleModNames(MultipleModNames);
            }

            if (!(single || multiple))
            {
                this.text.Append("\nThe operation may not have both the fields ModName and ModList as not valid!");
            }

            this.text.AppendFormat("\nThe final result is thus {0}.\n", check ? "SUCCESS" : "FAILURE");

            this.text.Append("\nMods in Load Order : \n");
            foreach (ModMetaData mod in ModsConfig.ActiveModsInLoadOrder)
            {
                this.text.AppendFormat(" - {0}\n", mod.Name);
            }

            return check;
        }

        private bool CheckMultipleModNames(List<string> mods)
        {
            int count = 0;

            foreach (string name in mods)
            {
                if (this.CheckSingleModName(name))
                {
                    count++;
                }
            }

            if(count == mods.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckSingleModName(string name)
        {
            if (ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.Name.Equals(name)))
            {
                this.text.AppendFormat("\nFOUND : The mod {0} has been found in the loadorder of active mods.", name);
                return true;
            }
            else
            {
                this.text.AppendFormat("\nERROR : The mod {0} has not been found in the loadorder of active mods.", name);
                return false;
            }
        }

        private void LogPatchInformation(string operationName, bool? debugState, string singleModName, List<string> multipleModNames)
        {
            this.text.AppendLine("<Operation Class=\"ModCompatCheck.PatchOperationModCompatabilityCheck\">");

            if (string.IsNullOrEmpty(operationName) || operationName.Trim().Length == 0)
            {
                this.text.Append("\t<Name />\n");
            }
            else
            {
                this.text.AppendFormat("\t<Name>{0}</Name>\n", operationName);
            }

            if (debugState.HasValue)
            {
                this.text.AppendFormat("\t<Debug>{0}</Debug>\n", debugState.GetValueOrDefault(false));
            }
            else
            {
                this.text.Append("\t<Debug />\n");
            }

            if (string.IsNullOrEmpty(singleModName) || singleModName.Trim().Length == 0)
            {
                this.text.Append("\t<ModName />\n");
            }
            else
            {
                this.text.AppendFormat("\t<ModName>{0}</ModName>\n", singleModName);
            }

            if (multipleModNames.NullOrEmpty())
            {
                this.text.Append("\t<ModList />\n");
            }
            else
            {
                this.text.Append("\t<ModList>\n");

                foreach (string mod in multipleModNames)
                {
                    this.text.AppendFormat("\t\t<li>{0}</li>\n", mod);
                }

                this.text.Append("\t</ModList>\n");
            }

            this.text.AppendLine("</Operation>");
        }
    }
}
