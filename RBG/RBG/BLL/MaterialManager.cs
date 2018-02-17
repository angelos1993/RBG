﻿using System.Linq;
using RBG.BLL.Infrastructure;
using RBG.DAL.Model;

namespace RBG.BLL
{
    public class MaterialManager : BaseManager
    {
        #region Properties

        #endregion

        #region Methods

        public void AddMaterial(Material material)
        {
            UnitOfWork.MaterialRepository.Add(material);
        }

        public bool IsMaterialCodeExists(string materialCode)
        {
            return UnitOfWork.MaterialRepository.Get(material => material.Code == materialCode).Any();
        }

        public bool IsMaterialNameExists(string materialName)
        {
            return UnitOfWork.MaterialRepository.Get(material => material.Name == materialName).Any();
        }

        #endregion
    }
}