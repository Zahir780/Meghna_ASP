#pragma checksum "E:\ASP PROJECT\Meghna\ESL\Views\Shared\_AttachmentPertialView.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "55a83114aa36030441f427389fb19c5d24fbd1b8c332069e6e95b1eb92e1b2bd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__AttachmentPertialView), @"mvc.1.0.view", @"/Views/Shared/_AttachmentPertialView.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"55a83114aa36030441f427389fb19c5d24fbd1b8c332069e6e95b1eb92e1b2bd", @"/Views/Shared/_AttachmentPertialView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"6f393bd9078297a04033a0c18f60bab3c825e2c28bc932749867fad4cd52f7b9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__AttachmentPertialView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
    <div class=""panel-body"" style=""background-color: #f7f7f7;"">
        <div class=""timeline"">
            <div class=""tm-body"">

                <ol class=""tm-items"">
                    <li>

                        <div class=""tm-box"">
                            <div class=""tm-meta"">
                                <span>
                                    <i class=""fa fa-user"" style=""color: #0088cc;""></i> Entry By:  <a id=""notice_user"">");
#nullable restore
#line 12 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\_AttachmentPertialView.cshtml"
                                                                                                                 Write(ViewBag.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                                </span>\r\n                                <span>\r\n                                    <i class=\"fa fa-clock-o\" style=\"color: #0088cc;\"></i> Date:  <a>");
#nullable restore
#line 15 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\_AttachmentPertialView.cshtml"
                                                                                               Write(ViewBag.Date);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                                </span>\r\n                                <span style=\"float: right\">\r\n                                    <i class=\"fa fa-clock-o\" style=\"color: #0088cc;\"></i> Entry Date: <a>");
#nullable restore
#line 18 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\_AttachmentPertialView.cshtml"
                                                                                                    Write(ViewBag.EntryDate);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</a>
                                </span>
                            </div>
                        </div>
                    </li>

                    <li>

                        <div class=""tm-box"">
                            <div class=""embed-responsive embed-responsive-16by9"" style=""height: 650px;"">
                                <iframe class=""embed-responsive-item"" id=""notice_attachment""");
            BeginWriteAttribute("src", "\r\n                                        src=\"", 1322, "\"", 1381, 1);
#nullable restore
#line 29 "E:\ASP PROJECT\Meghna\ESL\Views\Shared\_AttachmentPertialView.cshtml"
WriteAttributeValue("", 1369, ViewBag.Url, 1369, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                                </iframe>
                            </div>

                        </div>
                    </li>
                </ol>

                
            </div>
        </div>
    </div>
<script>
    $(document).ready(function () {
        //everything is fully loaded, don't use me if you can use DOMContentLoaded
       
    });
    
    
</script>

");
        }
        #pragma warning restore 1998
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
