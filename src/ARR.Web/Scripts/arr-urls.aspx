<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="arr-urls.aspx.cs" Inherits="ARR.Web.Scripts.arr_urls" ContentType="application/javascript" %>

function getArrApiUrl(action) {
    return '<%=ConfigurationManager.AppSettings["ApiUrl"] %>' + "/api/" + action + "?callback=?";
}

function getArrApiUrlPost(action) {
    return '<%=ConfigurationManager.AppSettings["ApiUrl"] %>' + "/api/" + action;
}