using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

namespace Follettorobotics.HappyBones.Booth.Gen2
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }

        private void frm_Login_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void frm_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void frm_Login_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        public void InitializeForm()
        {
            SetControlsProperties();

            SetControlsText();
        }

        public void SetControlsProperties()
        {
            GlobalFunction.SetControlsProperties(GlobalFunction.GetControls(this));
        }

        public void SetControlsText()
        {
            GlobalFunction.SetControlsText(GlobalFunction.GetControls(this));
        }

        private void Login_PW_DoubleClick(object sender, EventArgs e)
        {
            txt_Login_PW.Text = CONST.S_PW;
        }

        private void btn_Login_OK_Click(object sender, EventArgs e)
        {
            if (txt_Login_PW.Text == CONST.S_PW)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetMessage(e_Message.Login_PW));
            }
        }

        private void btn_Login_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
