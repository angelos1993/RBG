﻿using System;
using System.Windows.Forms;
using RBG.BLL;
using RBG.DAL.Model;
using RBG.Utility;
using static RBG.Utility.Constants;
using static RBG.Utility.MessageBoxUtility;

namespace RBG.PL.Forms
{
    public partial class FrmAddMaterial : FrmMaster
    {
        #region Constructor

        public FrmAddMaterial(int? materialId = null)
        {
            InitializeComponent();
            if (materialId.HasValue)
                SetFormForEditMode(materialId.Value);
        }

        #endregion

        #region Properties

        private MaterialManager _materialManager;
        private MaterialManager MaterialManager => _materialManager ?? (_materialManager = new MaterialManager());
        private bool IsEditMode { get; set; }
        private Material Material { get; set; }

        #endregion

        #region Events

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SaveMaterial();
            Cursor = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Methods

        private void SaveMaterial()
        {
            ErrorProvider.Clear();
            var isFormValid = true;
            if (txtCode.Text.FullTrim().IsNullOrEmptyOrWhiteSpace())
            {
                isFormValid = false;
                ErrorProvider.SetError(txtCode, ValidationMsg);
            }
            if (txtName.Text.FullTrim().IsNullOrEmptyOrWhiteSpace())
            {
                isFormValid = false;
                ErrorProvider.SetError(txtName, ValidationMsg);
            }
            if (Math.Abs(dblInPrice.Value) <= 0)
            {
                isFormValid = false;
                ErrorProvider.SetError(dblInPrice, ValidationMsg);
            }
            if (!isFormValid)
            {
                txtCode.Focus();
                return;
            }
            var isMaterialCodeExists = !IsEditMode
                ? MaterialManager.IsMaterialCodeExists(txtCode.Text.FullTrim())
                : txtCode.Text.FullTrim() != Material.Code &&
                  MaterialManager.IsMaterialCodeExists(txtCode.Text.FullTrim());
            var isMaterialNameExists = !IsEditMode
                ? MaterialManager.IsMaterialNameExists(txtName.Text.FullTrim())
                : txtName.Text.FullTrim() != Material.Name &&
                  MaterialManager.IsMaterialNameExists(txtName.Text.FullTrim());
            if (isMaterialNameExists && isMaterialCodeExists)
            {
                ShowErrorMsg("الكود والاسم مستخدمان من قبل");
                txtCode.Focus();
            }
            else if (isMaterialNameExists)
            {
                ShowErrorMsg("الاسم الذي أدخلتة مستخدم من قبل");
                txtName.Focus();
            }
            else if (isMaterialCodeExists)
            {
                ShowErrorMsg("الكود الذي أدخلتة مستخدم من قبل");
                txtCode.Focus();
            }
            else
            {
                if (!IsEditMode)
                {
                    MaterialManager.AddMaterial(new Material
                    {
                        Code = txtCode.Text.FullTrim(),
                        Name = txtName.Text.FullTrim(),
                        Price = (decimal) dblInPrice.Value
                    });
                }
                else
                {
                    Material.Code = txtCode.Text.FullTrim();
                    Material.Name = txtName.Text.FullTrim();
                    Material.Price = (decimal) dblInPrice.Value;
                    MaterialManager.UpdateMaterial(Material);
                }
                Close();
            }
        }

        private void SetFormForEditMode(int materialId)
        {
            IsEditMode = true;
            Material = MaterialManager.GetMaterialById(materialId);
            Text = @"تعديل مادة / خامة";
            txtCode.Text = Material.Code;
            txtName.Text = Material.Name;
            dblInPrice.Value = (double) Material.Price;
        }

        #endregion
    }
}