using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models.ViewModels
{
    public class Menu
    {
        public int Id { get; set; }
        public string VId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int ModuleId { get; set; }
        public int ParentId { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsAdd { get; set; }
        public Boolean IsEdit { get; set; }
        public Boolean IsDelete { get; set; }
        public Boolean IsPreview { get; set; }
        public Boolean IsReporting { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
