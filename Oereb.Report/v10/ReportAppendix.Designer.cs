namespace Oereb.Report.v10
{
    partial class ReportAppendix
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.NavigateToUrlAction navigateToUrlAction1 = new Telerik.Reporting.NavigateToUrlAction();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.LogoFederal = new Telerik.Reporting.PictureBox();
            this.CantonalLogo = new Telerik.Reporting.PictureBox();
            this.MunicipalityLogo = new Telerik.Reporting.PictureBox();
            this.LogoPLRCadastre = new Telerik.Reporting.PictureBox();
            this.shape7 = new Telerik.Reporting.Shape();
            this.detail = new Telerik.Reporting.DetailSection();
            this.liAppendixes = new Telerik.Reporting.List();
            this.panelAppendixes = new Telerik.Reporting.Panel();
            this.AppendixLabel = new Telerik.Reporting.TextBox();
            this.AppendixShortname = new Telerik.Reporting.TextBox();
            this.liAppendixDocPages = new Telerik.Reporting.List();
            this.panelAppendixDocPages = new Telerik.Reporting.Panel();
            this.DocPage = new Telerik.Reporting.PictureBox();
            this.PageDs = new Telerik.Reporting.ObjectDataSource();
            this.textBoxAppendixUrl = new Telerik.Reporting.TextBox();
            this.AppendixDs = new Telerik.Reporting.ObjectDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.FootCreationDate = new Telerik.Reporting.TextBox();
            this.FootExtractIdentifier = new Telerik.Reporting.TextBox();
            this.shape9 = new Telerik.Reporting.Shape();
            this.ExtractDs = new Telerik.Reporting.ObjectDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.8999999761581421D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.LogoFederal,
            this.CantonalLogo,
            this.MunicipalityLogo,
            this.LogoPLRCadastre,
            this.shape7});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // LogoFederal
            // 
            this.LogoFederal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.LogoFederal.MimeType = "";
            this.LogoFederal.Name = "LogoFederal";
            this.LogoFederal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.4000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(1.3000998497009277D));
            this.LogoFederal.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.LogoFederal.Value = "= Fields.Extract.Item1";
            // 
            // CantonalLogo
            // 
            this.CantonalLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.9795832633972168D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.CantonalLogo.MimeType = "";
            this.CantonalLogo.Name = "CantonalLogo";
            this.CantonalLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.2999999523162842D));
            this.CantonalLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.CantonalLogo.Value = "= Fields.Extract.Item2";
            // 
            // MunicipalityLogo
            // 
            this.MunicipalityLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.4985418319702148D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.MunicipalityLogo.MimeType = "";
            this.MunicipalityLogo.Name = "MunicipalityLogo";
            this.MunicipalityLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.2999999523162842D));
            this.MunicipalityLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.MunicipalityLogo.Value = "= Fields.Extract.Item3";
            // 
            // LogoPLRCadastre
            // 
            this.LogoPLRCadastre.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.890625D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.LogoPLRCadastre.MimeType = "";
            this.LogoPLRCadastre.Name = "LogoPLRCadastre";
            this.LogoPLRCadastre.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5D), Telerik.Reporting.Drawing.Unit.Cm(1D));
            this.LogoPLRCadastre.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.LogoPLRCadastre.Value = "= Fields.Extract.Item";
            // 
            // shape7
            // 
            this.shape7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.7727082967758179D));
            this.shape7.Name = "shape7";
            this.shape7.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.shape7.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.20000000298023224D);
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(29.69999885559082D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.liAppendixes});
            this.detail.Name = "detail";
            // 
            // liAppendixes
            // 
            this.liAppendixes.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= Fields.TocAppendixes"));
            this.liAppendixes.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(17.390623092651367D)));
            this.liAppendixes.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(29.699897766113281D)));
            this.liAppendixes.Body.SetCellContent(0, 0, this.panelAppendixes);
            tableGroup3.Name = "ColumnGroup";
            this.liAppendixes.ColumnGroups.Add(tableGroup3);
            this.liAppendixes.DataSource = this.AppendixDs;
            this.liAppendixes.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panelAppendixes});
            this.liAppendixes.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010007261880673468D));
            this.liAppendixes.Name = "liAppendixes";
            tableGroup4.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup4.Name = "DetailGroup";
            this.liAppendixes.RowGroups.Add(tableGroup4);
            this.liAppendixes.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.390623092651367D), Telerik.Reporting.Drawing.Unit.Cm(29.699897766113281D));
            // 
            // panelAppendixes
            // 
            this.panelAppendixes.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.AppendixLabel,
            this.AppendixShortname,
            this.liAppendixDocPages,
            this.textBoxAppendixUrl});
            this.panelAppendixes.Name = "panelAppendixes";
            this.panelAppendixes.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.390623092651367D), Telerik.Reporting.Drawing.Unit.Cm(29.699897766113281D));
            // 
            // AppendixLabel
            // 
            this.AppendixLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.7000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(1.1000000238418579D));
            this.AppendixLabel.Name = "AppendixLabel";
            this.AppendixLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.690624237060547D), Telerik.Reporting.Drawing.Unit.Cm(2.1998999118804932D));
            this.AppendixLabel.Style.Font.Bold = true;
            this.AppendixLabel.Style.Font.Name = "Cadastra";
            this.AppendixLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            this.AppendixLabel.Value = "= Fields.Description";
            // 
            // AppendixShortname
            // 
            this.AppendixShortname.Action = null;
            this.AppendixShortname.BookmarkId = "= Fields.Shortname + \" \" + Fields.Description";
            this.AppendixShortname.DocumentMapText = "= Fields.Shortname + \" \" + Fields.Description";
            this.AppendixShortname.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.1000000238418579D));
            this.AppendixShortname.Name = "AppendixShortname";
            this.AppendixShortname.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.3998997211456299D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.AppendixShortname.Style.Font.Bold = true;
            this.AppendixShortname.Style.Font.Name = "Cadastra";
            this.AppendixShortname.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            this.AppendixShortname.TocText = "= Fields.Shortname + \" \" + Fields.Description";
            this.AppendixShortname.Value = "= Fields.Shortname";
            // 
            // liAppendixDocPages
            // 
            this.liAppendixDocPages.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= Fields.Pages"));
            this.liAppendixDocPages.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(17.389999389648438D)));
            this.liAppendixDocPages.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(25D)));
            this.liAppendixDocPages.Body.SetCellContent(0, 0, this.panelAppendixDocPages);
            tableGroup1.Name = "ColumnGroup";
            this.liAppendixDocPages.ColumnGroups.Add(tableGroup1);
            this.liAppendixDocPages.DataSource = this.PageDs;
            this.liAppendixDocPages.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panelAppendixDocPages});
            this.liAppendixDocPages.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(4.6998996734619141D));
            this.liAppendixDocPages.Name = "liAppendixDocPages";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup2.Name = "DetailGroup";
            this.liAppendixDocPages.RowGroups.Add(tableGroup2);
            this.liAppendixDocPages.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.389999389648438D), Telerik.Reporting.Drawing.Unit.Cm(25D));
            // 
            // panelAppendixDocPages
            // 
            this.panelAppendixDocPages.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.DocPage});
            this.panelAppendixDocPages.Name = "panelAppendixDocPages";
            this.panelAppendixDocPages.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.389999389648438D), Telerik.Reporting.Drawing.Unit.Cm(25D));
            // 
            // DocPage
            // 
            this.DocPage.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.DocPage.MimeType = "";
            this.DocPage.Name = "DocPage";
            this.DocPage.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.389801025390625D), Telerik.Reporting.Drawing.Unit.Cm(24.999797821044922D));
            this.DocPage.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.DocPage.Value = "= Fields.Item";
            // 
            // PageDs
            // 
            this.PageDs.DataMember = "Pages";
            this.PageDs.DataSource = typeof(Oereb.Report.v10.ReportExtract.TocAppendix);
            this.PageDs.Name = "PageDs";
            // 
            // textBoxAppendixUrl
            // 
            navigateToUrlAction1.Url = "= Fields.Url";
            this.textBoxAppendixUrl.Action = navigateToUrlAction1;
            this.textBoxAppendixUrl.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.6999999284744263D), Telerik.Reporting.Drawing.Unit.Cm(3.599898099899292D));
            this.textBoxAppendixUrl.Name = "textBoxAppendixUrl";
            this.textBoxAppendixUrl.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.690001487731934D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
            this.textBoxAppendixUrl.Style.Color = System.Drawing.Color.RoyalBlue;
            this.textBoxAppendixUrl.Style.Font.Name = "Cadastra";
            this.textBoxAppendixUrl.Value = "= Fields.Url";
            // 
            // AppendixDs
            // 
            this.AppendixDs.DataSource = typeof(Oereb.Report.v10.ReportExtract.TocAppendix);
            this.AppendixDs.Name = "AppendixDs";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.40000000596046448D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.FootCreationDate,
            this.FootExtractIdentifier,
            this.shape9});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // FootCreationDate
            // 
            this.FootCreationDate.Format = "{0:G}";
            this.FootCreationDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.10583332926034927D));
            this.FootCreationDate.Name = "FootCreationDate";
            this.FootCreationDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9000003337860107D), Telerik.Reporting.Drawing.Unit.Cm(0.25D));
            this.FootCreationDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6.5D);
            this.FootCreationDate.Value = "= Fields.Extract.CreationDate";
            // 
            // FootExtractIdentifier
            // 
            this.FootExtractIdentifier.Format = "";
            this.FootExtractIdentifier.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.1749999523162842D), Telerik.Reporting.Drawing.Unit.Cm(0.10583332926034927D));
            this.FootExtractIdentifier.Name = "FootExtractIdentifier";
            this.FootExtractIdentifier.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.25D));
            this.FootExtractIdentifier.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6.5D);
            this.FootExtractIdentifier.Value = "= Fields.Extract.ExtractIdentifier";
            // 
            // shape9
            // 
            this.shape9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.026458332315087318D));
            this.shape9.Name = "shape9";
            this.shape9.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806D));
            this.shape9.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.800000011920929D);
            // 
            // ExtractDs
            // 
            this.ExtractDs.DataMember = "GetReportExtract";
            this.ExtractDs.DataSource = typeof(Oereb.Report.v10.ReportExtract);
            this.ExtractDs.Name = "ExtractDs";
            // 
            // ReportAppendix
            // 
            this.BookmarkId = "";
            this.DataSource = this.ExtractDs;
            this.DocumentMapText = "";
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = "ReportAppendix";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(18D), Telerik.Reporting.Drawing.Unit.Mm(18D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(12D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.TocText = "";
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.ObjectDataSource ExtractDs;
        private Telerik.Reporting.PictureBox LogoFederal;
        private Telerik.Reporting.PictureBox CantonalLogo;
        private Telerik.Reporting.PictureBox MunicipalityLogo;
        private Telerik.Reporting.PictureBox LogoPLRCadastre;
        private Telerik.Reporting.Shape shape7;
        private Telerik.Reporting.TextBox FootCreationDate;
        private Telerik.Reporting.TextBox FootExtractIdentifier;
        private Telerik.Reporting.Shape shape9;
        private Telerik.Reporting.List liAppendixes;
        private Telerik.Reporting.Panel panelAppendixes;
        private Telerik.Reporting.ObjectDataSource AppendixDs;
        private Telerik.Reporting.TextBox AppendixLabel;
        private Telerik.Reporting.TextBox AppendixShortname;
        private Telerik.Reporting.List liAppendixDocPages;
        private Telerik.Reporting.Panel panelAppendixDocPages;
        private Telerik.Reporting.PictureBox DocPage;
        private Telerik.Reporting.ObjectDataSource PageDs;
        private Telerik.Reporting.TextBox textBoxAppendixUrl;
    }
}