﻿/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.MetaModel;

namespace MindMate.View.Dialogs
{
    public partial class IconSelectorExt : Form
    {
        public static IconSelectorExt Instance = new IconSelectorExt();

        public const string REMOVE_ICON_NAME = "remove";
        public const string REMOVE_ALL_ICON_NAME = "removeAll";

        public string SelectedIcon = "";

        private IconSelectorExt()
        {
            InitializeComponent();

            Debugging.Utility.StartTimeCounter("Loading icons");

            ImageList imageList = new ImageList();
            for (int i = 0; i < MetaModel.MetaModel.Instance.IconsList.Count; i++)
            {
                ModelIcon icon = MetaModel.MetaModel.Instance.IconsList[i];
                imageList.Images.Add(icon.Bitmap);
                listView.Items.Add(icon.Title, i);
            }
            listView.SmallImageList = imageList;
            listView.LargeImageList = imageList;
                        
            Debugging.Utility.EndTimeCounter("Loading icons");
        }

        

        private Button CreateIconButton(ModelIcon icon)
        {
            Button b = new Button();
            b.Name = icon.Name;
            b.FlatStyle = FlatStyle.Standard;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            b.Image = icon.Bitmap;
            b.Size = new Size(80, 27);
            b.Text = icon.Title.Length > 8 ? icon.Title.Substring(0, 8) : icon.Title;
            b.ImageAlign = ContentAlignment.MiddleLeft;
            b.TextAlign = ContentAlignment.MiddleRight;
            toolTip1.SetToolTip(b, icon.Title + ", " + icon.Shortcut.ToString());
            b.Margin = new Padding(0);
            //b.Tag = icon.Shortcut;
            b.MouseHover += new EventHandler(b_MouseHover);
            b.Click += new EventHandler(b_Click);
            b.GotFocus += b_GotFocus;
            b.LostFocus += b_LostFocus;
                        
            return b;
        }

        void b_LostFocus(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Standard; 
        }

        void b_GotFocus(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = toolTip1.GetToolTip((Control)sender);
            ((Button)sender).FlatStyle = FlatStyle.Flat; 
        }

        void b_MouseHover(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        void b_Click(object sender, EventArgs e)
        {
            this.SelectedIcon = ((Button)sender).Name;
            this.DialogResult = DialogResult.OK;
        }

        
        private Button CreateRemoveIconButton()
        {
            Button bRemove = new Button();
            bRemove.Name = REMOVE_ICON_NAME;
            bRemove.FlatStyle = FlatStyle.Standard;
            bRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            bRemove.Image = MindMate.Properties.Resources.remove;
            bRemove.Size = new Size(27, 27);
            toolTip1.SetToolTip(bRemove, "Remove Last Icon, Backspace");
            bRemove.Margin = new Padding(0);
            //bRemove.Tag = "";
            bRemove.MouseHover += new EventHandler(b_MouseHover);
            bRemove.Click += new EventHandler(b_Click);
            bRemove.GotFocus += b_GotFocus;
            bRemove.LostFocus += b_LostFocus;

            return bRemove;
           
        }

        private Button CreateRemoveAllIconButton()
        {
            Button bRemoveAll = new Button();
            bRemoveAll.Name = REMOVE_ALL_ICON_NAME;
            bRemoveAll.FlatStyle = FlatStyle.Standard;
            bRemoveAll.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            bRemoveAll.Image = MindMate.Properties.Resources.edittrash;
            bRemoveAll.Size = new Size(27, 27);
            toolTip1.SetToolTip(bRemoveAll, "Remove All Icons, Delete");
            bRemoveAll.Margin = new Padding(0);
            //bRemoveAll.Tag = "";
            bRemoveAll.MouseHover += new EventHandler(b_MouseHover);
            bRemoveAll.Click += new EventHandler(b_Click);
            bRemoveAll.GotFocus += b_GotFocus;
            bRemoveAll.LostFocus += b_LostFocus;

            return bRemoveAll;

        }

                
        private void IconSelector_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Back: //Backspace key pressed
                    SelectedIcon = REMOVE_ICON_NAME;
                    this.DialogResult = DialogResult.OK;
                    break;
                case Keys.Delete: //Delete key pressed    
                    SelectedIcon = REMOVE_ALL_ICON_NAME;
                    this.DialogResult = DialogResult.OK;
                    break;           
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;                
                default: //Other keys                    
                    foreach (ModelIcon icon in MetaModel.MetaModel.Instance.IconsList)
                    {
                        if (icon.Shortcut == ((char)e.KeyValue).ToString().ToUpper())
                        {
                            this.SelectedIcon = icon.Name;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    break;
            }
            //e.SuppressKeyPress = true;
            //e.Handled = true;
        }        
    }
}