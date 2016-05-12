namespace Dispatcher
{
    partial class DispatcherForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripDispatcher = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemClient = new System.Windows.Forms.ToolStripMenuItem();
            this.ajouterClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifierClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemIntervention = new System.Windows.Forms.ToolStripMenuItem();
            this.ajouterToolStripMenuItemIntervention = new System.Windows.Forms.ToolStripMenuItem();
            this.supprimerToolStripMenuItemIntervention = new System.Windows.Forms.ToolStripMenuItem();
            this.modifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aperçuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envoiSMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionMatérielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajouterMaterielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifierMatérielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.affecterMaterielAUnTechnicienToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TechnicienToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAjoutTechnicienToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifierTechnicienToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListBoxTechniciens = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.MapMain = new GMap.NET.WindowsForms.GMapControl();
            this.timerRafraichissementPositionTechnicien = new System.Windows.Forms.Timer(this.components);
            this.ListBoxClients = new System.Windows.Forms.ListBox();
            this.menuStripDispatcher.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripDispatcher
            // 
            this.menuStripDispatcher.Enabled = false;
            this.menuStripDispatcher.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClient,
            this.toolStripMenuItemIntervention,
            this.envoiSMSToolStripMenuItem,
            this.gestionMatérielToolStripMenuItem,
            this.TechnicienToolStripMenuItem});
            this.menuStripDispatcher.Location = new System.Drawing.Point(0, 0);
            this.menuStripDispatcher.Name = "menuStripDispatcher";
            this.menuStripDispatcher.Size = new System.Drawing.Size(804, 24);
            this.menuStripDispatcher.TabIndex = 0;
            this.menuStripDispatcher.Text = "menuStripDispatcher";
            // 
            // toolStripMenuItemClient
            // 
            this.toolStripMenuItemClient.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajouterClientToolStripMenuItem,
            this.modifierClientToolStripMenuItem});
            this.toolStripMenuItemClient.Enabled = false;
            this.toolStripMenuItemClient.Name = "toolStripMenuItemClient";
            this.toolStripMenuItemClient.Size = new System.Drawing.Size(93, 20);
            this.toolStripMenuItemClient.Text = "Gestion Client";
            // 
            // ajouterClientToolStripMenuItem
            // 
            this.ajouterClientToolStripMenuItem.Name = "ajouterClientToolStripMenuItem";
            this.ajouterClientToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ajouterClientToolStripMenuItem.Text = "Ajouter";
            this.ajouterClientToolStripMenuItem.Click += new System.EventHandler(this.ajouterClientToolStripMenuItem_Click);
            // 
            // modifierClientToolStripMenuItem
            // 
            this.modifierClientToolStripMenuItem.Name = "modifierClientToolStripMenuItem";
            this.modifierClientToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.modifierClientToolStripMenuItem.Text = "Modifier/Supprimer";
            this.modifierClientToolStripMenuItem.Click += new System.EventHandler(this.modifierClientToolStripMenuItem_Click);
            // 
            // toolStripMenuItemIntervention
            // 
            this.toolStripMenuItemIntervention.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajouterToolStripMenuItemIntervention,
            this.supprimerToolStripMenuItemIntervention,
            this.modifierToolStripMenuItem,
            this.aperçuToolStripMenuItem});
            this.toolStripMenuItemIntervention.Enabled = false;
            this.toolStripMenuItemIntervention.Name = "toolStripMenuItemIntervention";
            this.toolStripMenuItemIntervention.Size = new System.Drawing.Size(134, 20);
            this.toolStripMenuItemIntervention.Text = "Gestion  interventions";
            // 
            // ajouterToolStripMenuItemIntervention
            // 
            this.ajouterToolStripMenuItemIntervention.Enabled = false;
            this.ajouterToolStripMenuItemIntervention.Name = "ajouterToolStripMenuItemIntervention";
            this.ajouterToolStripMenuItemIntervention.Size = new System.Drawing.Size(129, 22);
            this.ajouterToolStripMenuItemIntervention.Text = "Ajouter";
            this.ajouterToolStripMenuItemIntervention.Click += new System.EventHandler(this.ajouterToolStripMenuItemIntervention_Click);
            // 
            // supprimerToolStripMenuItemIntervention
            // 
            this.supprimerToolStripMenuItemIntervention.Enabled = false;
            this.supprimerToolStripMenuItemIntervention.Name = "supprimerToolStripMenuItemIntervention";
            this.supprimerToolStripMenuItemIntervention.Size = new System.Drawing.Size(129, 22);
            this.supprimerToolStripMenuItemIntervention.Text = "Supprimer";
            this.supprimerToolStripMenuItemIntervention.Click += new System.EventHandler(this.supprimerToolStripMenuItemIntervention_Click);
            // 
            // modifierToolStripMenuItem
            // 
            this.modifierToolStripMenuItem.Enabled = false;
            this.modifierToolStripMenuItem.Name = "modifierToolStripMenuItem";
            this.modifierToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.modifierToolStripMenuItem.Text = "Modifier";
            this.modifierToolStripMenuItem.Click += new System.EventHandler(this.modifierToolStripMenuItem_Click);
            // 
            // aperçuToolStripMenuItem
            // 
            this.aperçuToolStripMenuItem.Enabled = false;
            this.aperçuToolStripMenuItem.Name = "aperçuToolStripMenuItem";
            this.aperçuToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.aperçuToolStripMenuItem.Text = "Aperçu";
            this.aperçuToolStripMenuItem.Click += new System.EventHandler(this.aperçuToolStripMenuItem_Click);
            // 
            // envoiSMSToolStripMenuItem
            // 
            this.envoiSMSToolStripMenuItem.Enabled = false;
            this.envoiSMSToolStripMenuItem.Name = "envoiSMSToolStripMenuItem";
            this.envoiSMSToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.envoiSMSToolStripMenuItem.Text = "Envoi SMS";
            this.envoiSMSToolStripMenuItem.Click += new System.EventHandler(this.envoiSMSToolStripMenuItem_Click);
            // 
            // gestionMatérielToolStripMenuItem
            // 
            this.gestionMatérielToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajouterMaterielToolStripMenuItem,
            this.modifierMatérielToolStripMenuItem,
            this.affecterMaterielAUnTechnicienToolStripMenuItem});
            this.gestionMatérielToolStripMenuItem.Enabled = false;
            this.gestionMatérielToolStripMenuItem.Name = "gestionMatérielToolStripMenuItem";
            this.gestionMatérielToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.gestionMatérielToolStripMenuItem.Text = "Gestion Matériel";
            // 
            // ajouterMaterielToolStripMenuItem
            // 
            this.ajouterMaterielToolStripMenuItem.Name = "ajouterMaterielToolStripMenuItem";
            this.ajouterMaterielToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.ajouterMaterielToolStripMenuItem.Text = "Ajouter";
            this.ajouterMaterielToolStripMenuItem.Click += new System.EventHandler(this.ajouterMaterielToolStripMenuItem_Click);
            // 
            // modifierMatérielToolStripMenuItem
            // 
            this.modifierMatérielToolStripMenuItem.Name = "modifierMatérielToolStripMenuItem";
            this.modifierMatérielToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.modifierMatérielToolStripMenuItem.Text = "Modifier";
            this.modifierMatérielToolStripMenuItem.Click += new System.EventHandler(this.modifierMatérielToolStripMenuItem_Click);
            // 
            // affecterMaterielAUnTechnicienToolStripMenuItem
            // 
            this.affecterMaterielAUnTechnicienToolStripMenuItem.Name = "affecterMaterielAUnTechnicienToolStripMenuItem";
            this.affecterMaterielAUnTechnicienToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.affecterMaterielAUnTechnicienToolStripMenuItem.Text = "Affecter matériel";
            this.affecterMaterielAUnTechnicienToolStripMenuItem.Click += new System.EventHandler(this.affecterMaterielAUnTechnicienToolStripMenuItem_Click);
            // 
            // TechnicienToolStripMenuItem
            // 
            this.TechnicienToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mAjoutTechnicienToolStripMenuItem,
            this.modifierTechnicienToolStripMenuItem});
            this.TechnicienToolStripMenuItem.Enabled = false;
            this.TechnicienToolStripMenuItem.Name = "TechnicienToolStripMenuItem";
            this.TechnicienToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
            this.TechnicienToolStripMenuItem.Text = "Gestion techniciens";
            // 
            // mAjoutTechnicienToolStripMenuItem
            // 
            this.mAjoutTechnicienToolStripMenuItem.Name = "mAjoutTechnicienToolStripMenuItem";
            this.mAjoutTechnicienToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.mAjoutTechnicienToolStripMenuItem.Text = "Ajouter technicien";
            this.mAjoutTechnicienToolStripMenuItem.Click += new System.EventHandler(this.mAjoutTechnicienToolStripMenuItem_Click);
            // 
            // modifierTechnicienToolStripMenuItem
            // 
            this.modifierTechnicienToolStripMenuItem.Name = "modifierTechnicienToolStripMenuItem";
            this.modifierTechnicienToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.modifierTechnicienToolStripMenuItem.Text = "Modifier";
            this.modifierTechnicienToolStripMenuItem.Click += new System.EventHandler(this.modifierTechnicienToolStripMenuItem_Click);
            // 
            // ListBoxTechniciens
            // 
            this.ListBoxTechniciens.FormattingEnabled = true;
            this.ListBoxTechniciens.Location = new System.Drawing.Point(12, 47);
            this.ListBoxTechniciens.Name = "ListBoxTechniciens";
            this.ListBoxTechniciens.Size = new System.Drawing.Size(156, 225);
            this.ListBoxTechniciens.TabIndex = 2;
            this.ListBoxTechniciens.Tag = "";
            this.ListBoxTechniciens.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxTechniciens_MouseClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(36, 527);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(104, 33);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // MapMain
            // 
            this.MapMain.Bearing = 0F;
            this.MapMain.CanDragMap = true;
            this.MapMain.GrayScaleMode = false;
            this.MapMain.LevelsKeepInMemmory = 5;
            this.MapMain.Location = new System.Drawing.Point(174, 47);
            this.MapMain.MarkersEnabled = true;
            this.MapMain.MaxZoom = 2;
            this.MapMain.MinZoom = 2;
            this.MapMain.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MapMain.Name = "MapMain";
            this.MapMain.NegativeMode = false;
            this.MapMain.PolygonsEnabled = true;
            this.MapMain.RetryLoadTile = 0;
            this.MapMain.RoutesEnabled = true;
            this.MapMain.ShowTileGridLines = false;
            this.MapMain.Size = new System.Drawing.Size(630, 520);
            this.MapMain.TabIndex = 1;
            this.MapMain.Zoom = 0D;
            this.MapMain.Load += new System.EventHandler(this.Map_Load);
            // 
            // timerRafraichissementPositionTechnicien
            // 
            this.timerRafraichissementPositionTechnicien.Interval = 15000;
            this.timerRafraichissementPositionTechnicien.Tick += new System.EventHandler(this.timerRafraichissementPositionTechnicien_Tick);
            // 
            // ListBoxClients
            // 
            this.ListBoxClients.FormattingEnabled = true;
            this.ListBoxClients.Location = new System.Drawing.Point(13, 279);
            this.ListBoxClients.Name = "ListBoxClients";
            this.ListBoxClients.Size = new System.Drawing.Size(155, 238);
            this.ListBoxClients.TabIndex = 4;
            this.ListBoxClients.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxClients_MouseClick);
            // 
            // DispatcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 572);
            this.Controls.Add(this.ListBoxClients);
            this.Controls.Add(this.MapMain);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.ListBoxTechniciens);
            this.Controls.Add(this.menuStripDispatcher);
            this.MainMenuStrip = this.menuStripDispatcher;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(820, 610);
            this.MinimumSize = new System.Drawing.Size(820, 610);
            this.Name = "DispatcherForm";
            this.Text = "Application PPE3";
            this.menuStripDispatcher.ResumeLayout(false);
            this.menuStripDispatcher.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripDispatcher;
        private System.Windows.Forms.ToolStripMenuItem TechnicienToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAjoutTechnicienToolStripMenuItem;
        //private System.Windows.Forms.StatusStrip statusStripBDD;
        private System.Windows.Forms.ToolStripMenuItem envoiSMSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClient;
        private System.Windows.Forms.ToolStripMenuItem ajouterClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifierClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionMatérielToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajouterMaterielToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifierMatérielToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem affecterMaterielAUnTechnicienToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemIntervention;
        private System.Windows.Forms.ToolStripMenuItem ajouterToolStripMenuItemIntervention;
        private System.Windows.Forms.ToolStripMenuItem supprimerToolStripMenuItemIntervention;
        private System.Windows.Forms.ToolStripMenuItem modifierTechnicienToolStripMenuItem;
        private System.Windows.Forms.ListBox ListBoxTechniciens;
        private System.Windows.Forms.Button btnRefresh;
        private GMap.NET.WindowsForms.GMapControl MapMain;
        private System.Windows.Forms.Timer timerRafraichissementPositionTechnicien;
        private System.Windows.Forms.ListBox ListBoxClients;
        private System.Windows.Forms.ToolStripMenuItem modifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aperçuToolStripMenuItem;
    }
}

