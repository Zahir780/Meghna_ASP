#pragma checksum "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "58f77d8ff2f26a6c4b2a56396111366e347e563d79b7a0ac4df1ee3373e0b9fa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_Menu_Default), @"mvc.1.0.view", @"/Views/Shared/Components/Menu/Default.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\ASP PROJECT\Meghna\ESL\Views\_ViewImports.cshtml"
using ESL;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ASP PROJECT\Meghna\ESL\Views\_ViewImports.cshtml"
using ESL.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
using ESL.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
using ESL.Utility;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"58f77d8ff2f26a6c4b2a56396111366e347e563d79b7a0ac4df1ee3373e0b9fa", @"/Views/Shared/Components/Menu/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"6f393bd9078297a04033a0c18f60bab3c825e2c28bc932749867fad4cd52f7b9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_Components_Menu_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ESL.Models.ViewModels.Menus>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "Admin", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n\r\n\r\n<div class=\"nano\">\r\n    <div class=\"nano-content\">\r\n        <nav id=\"menu\" class=\"nav-main\" role=\"navigation\">\r\n            <ul class=\"nav nav-main\">\r\n                <li class=\"nav-active\">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "58f77d8ff2f26a6c4b2a56396111366e347e563d79b7a0ac4df1ee3373e0b9fa4979", async() => {
                WriteLiteral("\r\n                        <i class=\"fa fa-home\" aria-hidden=\"true\"></i>\r\n                        <span>Dashboard</span>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </li>\r\n");
#nullable restore
#line 58 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
                  
                    foreach (Menu menu in Model.Modules)
                    {
                        string html = "";
                        if (menu.IsActive)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li class=\"nav-parent\">\r\n                                <a>\r\n                                    <i");
            BeginWriteAttribute("class", " class=\"", 2500, "\"", 2518, 1);
#nullable restore
#line 66 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 2508, menu.Icon, 2508, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" aria-hidden=\"true\"></i>\r\n                                    <span>");
#nullable restore
#line 67 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
                                     Write(menu.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                                </a>\r\n                                <ul class=\"nav nav-children\">\r\n                                    ");
#nullable restore
#line 70 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
                               Write(Html.Raw(ShowSubItems(menu.Menus, html)));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </ul>\r\n                            </li>\r\n");
#nullable restore
#line 73 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
                        }


                    }
                

#line default
#line hidden
#nullable disable
            WriteLiteral(@"



            </ul>
        </nav>

        <hr class=""separator"" />


    </div>

</div>

<script>

    function delay(URL) {
        setTimeout(function () { window.location = URL }, 3000);
    }
    
    function menuset(e) {
        var mId = e.target.getAttribute('data-mId');
        var uId = e.target.getAttribute('data-uId');
        var url = e.target.getAttribute('data-href');
        $.ajax({
            url: '/api/menupush',
            data: JSON.stringify({ ""uId"": uId, ""menuVId"": mId }),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                window.location = url;
            }
        });

}
</script>

");
        }
        #pragma warning restore 1998
#nullable restore
#line 8 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\Components\Menu\Default.cshtml"
           

    public string ShowSubItems(IEnumerable<Menu> menus, string html)
    {

        foreach (Menu subItem in menus)
        {

            if (subItem.IsActive)
            {
                
                    if (!subItem.IsReporting && subItem.Menus.Any()) // add child Menu
                    {
                        html += "<li class='nav-parent'>" +
                            "<a data-toggle='tooltip' data-placement='top' title='' data-original-title='" + subItem.Name + "'>" +
                                "<i class='" + subItem.Icon + "' aria-hidden='true'></i>" +
                                "<span>" + subItem.Name + "</span>" +
                            "</a>" +
                            "<ul class='nav nav-children'>";
                        html = ShowSubItems(subItem.Menus, html);
                        html += "</ul></li>";
                    }
                    else // add parent Menu
                    {
                        html += "<li>" +
                                    "<a class='gotocontroller' data-mId='" + subItem.Id + "' data-uId='" + @ViewBag.UserId + "' data-href='" + subItem.Url + "' onclick='menuset(event)' data-toggle='tooltip' data-placement='top' title='' data-original-title='" + subItem.Name + "' >" + subItem.Name + "</a>" +
                                "</li>";
                    }
                

            }

        }

        return html;
    }

#line default
#line hidden
#nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<IdentityUser> UserManager { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<IdentityUser> SignInManager { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ESL.Models.ViewModels.Menus> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
