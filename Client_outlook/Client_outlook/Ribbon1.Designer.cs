namespace Client_outlook
{
    partial class MainContainerRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MainContainerRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Rssfeed = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.EmailBox = this.Factory.CreateRibbonEditBox();
            this.PasswordBox = this.Factory.CreateRibbonEditBox();
            this.button1 = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.URLBox = this.Factory.CreateRibbonEditBox();
            this.button2 = this.Factory.CreateRibbonButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.group3 = this.Factory.CreateRibbonGroup();
            this.ShowRegisterBox = this.Factory.CreateRibbonButton();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.EmailRegisterBox = this.Factory.CreateRibbonEditBox();
            this.UserNameBox = this.Factory.CreateRibbonEditBox();
            this.PasswordRegisterBox = this.Factory.CreateRibbonEditBox();
            this.Register = this.Factory.CreateRibbonButton();
            this.ChangePassword = this.Factory.CreateRibbonButton();
            this.ConfirmPasswordBox = this.Factory.CreateRibbonEditBox();
            this.NewPasswordBox = this.Factory.CreateRibbonEditBox();
            this.CurrentPasswordBox = this.Factory.CreateRibbonEditBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.Rssfeed.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            // 
            // Rssfeed
            // 
            this.Rssfeed.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Rssfeed.Groups.Add(this.group1);
            this.Rssfeed.Groups.Add(this.group2);
            this.Rssfeed.Groups.Add(this.group3);
            this.Rssfeed.Groups.Add(this.group4);
            this.Rssfeed.Label = "RSS Feed";
            this.Rssfeed.Name = "Rssfeed";
            // 
            // group1
            // 
            this.group1.Items.Add(this.EmailBox);
            this.group1.Items.Add(this.PasswordBox);
            this.group1.Items.Add(this.button1);
            this.group1.Label = "Connection";
            this.group1.Name = "group1";
            // 
            // EmailBox
            // 
            this.EmailBox.Label = "Email";
            this.EmailBox.Name = "EmailBox";
            this.EmailBox.Text = null;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Label = "Password";
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Text = null;
            // 
            // button1
            // 
            this.button1.Label = "Connect";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.URLBox);
            this.group2.Items.Add(this.button2);
            this.group2.Label = "Feed";
            this.group2.Name = "group2";
            // 
            // URLBox
            // 
            this.URLBox.Label = "URL";
            this.URLBox.Name = "URLBox";
            this.URLBox.Text = null;
            // 
            // button2
            // 
            this.button2.Label = "Add Feed";
            this.button2.Name = "button2";
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.ShowRegisterBox);
            this.group3.Label = "More";
            this.group3.Name = "group3";
            // 
            // ShowRegisterBox
            // 
            this.ShowRegisterBox.Label = "Show Admin Forms";
            this.ShowRegisterBox.Name = "ShowRegisterBox";
            this.ShowRegisterBox.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ShowRegisterBox_Click);
            // 
            // group4
            // 
            this.group4.Items.Add(this.UserNameBox);
            this.group4.Items.Add(this.EmailRegisterBox);
            this.group4.Items.Add(this.PasswordRegisterBox);
            this.group4.Items.Add(this.Register);
            this.group4.Items.Add(this.separator1);
            this.group4.Items.Add(this.CurrentPasswordBox);
            this.group4.Items.Add(this.NewPasswordBox);
            this.group4.Items.Add(this.ConfirmPasswordBox);
            this.group4.Items.Add(this.ChangePassword);
            this.group4.Label = "Administration";
            this.group4.Name = "group4";
            this.group4.Visible = false;
            // 
            // EmailRegisterBox
            // 
            this.EmailRegisterBox.Label = "Email";
            this.EmailRegisterBox.Name = "EmailRegisterBox";
            // 
            // UserNameBox
            // 
            this.UserNameBox.Label = "UserName";
            this.UserNameBox.Name = "UserNameBox";
            // 
            // PasswordRegisterBox
            // 
            this.PasswordRegisterBox.Label = "Password";
            this.PasswordRegisterBox.Name = "PasswordRegisterBox";
            // 
            // Register
            // 
            this.Register.Label = "Register";
            this.Register.Name = "Register";
            this.Register.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Register_Click);
            // 
            // ChangePassword
            // 
            this.ChangePassword.Label = "Change Password";
            this.ChangePassword.Name = "ChangePassword";
            this.ChangePassword.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ChangePassword_Click);
            // 
            // ConfirmPasswordBox
            // 
            this.ConfirmPasswordBox.Label = "Confirm Password";
            this.ConfirmPasswordBox.Name = "ConfirmPasswordBox";
            // 
            // NewPasswordBox
            // 
            this.NewPasswordBox.Label = "New Password";
            this.NewPasswordBox.Name = "NewPasswordBox";
            // 
            // CurrentPasswordBox
            // 
            this.CurrentPasswordBox.Label = "Current Password";
            this.CurrentPasswordBox.Name = "CurrentPasswordBox";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // MainContainerRibbon
            // 
            this.Name = "MainContainerRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.Rssfeed);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.Rssfeed.ResumeLayout(false);
            this.Rssfeed.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Rssfeed;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox EmailBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox PasswordBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox URLBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
        private System.Windows.Forms.BindingSource bindingSource1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ShowRegisterBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox EmailRegisterBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox PasswordRegisterBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Register;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox UserNameBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox CurrentPasswordBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox NewPasswordBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox ConfirmPasswordBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ChangePassword;
    }

    partial class ThisRibbonCollection
    {
        internal MainContainerRibbon Ribbon1
        {
            get { return this.GetRibbon<MainContainerRibbon>(); }
        }
    }
}
