<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<input type="file" name="<%: ViewData.TemplateInfo.GetFullHtmlFieldName("") %>" id="<%: ViewData.TemplateInfo.GetFullHtmlFieldId("") %>" />
