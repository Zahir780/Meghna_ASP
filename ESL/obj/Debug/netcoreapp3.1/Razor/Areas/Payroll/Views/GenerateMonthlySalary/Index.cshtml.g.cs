#pragma checksum "E:\ASP PROJECT\Meghna\ESL\Areas\Payroll\Views\GenerateMonthlySalary\Index.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Payroll_Views_GenerateMonthlySalary_Index), @"mvc.1.0.view", @"/Areas/Payroll/Views/GenerateMonthlySalary/Index.cshtml")]
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
#line 1 "E:\ASP PROJECT\Meghna\ESL\Areas\Payroll\Views\_ViewImports.cshtml"
using ESL;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ASP PROJECT\Meghna\ESL\Areas\Payroll\Views\_ViewImports.cshtml"
using ESL.DataAccess.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ASP PROJECT\Meghna\ESL\Areas\Payroll\Views\_ViewImports.cshtml"
using ESL.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b", @"/Areas/Payroll/Views/GenerateMonthlySalary/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"eccb5e970c1360abd43e5bc3b2e51207078d5ad96eedb8a15b4beb1b9b1b2018", @"/Areas/Payroll/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Areas_Payroll_Views_GenerateMonthlySalary_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal form-bordered"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("autocomplete", new global::Microsoft.AspNetCore.Html.HtmlString("off"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_ModalButtonGenerate", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/JSPayroll/GenerateMonthlySalaryService.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<style>
    
</style>

<section class=""panel"">
    <header class=""panel-heading"">
        <div class=""panel-actions"">
            <a href=""#"" class=""fa fa-caret-down""></a>
        </div>
        <h2 class=""panel-title entryListHeader"">Generate Monthly Salary</h2>
    </header>

    <div class=""panel-body"">
        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b6211", async() => {
                WriteLiteral(@"
            <input type=""hidden"" id=""autoId"" />
            <div class=""col-12"">
                <div class=""row"" style=""margin:0; padding:0;"">
                    <div class=""col-sm-2""></div>
                    <div class=""col-sm-6"">
                        <div class=""row"" style=""margin:0; padding:0;"">
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Generate Date:</label>
                                <div class=""col-sm-4"">
                                    <div class=""input-group"">
                                        <span class=""input-group-addon"">
                                            <i class=""fa fa-calendar""></i>
                                        </span>
                                        <input type=""text"" id=""pGenerateDate"" onkeypress=""inputMask(event,'00-00-0000')"" class=""form-control input-sm datepick"" required />
                                    ");
                WriteLiteral(@"</div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Department:</label>
                                <div class=""col-sm-8"" style=""display: flex;"">
                                    <select data-plugin-selectTwo id=""pDepartmentId"" class=""form-control input-sm"">
                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b8190", async() => {
                    WriteLiteral("==Choose a Department==");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                                    </select>
                                </div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Section:</label>
                                <div class=""col-sm-8"" style=""display: flex;"">
                                    <select data-plugin-selectTwo id=""pSectionId"" class=""form-control input-sm"">
                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b9999", async() => {
                    WriteLiteral("==Choose a Section==");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                                    </select>
                                </div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Employee:</label>
                                <div class=""col-sm-8"" style=""display: flex;"">
                                    <select data-plugin-selectTwo id=""pEmployeeId"" class=""form-control input-sm"">
                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b11807", async() => {
                    WriteLiteral("==Choose a Employee==");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                                    </select>
                                </div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Salary of Month:</label>
                                <div class=""col-sm-4"">
                                    <div class=""input-group"">
                                        <span class=""input-group-addon"">
                                            <i class=""fa fa-calendar""></i>
                                        </span>
                                        <input type=""text"" id=""pSalaryMonthDate"" onkeypress=""inputMask(event,'00-00-0000')"" class=""form-control input-sm datepick"" required />
                                    </div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                  ");
                WriteLiteral(@"          <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Total Days:</label>
                                <div class=""col-sm-2"">
                                    <div class=""input-group"">
                                        <input type=""text"" id=""pTotalDays"" style=""width: 40px;"" class=""form-control input-sm amount"" required />
                                    </div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>No of Fridays:</label>
                                <div class=""col-sm-2"">
                                    <div class=""input-group"">
                                        <input type=""text"" id=""pFridays"" style=""width: 40px;"" ");
                WriteLiteral(@"class=""form-control input-sm amount"" required />
                                    </div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>No of Holidays:</label>
                                <div class=""col-sm-2"">
                                    <div class=""input-group"">
                                        <input type=""text"" id=""pHolidays"" style=""width: 40px;"" class=""form-control input-sm amount"" required />
                                    </div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-labe");
                WriteLiteral(@"l""><span class=""text-danger"">*</span>No of Working Days:</label>
                                <div class=""col-sm-2"">
                                    <div class=""input-group"">
                                        <input type=""text"" id=""pWorkingDays"" style=""width: 40px;"" class=""form-control input-sm amount"" required />
                                    </div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                            <div class=""form-group"">
                                <label class=""col-sm-4 control-label""><span class=""text-danger"">*</span>Revenue Stamp:</label>
                                <div class=""col-sm-2"">
                                    <div class=""input-group"">
                                        <input type=""text"" id=""pRevenueStamp"" style=""width: 40px;"" class=""form-control input-sm amount"" required />
                                    </");
                WriteLiteral(@"div>
                                </div>
                                <div class=""col-sm-4"" style=""display:flex""></div>
                            </div>
                        </div>
                    </div>
                    <div class=""col-sm-2""></div>
                </div>
            </div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n    <footer class=\"panel-footer text-center\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b18984", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </footer>\r\n</section>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8cc985f14d8ad09e4508fd0b975d7a0fba3ae34078606a1e96c55bd001db232b20254", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
#nullable restore
#line 126 "E:\ASP PROJECT\Meghna\ESL\Areas\Payroll\Views\GenerateMonthlySalary\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
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
