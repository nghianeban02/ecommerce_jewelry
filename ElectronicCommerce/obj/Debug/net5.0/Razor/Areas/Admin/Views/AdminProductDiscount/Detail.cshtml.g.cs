#pragma checksum "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "000071c2a01e91251be2783b3676db6b6138dc6df75e8faa6cdcf3080124cd32"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ElectronicCommerce.AdminProductDiscount.Areas_Admin_Views_AdminProductDiscount_Detail), @"mvc.1.0.view", @"/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml")]
namespace ElectronicCommerce.AdminProductDiscount
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"000071c2a01e91251be2783b3676db6b6138dc6df75e8faa6cdcf3080124cd32", @"/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"63843f91989032777c91fb874a4bc6095038aba6958026788c53779bfcef0329", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Areas_Admin_Views_AdminProductDiscount_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ElectronicCommerce.Models.ProductDiscount>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/admin/lib/ProductDiscount.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
  
	Layout = "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "000071c2a01e91251be2783b3676db6b6138dc6df75e8faa6cdcf3080124cd324042", async() => {
                WriteLiteral(@"

	<script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js""></script>

	<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
	<style>
		.switch {
			position: relative;
			display: inline-block;
			width: 60px;
			height: 34px;
		}

			.switch input {
				opacity: 0;
				width: 0;
				height: 0;
			}

		.slider {
			position: absolute;
			cursor: pointer;
			top: 0;
			left: 0;
			right: 0;
			bottom: 0;
			background-color: #ccc;
			-webkit-transition: .4s;
			transition: .4s;
		}

			.slider:before {
				position: absolute;
				content: """";
				height: 26px;
				width: 26px;
				left: 4px;
				bottom: 4px;
				background-color: white;
				-webkit-transition: .4s;
				transition: .4s;
			}

		input:checked + .slider {
			background-color: #2196F3;
		}

		input:focus + .slider {
			box-shadow: 0 0 1px #2196F3;
		}

		input:checked + .slider:before {
			-webkit-transform: translateX(26px);
			-ms-transform: translateX(26");
                WriteLiteral("px);\r\n\t\t\ttransform: translateX(26px);\r\n\t\t}\r\n\r\n\t\t/* Rounded sliders */\r\n\t\t.slider.round {\r\n\t\t\tborder-radius: 34px;\r\n\t\t}\r\n\r\n\t\t\t.slider.round:before {\r\n\t\t\t\tborder-radius: 50%;\r\n\t\t\t}\r\n\t</style>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "000071c2a01e91251be2783b3676db6b6138dc6df75e8faa6cdcf3080124cd326323", async() => {
                WriteLiteral("\r\n\t<div class=\"content-wrapper\">\r\n\t\t<h3>Chi tiết khuyến mãi</h3>\r\n\t\t<div class=\"table-responsive\">\r\n\t\t<table>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Mã khuyến mãi: </th>\r\n\t\t\t\t<td style=\"padding-left:20px;\">");
#nullable restore
#line 81 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                          Write(ViewBag.productDiscounts.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Tên khuyến mãi: </th>\r\n\t\t\t\t<td style=\"padding-left:20px;\">");
#nullable restore
#line 85 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                          Write(ViewBag.productDiscounts.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Giảm: </th>\r\n\t\t\t\t<td style=\"padding-left:20px;\">");
#nullable restore
#line 89 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                          Write(ViewBag.productDiscounts.DiscountValue);

#line default
#line hidden
#nullable disable
#nullable restore
#line 89 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                                                                 Write(ViewBag.productDiscounts.DiscountUnit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Ngày tạo: </th>\r\n\t\t\t\t<td style=\"padding-left:20px;\">");
#nullable restore
#line 93 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                          Write(ViewBag.productDiscounts.DateCreated);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<th>Hiệu lực đến: </th>\r\n\t\t\t\t<td style=\"padding-left:20px;\">");
#nullable restore
#line 97 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                          Write(ViewBag.productDiscounts.ValidUntil);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t<th>Trạng thái:</th>\r\n\t\t\t\t\t\t\t\t\t<td style=\"padding-left:20px;padding-top:10px;\">\r\n");
#nullable restore
#line 102 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                         if (ViewBag.productDiscounts.Active == true)
										{

#line default
#line hidden
#nullable disable
                WriteLiteral("\t\t\t\t\t\t\t\t\t\t\t<label class=\"switch\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<input type=\"checkbox\" class=\"active\" checked value=\"true\" data-id=\"");
#nullable restore
#line 105 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                                                                                               Write(ViewBag.productDiscounts.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<span class=\"slider round\"></span>\r\n\t\t\t\t\t\t\t\t\t\t\t</label>\r\n");
#nullable restore
#line 108 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
										}
										else
										{

#line default
#line hidden
#nullable disable
                WriteLiteral("\t\t\t\t\t\t\t\t\t\t\t<label class=\"switch\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<input type=\"checkbox\" class=\"active\" value=\"false\" data-id=\"");
#nullable restore
#line 112 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
                                                                                                        Write(ViewBag.productDiscounts.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t<span class=\"slider round\"></span>\r\n\t\t\t\t\t\t\t\t\t\t\t</label>\r\n");
#nullable restore
#line 115 "/Users/nguyenhuunghia/DemoEcomerce/e-commerce/ElectronicCommerce/Areas/Admin/Views/AdminProductDiscount/Detail.cshtml"
										}

#line default
#line hidden
#nullable disable
                WriteLiteral("\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t</tr>\r\n\t\t</table>\r\n\t</div>\r\n\t</div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "000071c2a01e91251be2783b3676db6b6138dc6df75e8faa6cdcf3080124cd3212174", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"">
<script src=""https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js""></script>
<script src=""https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js""></script>
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ElectronicCommerce.Models.ProductDiscount> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
