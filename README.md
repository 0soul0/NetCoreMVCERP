# NetCoreMVCERP
  <h3>Create new demo erp 更新時間2021/1/29 </h3></br>
  開新專案NetCoreMVCERP(ASP.NET Core Web Application)>Web Application(Model-View-Controller)</br>
  
  <h3>2021/1/29 新增RDLC(Report Services)報表功能</h3>
    1.新增Project RDLCDesign(Windows Forms App(.NET Framework)) </br>
    2.在主專案安裝(NuGet) </br>
    . . .a.AspNetCore.Reporting by WeiGe </br>
    . . .b.System.CodeDom by Microsoft </br>
    . . .C.System.Data.SqlClient by Microsoft </br>
    3.RDLCDesign(right button)>Add>New Item>Report1.rdlc </br>
    4.Copy Report1.rdlc to NetCoreMVCERP\wwwroot\reports\Report1.rdlc </br>
    5.Change Properties of Report1.rdlc </br>
    . . .a.Build Action=>Content Copy to Output </br>
    . . .b.Directory=>Copy always</br>
    6. coding in Controller\ReportController\Print
    
