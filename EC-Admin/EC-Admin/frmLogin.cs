﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EC_Admin
{
    public partial class frmLogin : Form
    {
        bool cancelo;

        public frmLogin()
        {
            InitializeComponent();
            cancelo = true;
        }   

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Usuario.VerificarIngresoUsuario(txtUsuario.Text, txtPass.Text))
            {
                frmPrincipal.Instancia.Show();
                cancelo = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario y/o contraseña no coinciden.", "EC-Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPass.Text = "";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancelo)
            {
                Application.Exit();
                ConexionBD.CerrarConexion();
            }
        }

        private void llbReestablecer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new Forms.frmReestablecerPass()).ShowDialog(this);
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (Usuario.CantidadUsuariosAdministrador == 0)
            {
                if (MessageBox.Show("No tienes usuarios registrados, ¿deseas crear uno?", "EC-Admin", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    (new Forms.frmNuevoUsuario(new string[] { "Administador" })).Show();
                }
                else
                {
                    MessageBox.Show("La aplicación se cerrará. Puede crear un usuario la próxima vez que lo abra.", "EC-Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCancelar.PerformClick();
                }
            }
        }

        private void btnPruebas_Click(object sender, EventArgs e)
        {
        }
    }
}