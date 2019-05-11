using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.VSCode
{
    public class VSCodeToolStripSettings
    {
        public VSCodeToolStripSettings(Form Form, ToolStrip Menu, VSCodeIconTheme IconTheme)
        {
            this.Form = Form;
            this.Menu = Menu;
            this.ControlBox = new VSCodeControlBox(IconTheme);
        }

        #region Properties

        public Form Form { get; }

        public ToolStrip Menu { get; }

        public VSCodeControlBox ControlBox { get; }

        #endregion
    }
}
