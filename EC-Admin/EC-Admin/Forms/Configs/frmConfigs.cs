﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EC_Admin.Forms
{
    public partial class frmConfigs : Form
    {
        #region Instancia
        private static frmConfigs frmInstancia;
        public static frmConfigs Instancia
        {
            get
            {
                if (frmInstancia == null)
                    frmInstancia = new frmConfigs();
                else if (frmInstancia.IsDisposed)
                    frmInstancia = new frmConfigs();
                return frmInstancia;

            }
            set
            {
                frmInstancia = value;
            }
        }
        #endregion

        public frmConfigs()
        {
            InitializeComponent();
        }

        private void btnSucursales_Click(object sender, EventArgs e)
        {
            if (!frmSucursal.Instancia.Visible)
                frmSucursal.Instancia.Show();
            else
                frmSucursal.Instancia.Select();
        }

        private void btnDirecciones_Click(object sender, EventArgs e)
        {
            if (!frmDomicilio.Instancia.Visible)
                frmDomicilio.Instancia.Show();
            else
                frmDomicilio.Instancia.Select();
        }
    }
}