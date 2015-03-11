﻿using MySql.Data.MySqlClient;
using System;
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
    public partial class frmEditarProveedor : Form
    {
        int idC = 0;
        Proveedor p;
        TipoPersona t;
        List<int> idSucursal = new List<int>();

        public frmEditarProveedor(int id)
        {
            InitializeComponent();
            p = new Proveedor(id); 
            p.Contacto.ContactDataBaseModified += new EventHandler(Contact_Modified);
        }

        private void CargarSucursales()
        {
            try
            {
                string sql = "SELECT id, nombre FROM sucursal";
                DataTable dt = ConexionBD.EjecutarConsultaSelect(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    idSucursal.Add((int)dr["id"]);
                    cboSucursal.Items.Add(dr["nombre"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int PosicionSucursal(int id)
        {
            int pos = -1;
            try
            {
                for (int i = 0; i < idSucursal.Count; i++)
                {
                    if (idSucursal[i] == id)
                    {
                        pos = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pos;
        }

        private void MostrarDatos()
        {
            try
            {
                p.ObtenerDatos();
                cboSucursal.SelectedIndex = PosicionSucursal(p.ID);
                txtNombre.Text = p.Nombre;
                txtRazonSocial.Text = p.RazonSocial;
                txtRFC.Text = p.RFC;
                txtCalle.Text = p.Calle;
                txtNumExt.Text = p.NumExt;
                txtNumInt.Text = p.NumInt;
                txtColonia.Text = p.Colonia;
                txtCiudad.Text = p.Ciudad;
                txtEstado.Text = p.Estado;
                txtCP.Text = p.CP.ToString();
                txtTelefono01.Text = p.Telefono01;
                txtTelefono02.Text = p.Telefono02;
                txtLada01.Text = p.Lada01;
                txtLada02.Text = p.Lada02;
                txtCorreo.Text = p.Correo;
                t = p.Tipo;
                switch (p.Tipo)
                {
                    case TipoPersona.Credito:
                        cboTipoCredito.SelectedIndex = 1;
                        break;
                    case TipoPersona.SinCredito:
                        cboTipoCredito.SelectedIndex = 0;
                        break;
                }
                txtLimiteCredito.Text = p.LimiteCredito.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MostrarContactos()
        {
            dgvContactos.Rows.Clear();
            ProveedorContacto pc = p.Contacto;
            for (int i = 0; i < pc.IDCS.Count; i++)
            {
                string telefonos = "", correo = "";
                if (pc.TelefonoContactos01[i] != "" && pc.TelefonoContactos02[i] != "")
                {
                    if (pc.LadaContactos01[i] != "")
                    {
                        telefonos += pc.LadaContactos01[i] + " " + pc.TelefonoContactos01[i];
                    }
                    else
                    {
                        telefonos += pc.TelefonoContactos01[i];
                    }
                    if (pc.LadaContactos02[i] != "")
                    {
                        telefonos += ", " + pc.LadaContactos02[i] + " " + pc.TelefonoContactos02[i];
                    }
                    else
                    {
                        telefonos += ", " + pc.TelefonoContactos02[i];
                    }
                }
                else if (pc.TelefonoContactos01[i] != "")
                {
                    if (pc.LadaContactos01[i] != "")
                    {
                        telefonos += pc.LadaContactos01[i] + " " + pc.TelefonoContactos01[i];
                    }
                    else
                    {
                        telefonos += pc.TelefonoContactos01[i];
                    }
                }
                else if (pc.TelefonoContactos02[i] != "")
                {
                    if (pc.LadaContactos02[i] != "")
                    {
                        telefonos += pc.LadaContactos02[i] + " " + pc.TelefonoContactos02[i];
                    }
                    else
                    {
                        telefonos += pc.TelefonoContactos02[i];
                    }
                }
                if (pc.CorreoContactos[i] != "")
                {
                    correo = pc.CorreoContactos[i];
                }
                else
                {
                    correo = "Sin información";
                }
                dgvContactos.Rows.Add(new object[] { pc.IDCS[i], pc.NombreContactos[i], telefonos, correo });
            }
            dgvContactos_RowEnter(dgvContactos, new DataGridViewCellEventArgs(0, 0));
        }

        private void Editar()
        {
            try
            {
                p.Sucursal = idSucursal[cboSucursal.SelectedIndex];
                p.Nombre = txtNombre.Text;
                p.RazonSocial = txtRazonSocial.Text;
                p.RFC = txtRFC.Text;
                p.Calle = txtCalle.Text;
                p.NumExt = txtNumExt.Text;
                p.NumInt = txtNumInt.Text;
                p.Colonia = txtColonia.Text;
                p.Ciudad = txtCiudad.Text;
                p.Estado = txtEstado.Text;
                p.CP = int.Parse(txtCP.Text);
                p.Telefono01 = txtTelefono01.Text;
                p.Telefono02 = txtTelefono02.Text;
                p.Lada01 = txtLada01.Text;
                p.Lada02 = txtLada02.Text;
                p.Correo = txtCorreo.Text;
                p.Tipo = t;
                if (txtLimiteCredito.Text.Trim() == "")
                    p.LimiteCredito = decimal.Parse(txtLimiteCredito.Text);
                else
                    p.LimiteCredito = 0M;
                p.Editar();
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool VerificarDatos()
        {
            if (cboSucursal.SelectedIndex < 0)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo sucursal es obligatorio", "EC-Admin");
                return false;
            }
            if (txtNombre.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo nombre es obligatorio", "EC-Admin");
                return false;
            }
            if (txtRFC.Text.Length < 12)
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo RFC debe tener entre 12 o 13 caracteres", "EC-Admin");
                return false;
            }
            if (txtCalle.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo calle es obligatorio", "EC-Admin");
                return false;
            }
            if (txtNumInt.Text.Trim() != "" && txtNumExt.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo número exterior debe ser ingresado antes que el número interior", "EC-Admin");
                return false;
            }
            if (txtNumExt.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo número exterior es obligatorio", "EC-Admin");
                return false;
            }
            if (txtColonia.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo colonia es obligatorio", "EC-Admin");
                return false;
            }
            if (txtCiudad.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo ciudad es obligatorio", "EC-Admin");
                return false;
            }
            if (txtEstado.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo estado es obligatorio", "EC-Admin");
                return false;
            }
            if (txtCP.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo código postal es obligatorio", "EC-Admin");
                return false;
            }
            if (txtTelefono01.Text.Trim() == "" && txtTelefono02.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "Debes ingresar al menos un teléfono", "EC-Admin");
                return false;
            }
            else
            {
                if (txtTelefono01.Text.Trim() == "" && txtTelefono02.Text.Trim() != "")
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "Debes ingresar el primer teléfono antes que el segundo", "EC-Admin");
                    return false;
                }
            }
            if (txtCorreo.Text.Trim() == "")
            {
                FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo correo es obligatorio", "EC-Admin");
                return false;
            }
            else
            {
                if (!FuncionesGenerales.EsCorreoValido(txtCorreo.Text))
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "No se reconoce el correo ingresado como uno válido", "EC-Admin");
                    return false;
                }
            }
            if (cboTipoCredito.SelectedIndex == 1)
            {
                if (txtLimiteCredito.Text.Trim() == "")
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Alerta, "El campo límite de crédito es obligatorio", "EC-Admin");
                    return false;
                }
            }
            return true;
        }

        private void txtCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            FuncionesGenerales.VerificarEsNumero(ref sender, ref e, true);
        }

        private void txtLimiteCredito_KeyPress(object sender, KeyPressEventArgs e)
        {
            FuncionesGenerales.VerificarEsNumero(ref sender, ref e, false);
        }

        private void cboTipoCredito_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboTipoCredito.SelectedIndex)
            {
                case 0:
                    lblELimiteCredito.Visible = txtLimiteCredito.Visible = false;
                    t = TipoPersona.SinCredito;
                    break;
                case 1:
                    lblELimiteCredito.Visible = txtLimiteCredito.Visible = true;
                    t = TipoPersona.Credito;
                    break;
            }
            txtLimiteCredito.Text = "0.00";
        }

        private void dgvContactos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvContactos.CurrentRow != null)
                idC = (int)dgvContactos[0, e.RowIndex].Value;
            else
                idC = 0;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (VerificarDatos())
            {
                try
                {
                    Editar();
                    FuncionesGenerales.Mensaje(this, Mensajes.Exito, "¡Se ha modificado el proveedor correctamente!", "EC-Admin");
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error al modificar el proveedor. No se ha podido conectar a la base de datos", "EC-Admin", ex);
                }
                catch (Exception ex)
                {
                    FuncionesGenerales.Mensaje(this, Mensajes.Error, "Ocurrió un error genérico al modificar el proveedor.", "EC-Admin", ex);
                }
            }
        }

        private void frmEditarProveedor_Load(object sender, EventArgs e)
        {
            CargarSucursales();
            MostrarDatos();
            MostrarContactos();
        }

        private void lblEDatosCliente_Click(object sender, EventArgs e)
        {
            lblEDatosCliente.ForeColor = Colores.Claro;
            lblEDatosCliente.BackColor = Colores.Obscuro;
            lblEContactos.ForeColor = Colores.Obscuro;
            lblEContactos.BackColor = Colores.Claro;
            pnlContactos.Visible = false;
            pnlDatos.Visible = true;
        }

        private void lblEContactos_Click(object sender, EventArgs e)
        {
            lblEDatosCliente.BackColor = Colores.Claro;
            lblEDatosCliente.ForeColor = Colores.Obscuro;
            lblEContactos.BackColor = Colores.Obscuro;
            lblEContactos.ForeColor = Colores.Claro;
            pnlContactos.Visible = true;
            pnlDatos.Visible = false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            (new frmDatosContactoProveedor(p.Contacto)).ShowDialog(this);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvContactos.CurrentRow != null)
            {
                (new frmDatosContactoProveedor(idC, p.Contacto)).ShowDialog(this);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvContactos.CurrentRow != null)
            {
                if (FuncionesGenerales.Mensaje(this, Mensajes.Pregunta, "¿Realmente deseas eliminar a " + dgvContactos[1, dgvContactos.CurrentRow.Index].Value.ToString() + "?", "") == System.Windows.Forms.DialogResult.Yes)
                {
                    p.Contacto.Eliminar(idC);
                }
            }
        }
    
        private void Contact_Modified(object sender, EventArgs e)
        {
            MostrarContactos();
        }
    }
}