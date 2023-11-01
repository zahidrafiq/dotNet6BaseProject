using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Repository.Infrastructure;

namespace Core.Repository
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public void SetDefaultStartDate()
        {
            dynamic child = this;

            try
            {
                child.Effective_Start_Date = DateTime.UtcNow;
            }
            catch
            {
                throw new Exception("Effective_Start_Date property does not exist");
            }
        }

        public void SetDefaultEndDate()
        {
            dynamic child = this;

            try
            {
                child.Effective_End_Date = Convert.ToDateTime("3/12/2099");
            }
            catch
            {
                throw new Exception("Effective_End_Date property does not exist");
            }
        }

        public void SetDefaultStartEndDate()
        {
            SetDefaultStartDate();
            SetDefaultEndDate();
        }
    }
}
